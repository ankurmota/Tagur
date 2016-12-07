using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using tagur.Data;
using tagur.Models;

namespace tagur
{
    public static class Common
    {
        public static MainViewModel ViewModel { get; set; }
        public static StorageAccountSettings CurrentStorageAccountSettings { get; set; }
        public static TagsContext CurrentTagsContext { get; set; }
        public static string DbContextConnectionString { get; set; }
    }

    public static class GraphConstants
    {
        public static string GraphRedirectUri
        {
            get
            {
                string redirectUrl = "/api/auth/receivetoken";
                if (Environment.GetEnvironmentVariable("COMPUTERNAME") != null && Environment.GetEnvironmentVariable("COMPUTERNAME").Equals("[YOUR LOCAL MACHINE NAME]"))
                {
                    redirectUrl = "http://localhost:12778" + redirectUrl;
                }
                else
                {

                    switch (Helpers.PlatformHelper.CurrentPlatform)
                    {
                        case Helpers.PlatformType.Linux:
                            redirectUrl = "https://[yourazurelinuxwebapp.url]" + redirectUrl;
                            break;
                        case Helpers.PlatformType.Windows:
                            redirectUrl = "https://[yourazurewebapp.url]" + redirectUrl;
                            break;
                        default:
                            redirectUrl = "https://[yourazurewebapp.url]" + redirectUrl;
                            break;
                    }
                }

                return redirectUrl;
            }
        }
        
        public static string GraphAuthorizeUrl = "https://login.microsoftonline.com/common/oauth2/v2.0/authorize";
        public static string GraphTokenUrl = "https://login.microsoftonline.com/common/oauth2/v2.0/token";      

        public static string GraphClientId = "[YOUR GRAPH CLIENT ID]";
        public static string GraphScopes = "Files.ReadWrite User.Read";
        public static string GraphClientSecret = "[YOUR GRAPH CLIENT SECRET]";
    }

    public static class ServiceConstants
    {
        public static string ApiImageUrl = "[YOUR SQL SERVER BINARY TO IMAGE WRITER API URL]";        
    }

    public static class SearchConstants
    {
        public static string SearchApiKey = "[YOUR AZURE SEARCH SERVICE APP KEY]";
        public static string SearchServiceName = "[YOUR AZURE SEARCH SERVICE NAME]";
        public static string SearchIndexName = "[YOUR AZURE SEARCH SERVICE INDEX NAME]";
    }

    public static class CognitiveConstants
    {
        public static string LuisAppId = "[YOUR LUIS APP ID]";
        public static string LuisApiSubscriptionKey = "[YOUR LUIS/CS SUBSCRIPTION KEY]";       
        public static string ComputerVisionApiSubscriptionKey = "[YOUR CS COMPUTER VISION API SUBSCRIPTION KEY]";
    }

}
