using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace tagur.Models
{
    public class ImageInformation
    {
        public Guid Id { get; set; }
        public string FileName { get; set; }
        public bool IsAdult { get; set; }
        public bool IsRacy { get; set; }
        public string ImageUrl { get; set; }
        public string ThumbnailUrl { get; set; }
        public byte[] Photo { get; set; }
        public string Caption { get; set; }
        public List<string> Tags { get; set; }
    }
}
