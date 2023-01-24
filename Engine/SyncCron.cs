using JacRed.Engine.CORE;
using JacRed.Models.Sync;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace JacRed.Engine
{
    public static class SyncCron
    {
        static long lastsync = -1;

        async public static Task Run()
        {
            while (true)
            {
                try
                {
                    if (!string.IsNullOrWhiteSpace(AppInit.conf.syncapi))
                    {
                        if (lastsync == -1 && File.Exists("lastsync.txt"))
                            lastsync = long.Parse(File.ReadAllText("lastsync.txt"));

                        var root = await HttpClient.Get<RootObject>($"{AppInit.conf.syncapi}/sync/torrents?time={lastsync}", MaxResponseContentBufferSize: 200_000_000);
                        if (root?.torrents != null && root.torrents.Count > 0)
                        {
                            FileDB.AddOrUpdate(root.torrents.Select(i => i.value).ToList());

                            lastsync = root.torrents.Last().value.updateTime.ToFileTimeUtc();

                            if (root.take == root.torrents.Count)
                                continue;
                        }

                        FileDB.SaveChangesToFile();
                        File.WriteAllText("lastsync.txt", lastsync.ToString());
                    }
                }
                catch { }

                await Task.Delay(TimeSpan.FromMinutes(AppInit.conf.timeSync));
            }
        }
    }
}
