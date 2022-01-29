using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace STG.Models.AmoCRM
{
    public class AmoCRMModel
    {
        public bool status { get; set; }
        public int expires_in { get; set; }
        public string token_type { get; set; }
        public string access_token { get; set; }
        public string refresh_token { get; set; }
        public int date_of_set_str { get; set; }

        public AmoCRMModel(bool status, int expires_in, string token_type, string access_token, string refresh_token, int date_of_set_str)
        {
            this.status = status;
            this.expires_in = expires_in;
            this.token_type = token_type;
            this.access_token = access_token;
            this.refresh_token = refresh_token;
            this.date_of_set_str = date_of_set_str;
        }
    }
}
