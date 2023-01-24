using JacRed.Models.Details;

namespace JacRed.Models.Sync
{
    public class Torrent
    {
        public string key { get; set; }

        public TorrentBaseDetails value { get; set; }
    }
}
