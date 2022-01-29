using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace STG.Models.AmoCRM
{
    public class AmoCRMEmbedded
    {
        [JsonProperty("json")]
        public AmoCRMLead[] leads { get; set; }
    }
}
