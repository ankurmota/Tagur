using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
 
using tagur.Models;

namespace tagur.Helpers
{
    public class ImageHelper
    {
        public static async Task<byte[]> GetUpdatedImageMetadataAsync(ImageInformation image, byte[] fileBytes)
        {
            HttpClient client = new HttpClient();
            
            string caption = Uri.EscapeDataString(image.Caption);
            string tags = string.Join(",", image.Tags);

            var result = await client.PostAsync($"https://[YOUR AZURE API APP FOR IMAGE METADATA STAMPING].azurewebsites.net/api/ImageUtility?caption={caption}&tags={tags}", new ByteArrayContent(fileBytes));

            var taggedImageBytes = await result.Content.ReadAsByteArrayAsync();

            return taggedImageBytes;
        }

        public static async Task<byte[]> GetUpdatedImageMetadataAsync(ImageInformation image)
        {
            HttpClient client = new HttpClient();

            var imageResult = await client.GetAsync($"https://[YOUR AZURE BLOB STORAGE ACCOUNT URL]/app-uploads/{image.FileName}");

            byte[] fileBytes = await imageResult.Content.ReadAsByteArrayAsync();

            string caption = Uri.EscapeDataString(image.Caption);
            string tags = string.Join(",", image.Tags);

            var result = await client.PostAsync($"https://[YOUR AZURE API APP FOR IMAGE METADATA STAMPING].azurewebsites.net/api/ImageUtility?caption={caption}&tags={tags}", new ByteArrayContent(fileBytes));

            var taggedImageBytes = await result.Content.ReadAsByteArrayAsync();

            return taggedImageBytes;
        }

        public async static Task<ImageAnalysisResult> GetImageAnalysisAsync(Guid id, byte[] bytes)
        {
            HttpClient client = new HttpClient();
            
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", CognitiveConstants.ComputerVisionApiSubscriptionKey);
            
            HttpContent payload = new ByteArrayContent(bytes);
            payload.Headers.ContentType = new MediaTypeWithQualityHeaderValue("application/octet-stream");

            string analysisFeatures = "Color,ImageType,Tags,Categories,Description,Adult";

            var results = await client.PostAsync($"https://api.projectoxford.ai/vision/v1.0/analyze?visualFeatures={analysisFeatures}", payload);

            ImageAnalysisResult result = null;

            try
            {                
                var imageAnalysisResult = JsonConvert.DeserializeObject<ImageAnalysisInfo>(await results.Content.ReadAsStringAsync());

                result = new ImageAnalysisResult()
                {
                    id = id.ToString(),
                    details = imageAnalysisResult,
                    caption = imageAnalysisResult.description.captions.FirstOrDefault().text,
                    tags = imageAnalysisResult.description.tags.ToList(),
                };

            }
            catch (Exception ex)
            {

            }
            
            return result;
        }

    }
}
