using Microsoft.AspNetCore.Http;

namespace DoggoRecognizer.Models
{
    public class PhotoPost
    {
        public string ImageCaption { set; get; }
        public string ImageDescription { set; get; }
        public IFormFile UploadedImage { set; get; }
        public bool ShowInfo { get; set; } = false;
        public DogInfoModel DogInfoModel { get; set; }
    }
}
