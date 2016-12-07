using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace tagur.Models
{
    public class TagsHistory
    {
        public Guid Id { get; set; }
        public string Label { get; set; }
    }

    public class Tags
    {
        public Guid Id { get; set; }        
        public Guid UploadsId { get; set; }
        public string Label { get; set; }
    }

    public class Uploads
    {
        public Guid Id { get; set; }
        public string Caption { get; set; }
       
        public bool IsAdult { get; set; }

        public bool IsRacy { get; set; }

        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public byte[] Photo { get; set; }
        public DateTime UploadDate { get; set; }

    }

    public class TagSummary
    {
        public string DisplayName { get; set; }
        public int Count { get; set; }
    }
}
