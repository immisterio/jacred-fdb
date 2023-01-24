using System;
using System.Collections.Generic;
using System.Linq;
using JacRed.Engine;
using JacRed.Models.Details;
using Microsoft.AspNetCore.Mvc;

namespace JacRed.Controllers
{
    [Route("/sync/[action]")]
    public class SyncController : Controller
    {
        public JsonResult Torrents(long time)
        {
            if (!AppInit.conf.opensync || time == 0)
                return Json(new List<string>());

            int take = 2_000;
            DateTime lastsync = time == -1 ? default : DateTime.FromFileTimeUtc(time);

            var torrents = new Dictionary<string, TorrentDetails>();
            foreach (var item in FileDB.masterDb.OrderBy(i => i.Value).Where(i => i.Value > lastsync))
            {
                foreach (var torrent in FileDB.OpenRead(item.Key).OrderBy(i => i.Value.updateTime).Where(i => i.Value.updateTime > lastsync))
                    torrents.Add(torrent.Key, torrent.Value);

                if (torrents.Count >= take)
                    break;
            }

            return Json(new { take, torrents = torrents.Take(take) });
        }
    }
}
