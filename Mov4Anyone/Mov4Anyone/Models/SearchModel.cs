using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mov4Anyone.Models
{
    public class SearchModel
    {
        public int Id { get; set; }
        public string Type { get; set; }
    }

    public class PersonSearchModel : SearchModel
    {
        public PersonKnownFor[] KnownFor { get; set; }
    }
}
