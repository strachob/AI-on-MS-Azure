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
        private readonly DogAPIService _service;

        public HomeController(ILogger<HomeController> logger, DogAPIService service)
        {
            _logger = logger;
            _service = service;
        }

        public IActionResult Index()
        {
            return View(new PhotoPost());
        }

        public async Task<IActionResult> PhotoUploaded(PhotoPost post)
        {
            post.ImageCaption = post.UploadedImage.FileName;

            string imageString = "";
            if (post.UploadedImage.Length > 0)
            {
                using (var ms = new MemoryStream())
                {
                    post.UploadedImage.CopyTo(ms);
                    var fileBytes = ms.ToArray();
                    imageString = Convert.ToBase64String(fileBytes);
                }
            }
            
            post.DogInfoModel = await _service.FetchBreedInfo("Akita");
            post.DogInfoModel.Image = $"data:image/jpeg;base64,{imageString}";
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
