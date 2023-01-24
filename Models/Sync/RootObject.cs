using System.Collections.Generic;

namespace JacRed.Models.Sync
{
    public class RootObject
    {
        public int take { get; set; }

        public List<Torrent> torrents { get; set; }
    }
}
