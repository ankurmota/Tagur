

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
 
using System.Text;
using System.Threading.Tasks;
using tagur.Models;

namespace tagur.Helpers
{
    public static class GraphHelper
    {
        public async static Task<string> GetAppFolderIdAsync(string accessToken)
        {
            string graphUrl = "https://graph.microsoft.com/v1.0/me/drive/special/approot";

            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);
            var uri = new Uri(graphUrl);

            var result = await client.GetStringAsync(uri);
            var appFolder = JsonConvert.DeserializeObject<AppFolderInformation>(result);

            return appFolder.id;
        }

        public async static Task<ImageCreationInformation> UploadImageToAppFolderAsync(string folderId, string fileName, byte[] bytes, string accessToken)
        {
            ImageCreationInformation imageInformation = null;
                        
            var content = new ByteArrayContent(bytes);
            var client = new HttpClient();

            try
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                var uri = new Uri("https://graph.microsoft.com/v1.0/me/" + string.Format("/drive/items/{0}:/{1}:/content", folderId, fileName));
                var response = await client.PutAsync(uri, content);
                var stream = await response.Content.ReadAsStringAsync();
                
                imageInformation = JsonConvert.DeserializeObject<ImageCreationInformation>(stream);

            }
            catch (Exception ex)
            {
            }

            return imageInformation;
        }

        public async static Task<ImageCreationInformation> GetImageDetailsAsync(string fileId, string accessToken)
        {
            ImageCreationInformation imageInformation = null;
            
            var client = new HttpClient();

            try
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                var uri = new Uri($"https://graph.microsoft.com/v1.0/me/drive/items/{fileId}");
                
                var response = await client.GetAsync(uri);
                var stream = await response.Content.ReadAsStringAsync();
                
                imageInformation = JsonConvert.DeserializeObject<ImageCreationInformation>(stream);

            }
            catch (Exception ex)
            {
            }

            return imageInformation;
        }

        public async static Task<ImageCreationInformation> UpdateImageInformationAsync(string caption, List<string> tags, string fileId, string accessToken)
        {
            ImageCreationInformation imageInformation = null;
            
            var updates = new { description = caption };
 
            var client = new HttpClient();
            
            try
            {
                
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                 
                var uri = new Uri($"https://graph.microsoft.com/v1.0/me/drive/items/{fileId}");

                var request = new HttpRequestMessage(new HttpMethod("PATCH"), uri)
                {
                    Content = new StringContent(JsonConvert.SerializeObject(updates),System.Text.Encoding.UTF8, "application/json")
                };

                var response = await client.SendAsync(request);
                var stream = await response.Content.ReadAsStringAsync();
                
                imageInformation = JsonConvert.DeserializeObject<ImageCreationInformation>(stream);

            }
            catch (Exception ex)
            {
            }

            return imageInformation;
        }

        public async static Task<string> GetAccessTokenAsync(string code)
        {
            var tokenUri = Helpers.GraphHelper.CreateOAuthTokenRequestUri(code);
            string result = null;

            using (var http = new HttpClient())
            {
                var c = tokenUri.Query.Remove(0, 1);
                var content = new StringContent(c);
                content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");
                var resp = await http.PostAsync(new Uri(GraphConstants.GraphTokenUrl), content);
                result = await resp.Content.ReadAsStringAsync();
            }

            dynamic obj = JsonConvert.DeserializeObject(result);
            var accessToken = obj.access_token.ToString();

            return accessToken;
        }

        public static Uri CreateOAuthCodeRequestUri(string UserId)
        {
            UriBuilder uri = new UriBuilder(GraphConstants.GraphAuthorizeUrl);
            var query = new StringBuilder();

            query.AppendFormat("redirect_uri={0}", Uri.EscapeUriString(GraphConstants.GraphRedirectUri));
            query.AppendFormat("&client_id={0}", Uri.EscapeUriString(GraphConstants.GraphClientId));
            query.AppendFormat("&client_secret={0}", Uri.EscapeUriString(GraphConstants.GraphClientSecret));
            query.AppendFormat("&scope={0}", GraphConstants.GraphScopes);
            query.Append("&response_type=code");
            query.Append($"&state={UserId}");

            uri.Query = query.ToString();

            return uri.Uri;
        }

        public static Uri CreateOAuthTokenRequestUri(string code, string refreshToken = "")
        {
            UriBuilder uri = new UriBuilder(GraphConstants.GraphTokenUrl);
            var query = new StringBuilder();

            query.AppendFormat("redirect_uri={0}", Uri.EscapeUriString(GraphConstants.GraphRedirectUri));
            query.AppendFormat("&client_id={0}", Uri.EscapeUriString(GraphConstants.GraphClientId));
            query.AppendFormat("&client_secret={0}", Uri.EscapeUriString(GraphConstants.GraphClientSecret));

            string grant = "authorization_code";
            if (!string.IsNullOrEmpty(refreshToken))
            {
                grant = "refresh_token";
                query.AppendFormat("&refresh_token={0}", Uri.EscapeUriString(refreshToken));
            }
            else
            {
                query.AppendFormat("&code={0}", Uri.EscapeUriString(code));
            }

            query.Append(string.Format("&grant_type={0}", grant));
            uri.Query = query.ToString();
            return uri.Uri;
        }

        public async static Task<UserProfileInformation> GetUserProfileAsync(string accessToken)
        {
            UserProfileInformation userProfile = null;

            using (var http = new HttpClient())
            {
                var client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);
                var uri = new Uri("https://graph.microsoft.com/v1.0/me");
                
                var stream = await client.GetStringAsync(uri);
                
                userProfile = JsonConvert.DeserializeObject<UserProfileInformation>(stream);

                userProfile.accessToken = accessToken;
            }

            return userProfile;
        }

        

    }
}
