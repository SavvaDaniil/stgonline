using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace STG.DTO.Payment
{
    public class TinkoffGetStateDTO
    {
        public bool Success { get; set; }
        public string Status { get; set; }
        public int ErrorCode { get; set; }
        public string Message { get; set; }
        public string Details { get; set; }

        public TinkoffGetStateDTO(bool success, string status, int errorCode, string message, string details)
        {
            Success = success;
            Status = status;
            ErrorCode = errorCode;
            Message = message;
            Details = details;
        }
    }
}
