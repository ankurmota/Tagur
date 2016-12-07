using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System.IO;
using Microsoft.Extensions.Options;
using tagur.Data;
using Microsoft.EntityFrameworkCore;
using tagur.Models;
using Microsoft.AspNetCore.Authorization;

namespace tagur.Controllers
{
    public class HomeController : Controller
    {
        public HomeController(IOptions<Models.StorageAccountSettings> storageAccountSettings, TagsContext tagsContext)
        {
            Common.CurrentStorageAccountSettings = storageAccountSettings.Value;
            Common.CurrentTagsContext = tagsContext;
            if (Common.ViewModel == null) Common.ViewModel = new MainViewModel();
        }

        public IActionResult Index()
        {
            Common.ViewModel.UserImages = new List<ImageInformation>();
            Common.ViewModel.SearchResultImages = new List<ImageInformation>();

            return View(Common.ViewModel);
        }

        public IActionResult Tools()
        {
            var model = this.GetFullAndPartialViewModel();
            return this.View(model);
        }

        public IActionResult Search()
        {
            var model = this.GetFullAndPartialViewModel();
            return this.View(model);
        }

        public IActionResult Reports()
        {
            var model = this.GetFullAndPartialViewModel();
            return this.View(model);
        }

        private MainViewModel GetFullAndPartialViewModel()
        {
            return Common.ViewModel;
        }

        public async Task<IActionResult> UploadSearchImageToFolderAsync(string id)
        {
            var accessToken = Common.ViewModel.User.accessToken;

            var currentImage = Common.ViewModel.SearchResultImages.Where(w => w.Id.Equals(new Guid(id))).FirstOrDefault();

            if (currentImage != null)
            {
                byte[] updatedImage = await Helpers.ImageHelper.GetUpdatedImageMetadataAsync(currentImage);

                var appFolder = await Helpers.GraphHelper.GetAppFolderIdAsync(accessToken);
                var imageInformation = await Helpers.GraphHelper.UploadImageToAppFolderAsync(appFolder, currentImage.FileName, updatedImage, accessToken);
            }

            return Json(new { Message = $"Image uploaded to {Common.ViewModel.User.givenName}'s OneDrive." });
        }

        public async Task<IActionResult> UploadImageToFolderAsync(string id)
        {
            var accessToken = Common.ViewModel.User.accessToken;

            var theList = Common.ViewModel.UserImages.ToList();

            var currentImage = Common.ViewModel.UserImages.Where(w => w.Id.Equals(new Guid(id))).FirstOrDefault();

            if (currentImage != null)
            {
                byte[] updatedImage = await Helpers.ImageHelper.GetUpdatedImageMetadataAsync(currentImage, currentImage.Photo);

                var appFolder = await Helpers.GraphHelper.GetAppFolderIdAsync(accessToken);
                var imageInformation = await Helpers.GraphHelper.UploadImageToAppFolderAsync(appFolder, currentImage.FileName, updatedImage, accessToken);
            }

            return Json(new { Message = $"Image uploaded to {Common.ViewModel.User.givenName}'s OneDrive." });
        }

        public async Task<ActionResult> GetAnalyzedImagesAsync()
        {
            await StartAnalyzingImagesAsync();

            var model = Common.ViewModel;
            return PartialView("ImageResults", model);
        }

        //USE THIS VERSION IF SENDING IMAGES TO SQL SERVER
        //public async Task<ActionResult> UploadImageAsync()
        //{
        //    bool isSavedSuccessfully = false;

        //    var request = await Request.ReadFormAsync();

        //    Common.ViewModel.Latitude = Convert.ToDouble(request["latitude"]);
        //    Common.ViewModel.Longitude = Convert.ToDouble(request["longitude"]);

        //    foreach (var file in request.Files.Where(w => w.ContentType.StartsWith("image")))
        //    {
        //        var fileId = Guid.NewGuid();

        //        string fileName = $"{fileId}.{file.FileName.Split('.').Last()}";

        //        var photoStream = file.OpenReadStream();
        //        var photo = await photoStream.ToByteArrayAsync();

        //        var image = new ImageInformation()
        //        {
        //            Id = fileId,
        //            FileName = fileName,
        //            Photo = photo,
        //            Tags = new List<string>(),
        //        };

        //        Common.ViewModel.UserImages.Add(image);

        //        Helpers.TelemetryHelper.AddEvent("FileAdded");

        //        isSavedSuccessfully = true;
        //    }

        //    if (isSavedSuccessfully)
        //    {
        //        return Json(new { Message = "Image uploaded." });
        //    }
        //    else
        //    {
        //        return Json(new { Message = "Error uploading images." });
        //    }
        //}

        //USE THIS VERSION IF SENDING IMAGES TO BLOB STORAGE
        public async Task<ActionResult> UploadImageAsync()
        {
            bool isSavedSuccessfully = false;

            var request = await Request.ReadFormAsync();

            Common.ViewModel.Latitude = Convert.ToDouble(request["latitude"]);
            Common.ViewModel.Longitude = Convert.ToDouble(request["longitude"]);

            foreach (var file in request.Files.Where(w => w.ContentType.StartsWith("image")))
            {
                var fileId = await file.UploadAsObjectAsync();

                string fileName = $"{fileId}.{file.FileName.Split('.').Last()}";

                var photoStream = file.OpenReadStream();
                var photo = await photoStream.ToByteArrayAsync();

                var image = new ImageInformation()
                {
                    Id = fileId,
                    FileName = fileName,
                    Photo = photo,
                    Tags = new List<string>(),
                };

                Common.ViewModel.UserImages.Add(image);

                Helpers.TelemetryHelper.AddEvent("FileAdded");

                isSavedSuccessfully = true;
            }

            if (isSavedSuccessfully)
            {
                return Json(new { Message = "File saved" });
            }
            else
            {
                return Json(new { Message = "Error in saving file" });
            }
        }

        //USE THIS VERSION IF SENDING IMAGES TO SQL SERVER INSTEAD OF BLOB STORAGE
        //private async Task<bool> StartAnalyzingImagesAsync()
        //{
        //    double latitude = Common.ViewModel.Latitude;
        //    double longitude = Common.ViewModel.Longitude;

        //    string currentOs = "&os=" + Helpers.PlatformHelper.CurrentPlatform;

        //    foreach (var image in Common.ViewModel.UserImages)
        //    {
        //        var analysisResults = await Helpers.ImageHelper.GetImageAnalysisAsync(image.Id, image.Photo);

        //        image.Caption = analysisResults.caption;
        //        image.Tags = analysisResults.tags;
        //        image.IsRacy = analysisResults.details.adult.isRacyContent;
        //        image.IsAdult = analysisResults.details.adult.isAdultContent;

        //        var uploadResult = await Helpers.StorageHelper.SendImageToDatabaseAsync(image, image.Photo, latitude, longitude);

        //        if (!uploadResult.IsSuccessful)
        //        {
        //            image.Caption = uploadResult.ErrorMessage;
        //        }

        //        image.ImageUrl = ServiceConstants.ApiImageUrl + image.Id + currentOs;
        //        image.ThumbnailUrl = ServiceConstants.ApiImageUrl + image.Id + currentOs;

        //        Helpers.TelemetryHelper.AddMetric("AddedTags", image.Tags.Count);

        //        if (image.IsRacy) Helpers.TelemetryHelper.AddMetric("IsRacy", 1);
        //        if (image.IsAdult) Helpers.TelemetryHelper.AddMetric("IsAdult", 1);

        //        Helpers.StreamHelper.SendStreamToAnalyticsAsync(Common.ViewModel.Latitude, Common.ViewModel.Longitude, image.Tags.Count);

        //    }

        //    return true;
        //}

        //USE THIS VERSION IF SENDING IMAGES TO BLOB STORAGE INSTEAD OF SQL SERVER
        private async Task<bool> StartAnalyzingImagesAsync()
        {
            double latitude = Common.ViewModel.Latitude;
            double longitude = Common.ViewModel.Longitude;

            foreach (var image in Common.ViewModel.UserImages)
            {
                var updatedImage = await Helpers.StorageHelper.RetrieveImageFromObjectStorageAsync(image);

                Helpers.StreamHelper.SendStreamToAnalyticsAsync(Common.ViewModel.Latitude, Common.ViewModel.Longitude, updatedImage.Tags.Count);

                await Helpers.StorageHelper.SendImageToDatabaseAsync(updatedImage, image.Photo, latitude, longitude);

                Helpers.TelemetryHelper.AddMetric("AddedTags", updatedImage.Tags.Count);

                if (updatedImage.IsRacy) Helpers.TelemetryHelper.AddMetric("IsRacy", 1);
                if (updatedImage.IsAdult) Helpers.TelemetryHelper.AddMetric("IsAdult", 1);
            }

            return true;
        }

        public IActionResult Error()
        {
            return View();
        }

        public async Task<JsonResult> SuggestAsync(string term)
        {
            var suggestions = await Helpers.SearchHelper.GetSuggestionsAsync(term);

            return new JsonResult(suggestions);
        }

        public async Task<ActionResult> GetSearchResultImagesAsync(string searchQuery)
        {
            var searchResults = await Helpers.SearchHelper.SearchAsync(searchQuery);

            Common.ViewModel.SearchResultImages = new List<ImageInformation>();

            foreach (var result in searchResults.Take(8))
            {
                var imageResult = await Helpers.StorageHelper.SearchImagesInObjectStorageAsync(result.Id);

                if (imageResult != null) Common.ViewModel.SearchResultImages.Add(imageResult);
            }

            var model = Common.ViewModel;
            return PartialView("SearchResults", model);
        }
    }
}
