using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace STG.ViewModels.AmoCRM
{
    [Serializable]
    public class AmoCRMRefreshTokenRequest
    {
        [JsonProperty]
        public string client_id { get; set; }
        [JsonProperty]
        public string client_secret { get; set; }
        [JsonProperty]
        public string grant_type { get; set; }
        [JsonProperty]
        public string refresh_token { get; set; }
        [JsonProperty]
        public string redirect_uri { get; set; }

        public AmoCRMRefreshTokenRequest(string client_id, string client_secret, string grant_type, string refresh_token, string redirect_uri)
        {
            this.client_id = client_id;
            this.client_secret = client_secret;
            this.grant_type = grant_type;
            this.refresh_token = refresh_token;
            this.redirect_uri = redirect_uri;
        }
    }


}
