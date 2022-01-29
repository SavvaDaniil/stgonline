using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace STG.ViewModels.AmoCRM
{
    public class AmoCRMRefreshTokenResponse
    {
        public string token_type { get; set; }
        public int expires_in { get; set; }
        public string access_token { get; set; }
        public string refresh_token { get; set; }

        public AmoCRMRefreshTokenResponse(string token_type, int expires_in, string access_token, string refresh_token)
        {
            this.token_type = token_type;
            this.expires_in = expires_in;
            this.access_token = access_token;
            this.refresh_token = refresh_token;
        }
    }
}
