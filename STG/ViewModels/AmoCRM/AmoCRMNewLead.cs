using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace STG.ViewModels.AmoCRM
{
    [Serializable]
    public class AmoCRMNewLead
    {
        [JsonProperty("name")]
        public string name { get; set; }
        [JsonProperty("price")]
        public int price { get; set; }
        [JsonProperty("pipeline_id")]
        public int pipeline_id { get; set; }

        [JsonProperty("_embedded")]
        public AmoCRMNewLead_embedded _embedded { get; set; }

        public AmoCRMNewLead()
        {
        }
    }

    [Serializable]
    public class AmoCRMNewLead_embedded
    {
        [JsonProperty("contacts")]
        public AmoCRMNewLeadContact[] contacts { get; set; }
    }

    [Serializable]
    public class AmoCRMNewLeadContact
    {
        [JsonProperty("id")]
        public int id { get; set; }
    }
}
