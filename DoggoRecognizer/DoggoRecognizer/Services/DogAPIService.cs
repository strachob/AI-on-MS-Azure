using DoggoRecognizer.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DoggoRecognizer.Services
{
    public class DogAPIService
    {
        private readonly List<BreedInfo> _breedInfos;

        public DogAPIService(IConfiguration configuration)
        {
            var list = new List<BreedInfo>();
            configuration.GetSection("DogRaceSettings").Bind(list);
            _breedInfos = list;
        }

    }
}
