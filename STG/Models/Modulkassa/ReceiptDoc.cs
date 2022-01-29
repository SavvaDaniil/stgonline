using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace STG.Models.Modulkassa
{
    public class ReceiptDoc
    {
        [JsonProperty("id")]
        public string id { get; set; }

        [JsonProperty("checkoutDateTime")]
        public string checkoutDateTime { get; set; }

        [JsonProperty("docNum")]
        public string docNum { get; set; }

        [JsonProperty("docType")]
        public string docType { get; set; }

        [JsonProperty("email")]
        public string email { get; set; }

        [JsonProperty("inventPositions")]
        public ReceiptInventPosition[] inventPositions { get; set; }

        [JsonProperty("moneyPositions")]
        public ReceiptMoneyPosition[] moneyPositions { get; set; }

        public override string ToString()
        {
            return "id: " + id + ", checkoutDateTime: " + checkoutDateTime;
        }
    }
}
