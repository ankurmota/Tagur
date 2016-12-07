using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace tagur.Helpers
{
    public enum PlatformType { Any, Windows, Linux}
    public enum DatasourceType { Azure, Windows, Linux }

    public static class PlatformHelper
    {
        public static PlatformType CurrentPlatform
        {
            get
            {    
                return (Environment.GetEnvironmentVariable("HOSTNAME") == null) ? PlatformType.Windows : PlatformType.Linux;
            }
        }

        public static DatasourceType CurrentDatasource
        {
            get
            {
                return (CurrentPlatform == PlatformType.Any) ? DatasourceType.Azure : ((CurrentPlatform == PlatformType.Windows) ? DatasourceType.Windows : DatasourceType.Linux);
            }
        }
    }
}
