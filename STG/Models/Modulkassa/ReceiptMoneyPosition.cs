using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace STG.Models.Modulkassa
{
    public class ReceiptMoneyPosition
    {
        public string paymentType { get; set; }
        public decimal sum { get; set; }

        public ReceiptMoneyPosition()
        {
        }

        public ReceiptMoneyPosition(string paymentType, decimal sum)
        {
            this.paymentType = paymentType;
            this.sum = sum;
        }
    }
}
