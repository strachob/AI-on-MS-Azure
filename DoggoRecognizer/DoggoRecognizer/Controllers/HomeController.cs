using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using DoggoRecognizer.Models;
using DoggoRecognizer.Services;
using System.IO;

namespace DoggoRecognizer.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly DogAPIService _dogService;
        private readonly CustomVisionService _visionService;

        public HomeController(ILogger<HomeController> logger, DogAPIService service, CustomVisionService customVision)
        {
            _logger = logger;
            _dogService = service;
            _visionService = customVision;
        }

        public IActionResult Index()
        {
            return View(new PhotoPost());
        }

        public async Task<IActionResult> PhotoUploaded(PhotoPost post)
        {
            var predictedBreed = await _visionService.PredictBreed(post.UploadedImage);
            if (!predictedBreed.Equals("None"))
            {
                post.DogInfoModel = await _dogService.FetchBreedInfo(predictedBreed);
            }
            else
            {
                post.DogInfoModel = new DogInfoModel();
                post.DogInfoModel.IsValid = false;
            }
            post.ImageCaption = post.UploadedImage.FileName;
            post.DogInfoModel.Image = $"data:image/jpeg;base64,{_dogService.ChangeImageToBase64(post.UploadedImage)}";
            post.ShowInfo = true;
            return View("Index", post);
        }

        public IActionResult Author()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
