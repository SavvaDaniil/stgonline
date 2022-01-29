using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace STG.Models.Tinkoff
{
    [Serializable]
    public class TinkoffCanselRequest
    {
        [JsonProperty("PaymentId")]
        public int PaymentId { get; set; }

        [JsonProperty("TerminalKey")]
        public string TerminalKey { get; set; }

        [JsonProperty("Token")]
        public string Token { get; set; }

        public TinkoffCanselRequest(int paymentId, string terminalKey, string token)
        {
            PaymentId = paymentId;
            TerminalKey = terminalKey;
            Token = token;
        }
    }
}
