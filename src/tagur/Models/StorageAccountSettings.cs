using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace tagur.Models
{ 
    public class StorageAccountSettings
    {
        public string ConnectionString { get; set; }    
        public string UploadsContainerName { get; set; }
        public string DownloadsContainerName { get; set; }
    }
}
