using System;

namespace DoggoRecognizer.Models
{
    public class CustomVisionPredictionResult
    {
        public string Id { get; set; }
        public string Project { get; set; }
        public string Tteration { get; set; }
        public DateTime Created { get; set; }
        public Prediction[] Predictions { get; set; }
    }

    public class Prediction
    {
        public float Probability { get; set; }
        public string TagId { get; set; }
        public string TagName { get; set; }
    }

}
