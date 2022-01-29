using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace STG.Models.Modulkassa
{
    public class ReceiptDocResponse
    {
        public string status { get; set; }
        public string fnState { get; set; }
        public string fiscalInfo { get; set; }
        public string failureInfo { get; set; }
        public string message { get; set; }
        public string timeStatusChanged { get; set; }

        public ReceiptDocResponse()
        {
        }

        public ReceiptDocResponse(string status, string fnState, string fiscalInfo, string failureInfo, string message, string timeStatusChanged)
        {
            this.status = status;
            this.fnState = fnState;
            this.fiscalInfo = fiscalInfo;
            this.failureInfo = failureInfo;
            this.message = message;
            this.timeStatusChanged = timeStatusChanged;
        }
    }
}
