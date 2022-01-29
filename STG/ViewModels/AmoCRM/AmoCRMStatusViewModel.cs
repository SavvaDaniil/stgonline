using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace STG.ViewModels.AmoCRM
{
    public class AmoCRMStatusViewModel
    {
        public string status { get; set; }
        public string errors { get; set; }
        public bool amoCRM_status { get; set; }
        public string access_token { get; set; }

        public AmoCRMStatusViewModel(string status, string errors)
        {
            this.status = status;
            this.errors = errors;
        }

        public AmoCRMStatusViewModel(string status, string errors, bool amoCRM_status) : this(status, errors)
        {
            this.amoCRM_status = amoCRM_status;
        }

        public AmoCRMStatusViewModel(string status, string errors, bool amoCRM_status, string access_token) : this(status, errors, amoCRM_status)
        {
            this.access_token = access_token;
        }
    }
}
