using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mov4Anyone.Services
{
    public static class TMDBEndpoints
    {
        public static readonly Dictionary<string, string> APIEndpoints = new Dictionary<string, string>()
        {
            {"searchMovie", "/search/movie"},
            {"searchTV", "/search/tv"},
            {"searchPeople", "/search/person"},
            {"personDetails", "/person/{id}"},
            {"movieDetails", "/movie/{id}"},
            {"movieVideos", "/movie/{id}/videos"},
            {"tvDetails", "/tv/{id}"},
            {"tvVideos", "/tv/{id}/videos"},
            {"recommendMovie", "/movie/{id}/recommendations"},
            {"recommendTv", "/tv/{id}/recommendations"},
        };

    }
}
