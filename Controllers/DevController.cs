using System;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using JacRed.Engine;
using Microsoft.AspNetCore.Mvc;

namespace JacRed.Controllers
{
    [Route("/dev/[action]")]
    public class DevController : Controller
    {
        public JsonResult UpdateSize()
        {
            #region getSizeInfo
            long getSizeInfo(string sizeName)
            {
                if (string.IsNullOrWhiteSpace(sizeName))
                    return 0;

                try
                {
                    double size = 0.1;
                    var gsize = Regex.Match(sizeName, "([0-9\\.,]+) (Mb|МБ|GB|ГБ|TB|ТБ)", RegexOptions.IgnoreCase).Groups;
                    if (!string.IsNullOrWhiteSpace(gsize[2].Value))
                    {
                        if (double.TryParse(gsize[1].Value.Replace(",", "."), NumberStyles.Any, CultureInfo.InvariantCulture, out size) && size != 0)
                        {
                            if (gsize[2].Value.ToLower() is "gb" or "гб")
                                size *= 1024;

                            if (gsize[2].Value.ToLower() is "tb" or "тб")
                                size *= 1048576;

                            return (long)(size * 1048576);
                        }
                    }
                }
                catch { }

                return 0;
            }
            #endregion

            foreach (var item in FileDB.masterDb.OrderBy(i => i.Value))
            {
                using (var fdb = FileDB.OpenWrite(item.Key))
                {
                    foreach (var torrent in fdb.Database)
                    {
                        torrent.Value.size = getSizeInfo(torrent.Value.sizeName);
                        torrent.Value.updateTime = DateTime.UtcNow;
                        FileDB.masterDb[item.Key] = torrent.Value.updateTime;
                    }

                    fdb.savechanges = true;
                }
            }

            FileDB.SaveChangesToFile();
            return Json(new { ok = true });
        }

        public JsonResult ResetCheckTime()
        {
            foreach (var item in FileDB.masterDb)
            {
                using (var fdb = FileDB.OpenWrite(item.Key))
                {
                    foreach (var torrent in fdb.Database)
                    {
                        torrent.Value.checkTime = DateTime.Today.AddDays(-1);
                    }

                    fdb.savechanges = true;
                }
            }

            FileDB.SaveChangesToFile();
            return Json(new { ok = true });
        }
    }
}
