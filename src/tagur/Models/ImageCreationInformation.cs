using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace tagur.Models
{  

    public class ImageCreationInformation
    {       
        public string microsoftgraphdownloadUrl { get; set; }
      
        public string cTag { get; set; }
        public string eTag { get; set; }
        public string id { get; set; }
       
        public string name { get; set; }
    }

    public class Createdby
    {
        public Application application { get; set; }
        public User user { get; set; }
    }

    public class Application
    {
        public string displayName { get; set; }
        public string id { get; set; }
    }

    public class User
    {
        public string displayName { get; set; }
        public string id { get; set; }
    }

    public class Lastmodifiedby
    {
        public Application1 application { get; set; }
        public User1 user { get; set; }
    }

    public class Application1
    {
        public string displayName { get; set; }
        public string id { get; set; }
    }

    public class User1
    {
        public string displayName { get; set; }
        public string id { get; set; }
    }

}
