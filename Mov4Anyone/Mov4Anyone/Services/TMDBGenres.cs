using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mov4Anyone.Services
{
    public class TMDBGenres
    {
        public Dictionary<string, int> MovieGenres { get; set; } = new Dictionary<string, int>()
        {
            {"Action", 28},
            {"Adventure", 12},
            {"Animation", 16},
            {"Comedy", 35},
            {"Crime", 80},
            {"Documentary", 99},
            {"Drama", 18 },
            {"Family", 10751 },
            {"Fantasy", 14 },
            {"Drama", 18 },
            {"History", 36 },
            {"Horror", 27 },
            {"Mystery", 9648 },
            {"Romance", 10749 },
            {"Science Fiction", 878 },
            {"TV Movie", 10770 },
            {"Thriller", 53 },
            {"War", 10752 },
            {"Western", 37 }
        };

        public Dictionary<string, int> TVGenres { get; set; } = new Dictionary<string, int>()
        {
            {"Action & Adventure", 10759},
            {"Animation", 16},
            {"Comedy", 35},
            {"Crime", 80},
            {"Documentary", 99},
            {"Drama", 18 },
            {"Family", 10751 },
            {"Kids", 10762 },
            {"Mystery", 9648 },
            {"News", 10763 },
            {"Reality", 10764 },
            {"Sci-Fi & Fantasy", 10765 },
            {"Soap", 10766 },
            {"Talk", 10767 },
            {"War & Politics", 10768 },
            {"Western", 37 }
        };
    }
}
