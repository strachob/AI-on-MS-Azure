using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mov4Anyone.Bots
{
    public class SpeechDetectorModel
    {
        [JsonProperty("Speek")]
        public bool SpeechOn { get; set; }
    }
}
