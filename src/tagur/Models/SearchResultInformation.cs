using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace tagur.Models
{
    public class SearchResultInformation
    {
        public Guid Id { get; set; }
        public string Caption { get; set; }
        public List<string> Tags { get; set; }
    }
}
