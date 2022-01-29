using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace STG.Models.AmoCRM
{
    [Serializable]
    public class AmoCRMNewContact
    {
        [JsonProperty("id")]
        public int id { get; set; }
        [JsonProperty("name")]
        public string name { get; set; }
        [JsonProperty("first_name")]
        public string first_name { get; set; }
        [JsonProperty("last_name")]
        public string last_name { get; set; }

        [JsonProperty("custom_fields_values")]
        public AmoCRMContactData[] custom_fields_values { get; set; }
    }



    [Serializable]
    public class AmoCRMContactData
    {
        [JsonProperty("field_id")]
        public int field_id { get; set; }
        [JsonProperty("field_name")]
        public string field_name { get; set; }
        [JsonProperty("field_code")]
        public string field_code { get; set; }
        [JsonProperty("field_type")]
        public string field_type { get; set; }
        [JsonProperty("values")]
        public AmoCRMContactDataField[] values { get; set; }
    }


    [Serializable]
    public class AmoCRMContactDataField
    {
        [JsonProperty("value")]
        public string value { get; set; }
        [JsonProperty("enum_code")]
        public string enum_code { get; set; }
    }
}
