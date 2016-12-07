using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace tagur.Models
{
    public class MainViewModel
    {
        public MainViewModel()
        {
            this.IsWindows = Helpers.PlatformHelper.CurrentPlatform != Helpers.PlatformType.Linux;

            this.OsLabel = (this.IsWindows) ? "windows" : "linux";
            this.OsDescription = System.Runtime.InteropServices.RuntimeInformation.OSDescription;

            this.Tags = new List<TagSummary>();
            this.UserImages = new List<ImageInformation>();
            this.SearchResultImages = new List<ImageInformation>();
            this.User = new UserProfileInformation() { givenName = "Sign in" };
        }

        public bool IsWindows { get; set; }
        public string OsLabel { get; set; }
        public string OsDescription { get; set; }
       
        public UserProfileInformation User { get; set; }

        public IEnumerable<TagSummary> Tags { get; set; }
        public List<ImageInformation> UserImages { get; set; }
        public List<ImageInformation> SearchResultImages { get; set; }

        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}
