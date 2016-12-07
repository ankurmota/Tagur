using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using tagur.Models;

namespace tagur.Helpers
{
    public static class StorageHelper
    {
        public async static Task<ImageUploadResult> SendImageToDatabaseAsync(ImageInformation image, byte[] photo, double latitude, double longitude)
        {
            ImageUploadResult uploadResult = new ImageUploadResult()
            {
                ErrorMessage = "",
                IsSuccessful = false,
            };

            try
            {
                var upload = new Uploads()
                {
                    Id = image.Id,
                    Caption = image.Caption,
                    IsAdult = image.IsAdult,
                    IsRacy = image.IsRacy,
                    Latitude = latitude,
                    Longitude = longitude,
                    Photo = photo,
                    UploadDate = DateTime.Now,

                };

                Common.CurrentTagsContext.Uploads.Add(upload);

                List<Tags> tags = (from tag in image.Tags
                                   select new Tags()
                                   {
                                       Id = Guid.NewGuid(),
                                       Label = tag,
                                       UploadsId = image.Id,

                                   }).ToList();

                List<TagsHistory> tagsHistory = (from tag in tags
                                                 select new TagsHistory()
                                                 {
                                                     Id = tag.Id,
                                                     Label = tag.Label,

                                                 }).ToList();

                Common.CurrentTagsContext.Tags.AddRange(tags);
               Common.CurrentTagsContext.TagsHistory.AddRange(tagsHistory);

                var updates = await Common.CurrentTagsContext.SaveChangesAsync();

                uploadResult.IsSuccessful = true;
            }
            catch (Exception ex)
            {
                uploadResult.ErrorMessage = ex.Message;
            }

            return uploadResult;
        }

        public async static Task<byte[]> RetrieveOriginalImageFromObjectStorageAsync(ImageInformation image)
        {
            StorageAccountSettings storageAccountSettings = Common.CurrentStorageAccountSettings;

            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(storageAccountSettings.ConnectionString);

            CloudBlobClient client = storageAccount.CreateCloudBlobClient();
            CloudBlobContainer container = client.GetContainerReference(storageAccountSettings.UploadsContainerName);
            CloudBlockBlob blob = null;

            byte[] originalImageBytes = null;

            bool blobExists = false;

            try
            {
                blob = container.GetBlockBlobReference(image.FileName);
                blobExists = await blob.ExistsAsync();

                if (blobExists)
                {
                    originalImageBytes = new byte[blob.StreamWriteSizeInBytes];

                    await blob.DownloadToByteArrayAsync(originalImageBytes, 0);
                }
            }
            catch (Exception ex)
            {

            }

            return originalImageBytes;
        }

        public async static Task<ImageInformation> RetrieveImageFromObjectStorageAsync(ImageInformation image)
        {
            StorageAccountSettings storageAccountSettings = Common.CurrentStorageAccountSettings;

            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(storageAccountSettings.ConnectionString);

            CloudBlobClient client = storageAccount.CreateCloudBlobClient();
            CloudBlobContainer container = client.GetContainerReference(storageAccountSettings.DownloadsContainerName);
            CloudBlockBlob blob = null;


            bool blobExists = false;

            try
            {
                do
                {
                    blob = container.GetBlockBlobReference(image.FileName);
                    blobExists = await blob.ExistsAsync();

                    if (blobExists)
                    {
                       

                        image.Photo = new byte[blob.Properties.Length];
                         await blob.DownloadToByteArrayAsync(image.Photo, 0);

                       image.ThumbnailUrl = blob.Uri.OriginalString;
                        image.ImageUrl = blob.Uri.OriginalString.Replace("-downloads", "-uploads");

                        await blob.FetchAttributesAsync();

                        image.Caption = (blob.Metadata.ContainsKey("caption")) ? blob.Metadata["caption"] + "" : "(unable to caption)";
                        image.Tags = (blob.Metadata.ContainsKey("tags")) ? (blob.Metadata["tags"] + "").Split(',').ToList() : new List<string>();
                    }
                    else
                    {
                        await Task.Delay(500);
                    }

                }
                while (!blobExists);

            }
            catch (Exception ex)
            {

            }

            return image;
        }

        public async static Task<ImageInformation> SearchImagesInObjectStorageAsync(Guid id)
        {
            ImageInformation image = null;

            StorageAccountSettings storageAccountSettings = Common.CurrentStorageAccountSettings;

            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(storageAccountSettings.ConnectionString);

            CloudBlobClient client = storageAccount.CreateCloudBlobClient();
            CloudBlobContainer container = client.GetContainerReference(storageAccountSettings.DownloadsContainerName);
            CloudBlockBlob blob = null;


            bool blobExists = false;

            try
            {
                string fileName = $"{id}.jpg";

                blob = container.GetBlockBlobReference(fileName);
                blobExists = await blob.ExistsAsync();

                if (blobExists)
                {
                    await blob.FetchAttributesAsync();
                    image = new ImageInformation()
                    {
                        Id = id,

                        FileName = fileName,
                        
                        ThumbnailUrl = blob.Uri.OriginalString,
                        ImageUrl = blob.Uri.OriginalString.Replace("-downloads", "-uploads"),

                        Caption = (blob.Metadata.ContainsKey("caption")) ? blob.Metadata["caption"] + "" : "(unable to caption)",
                        Tags = (blob.Metadata.ContainsKey("tags")) ? (blob.Metadata["tags"] + "").Split(',').ToList() : new List<string>(),

                    };
                }
            }
            catch (Exception ex)
            {

            }

            return image;
        }
    }
}
