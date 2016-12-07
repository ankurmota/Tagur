using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace tagur
{
    public static class StorageExtensions
    {
        public async static Task<byte[]> ToByteArrayAsync(this System.IO.Stream stream)
        {
            Int32 length = stream.Length > Int32.MaxValue ? Int32.MaxValue : Convert.ToInt32(stream.Length);
            byte[] buffer = new Byte[length];
            await stream.ReadAsync(buffer, 0, length);
            return buffer;
        }

        public static async Task<Guid> UploadAsObjectAsync(this Microsoft.AspNetCore.Http.IFormFile file)
        {
            Guid fileId = Guid.NewGuid();

            Models.StorageAccountSettings storageAccountSettings = Common.CurrentStorageAccountSettings;

            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(storageAccountSettings.ConnectionString);

            CloudBlobClient client = storageAccount.CreateCloudBlobClient();
            CloudBlobContainer container = client.GetContainerReference(storageAccountSettings.UploadsContainerName);
            CloudBlockBlob blob = null;
            
            try
            {
                string fileName = $"{fileId}.{file.FileName.Split('.').Last()}";

                blob = container.GetBlockBlobReference(fileName);

                using (var outputStream = await blob.OpenWriteAsync())
                {
                    await file.CopyToAsync(outputStream);
                }
            }
            catch (Exception ex)
            {

            }

            return fileId;
        }



    }
}
