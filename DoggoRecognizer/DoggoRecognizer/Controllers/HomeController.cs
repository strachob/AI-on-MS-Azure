using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using DoggoRecognizer.Models;
using DoggoRecognizer.Services;

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

        public IActionResult PhotoUploaded(PhotoPost post)
        {
            post.ImageCaption = post.MyImage.FileName;
            return null;
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
