using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace STG.Models
{
    public class AmoCRMOauthAccessTokenRequest
    {
        [JsonProperty("client_id")]
        public string client_id { get; set; }
        [JsonProperty("client_secret")]
        public string client_secret { get; set; }
        [JsonProperty("grant_type")]
        public string grant_type { get; }
        [JsonProperty("code")]
        public string code { get; set; }
        [JsonProperty("redirect_uri")]
        public string redirect_uri { get; set; }

    }
}
