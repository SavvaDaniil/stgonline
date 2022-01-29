using Newtonsoft.Json;
using STG.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace STG.ViewModels.Payment
{
    public class TinkoffInitRequest
    {

        [JsonProperty("Amount")]
        public string Amount { get; set; }

        [JsonProperty("FailURL")]
        public string FailURL { get; }

        [JsonProperty("CustomerKey")]
        public string CustomerKey { get; set; }

        [JsonProperty("OrderId")]
        public string OrderId { get; set; }

        [JsonProperty("Recurrent")]
        public string Recurrent { get; set; }

        [JsonProperty("SuccessURL")]
        public string SuccessURL { get; set; }

        [JsonProperty("NotificationURL")]
        public string NotificationURL { get; set; }

        [JsonProperty("TerminalKey")]
        public string TerminalKey { get; set; }

        [JsonProperty("Receipt")]
        public TinkoffReceipt Receipt { get; set; }

        public TinkoffInitRequest(string amount, string orderId, string terminalKey)
        {
            Amount = amount;
            OrderId = orderId;
            TerminalKey = terminalKey;
        }

        public TinkoffInitRequest(string amount, string customerKey, string orderId, string recurrent, string successURL, string notificationURL, string terminalKey, TinkoffReceipt receipt)
        {
            Amount = amount;
            CustomerKey = customerKey;
            OrderId = orderId;
            Recurrent = recurrent;
            FailURL = "https://stgonline.pro/payment/error";
            NotificationURL = notificationURL;
            SuccessURL = successURL;
            TerminalKey = terminalKey;
            Receipt = receipt;
        }







        /*
        public TinkoffInitRequest(string amount, string customerKey, string orderId, string recurrent, string terminalKey, TinkoffReceipt receipt)
        {
            Amount = amount;
            CustomerKey = customerKey;
            OrderId = orderId;
            Recurrent = recurrent;
            FailURL = "https://stgonline.pro/payment/error";
            SuccessURL = "https://stgonline.pro/payment/result_tinkoff/" + orderId;
            TerminalKey = terminalKey;
            Receipt = receipt;
        }
        */
    }
}
