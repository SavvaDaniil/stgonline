using STG.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace STG.DTO.Payment
{
    public class TinkoffInitResponse
    {
        public bool Success { get; set; }
        public int PaymentId { get; set; }
        public string PaymentURL { get; set; }
        public string Message { get; set; }
        public string Details { get; set; }
        public int ErrorCode { get; set; }
        public string RebillID { get; set; }

        public TinkoffInitResponse(bool success, int paymentId, string paymentURL, string message, string details, int errorCode, string rebillID)
        {
            Success = success;
            PaymentId = paymentId;
            PaymentURL = paymentURL;
            Message = message;
            Details = details;
            ErrorCode = errorCode;
            RebillID = rebillID;
        }

        public TinkoffInitResponse(bool success, int paymentId, string paymentURL, string message, string details, int errorCode)
        {
            Success = success;
            PaymentId = paymentId;
            PaymentURL = paymentURL;
            Message = message;
            Details = details;
            ErrorCode = errorCode;
        }
    }
}
