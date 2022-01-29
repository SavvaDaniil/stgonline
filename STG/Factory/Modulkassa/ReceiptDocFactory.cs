using STG.Data;
using STG.Entities;
using STG.Models.Modulkassa;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace STG.Factory.Modulkassa
{
    public class ReceiptDocFactory
    {
        private ApplicationDbContext _dbc;
        public ReceiptDocFactory(ApplicationDbContext dbc)
        {
            _dbc = dbc;
        }
        public ReceiptDoc createSale(Payment payment, string username = null)
        {
            if (payment.user == null && username == null) return null;
            ReceiptInventPositionFactory receiptInventPositionFactory = new ReceiptInventPositionFactory(_dbc);
            ReceiptInventPosition receiptInventPosition = receiptInventPositionFactory.create(payment);
            if (receiptInventPosition == null) return null;

            ReceiptMoneyPositionFactory receiptMoneyPositionFactory = new ReceiptMoneyPositionFactory();

            string dateOfAddStr = ((DateTime)payment.dateOfAdd).ToString("o");

            ReceiptDoc receiptDoc = new ReceiptDoc();
            receiptDoc.id = payment.id.ToString();
            receiptDoc.checkoutDateTime = dateOfAddStr;
            receiptDoc.docNum = "Платеж №" + payment.id;
            receiptDoc.docType = "SALE";
            receiptDoc.email = (username != null ? username : payment.user.Username);
            receiptDoc.inventPositions = new ReceiptInventPosition[]
            {
                receiptInventPosition
            };
            receiptDoc.moneyPositions = new ReceiptMoneyPosition[]
            {
                receiptMoneyPositionFactory.create(payment)
            };

            return receiptDoc;
        }

    }
}
