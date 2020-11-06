using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mov4Anyone.Models
{
    public class Videos
    {
        public int Id { get; set; }
        public VideoResult[] Results { get; set; }
    }

    public class VideoResult
    {
        public string Id { get; set; }
        public string Key { get; set; }
        public string Name { get; set; }
        public string Site { get; set; }
        public int Size { get; set; }
        public string Type { get; set; }
    }
}
