using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace STG.DTO.AmoCRM
{
    public class TinkoffNotificationDTO
    {
        public string Status { get; set; }
        public bool Success { get; set; }
        public int OrderId { get; set; }
        public int PaymentId { get; set; }
        public string RebillId { get; set; }

        public string OrderIdStr { get; set; }
        public string PaymentIdStr { get; set; }

        public TinkoffNotificationDTO()
        {
        }


        public override string ToString()
        {
            return "Success: " + Success + ", OrderId:" + OrderId + ", PaymentId: " + PaymentId + ", RebillId: " + RebillId;
        }
    }
}
