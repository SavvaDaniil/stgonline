using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace STG.Models.Tinkoff
{
    public class TinkoffCanselResponse
    {
        public bool Success { get; set; }
        public string ErrorCode { get; set; }
        public string TerminalKey { get; set; }
        public string Status { get; set; }
        public string PaymentId { get; set; }
        public string OrderId { get; set; }
        public int OriginalAmount { get; set; }
        public int NewAmount { get; set; }

        public TinkoffCanselResponse(bool success, string errorCode, string terminalKey, string status, string paymentId, string orderId, int originalAmount, int newAmount)
        {
            Success = success;
            ErrorCode = errorCode;
            TerminalKey = terminalKey;
            Status = status;
            PaymentId = paymentId;
            OrderId = orderId;
            OriginalAmount = originalAmount;
            NewAmount = newAmount;
        }
    }
}
