using System;
using System.Collections.Generic;

namespace JacRed.Models.Details
{
    public class TorrentDetails : TorrentBaseDetails, ICloneable
    {
        public double size { get; set; }

        public int quality { get; set; }

        public string videotype { get; set; }

        public HashSet<string> voices { get; set; } = new HashSet<string>();

        public HashSet<int> seasons { get; set; } = new HashSet<int>();


        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}
