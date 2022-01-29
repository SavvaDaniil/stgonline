using STG.Data;
using STG.Entities;
using STG.Models.Modulkassa;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace STG.Factory.Modulkassa
{
    public class ReceiptMoneyPositionFactory
    {
        private const string paymentType = "CARD";

        public ReceiptMoneyPosition create(Payment payment)
        {
            return new ReceiptMoneyPosition(paymentType, payment.price);
        }
    }
}
