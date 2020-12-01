
namespace DoggoRecognizer.Models
{
    public class DogInfoModel
    {
        public DogAPIInfo ApiInfo { get; set; }
        public string WikiPath { get; set; }
        public int BreedId { get; set; }
        public string Image { set; get; }
        public bool IsValid { get; set; } = true;
    }
}
