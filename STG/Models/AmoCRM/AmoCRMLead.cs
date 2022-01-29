using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace STG.Models.AmoCRM
{
    public class AmoCRMLead
    {
        [JsonProperty("id")]
        public int id { get; set; }
        [JsonProperty("name")]
        public string name { get; set; }
        [JsonProperty("price")]
        public int price { get; set; }
        [JsonProperty("pipeline_id")]
        public int pipeline_id { get; set; }

        //[JsonProperty("_embedded[contacts][0][id]")]
        //public int id_of_contact { get; set; }
    }
}
