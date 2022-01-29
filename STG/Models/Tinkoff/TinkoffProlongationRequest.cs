using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace STG.Models
{
    public class TinkoffProlongationRequest
    {
        //[JsonProperty("InfoEmail")]
        //public string InfoEmail { get; set; }

        [JsonProperty("PaymentId")]
        public int PaymentId { get; set; }

        [JsonProperty("RebillId")]
        public string RebillId { get; set; }

        //[JsonProperty("SendEmail")]
        //public bool SendEmail { get; set; }

        [JsonProperty("TerminalKey")]
        public string TerminalKey { get; set; }

        [JsonProperty("Token")]
        public string Token { get; set; }

        public TinkoffProlongationRequest(int paymentId, string rebillId, string terminalKey, string token)
        {
            PaymentId = paymentId;
            RebillId = rebillId;
            TerminalKey = terminalKey;
            Token = token;
        }

        /*
        public TinkoffProlongationRequest(string infoEmail, int paymentId, string rebillId, bool sendEmail, string terminalKey, string token)
        {
            InfoEmail = infoEmail;
            PaymentId = paymentId;
            RebillId = rebillId;
            SendEmail = sendEmail;
            TerminalKey = terminalKey;
            Token = token;
        }
        */


    }
}
