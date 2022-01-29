using STG.Data;
using STG.Entities;
using STG.Models.Modulkassa;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace STG.Factory.Modulkassa
{
    public class ReceiptInventPositionFactory
    {
        private ApplicationDbContext _dbc;
        public ReceiptInventPositionFactory(ApplicationDbContext dbc)
        {
            _dbc = dbc;
        }
        private const int varTagToNDS = 1104;

        public ReceiptInventPosition create(Payment payment)
        {
            ReceiptInventPosition receiptInventPosition = null;
            if (payment.lesson != null)
            {
                receiptInventPosition = new ReceiptInventPosition(
                    "Доступ к уроку id" + payment.lesson.id,
                    payment.price,
                    1,
                    payment.lesson.id.ToString()
                );
            }
            else if (payment.subscription != null)
            {
                receiptInventPosition = new ReceiptInventPosition(
                    "Подписка id" + payment.subscription.id,
                    payment.price,
                    1,
                    payment.subscription.id.ToString()
                );
            }
            else if (payment.package != null)
            {
                receiptInventPosition = new ReceiptInventPosition(
                    "Программа id" + payment.package.id,
                    payment.price,
                    1,
                    payment.package.id.ToString()
                );
            }
            else if (payment.preUserWithAppointment != null)
            {
                receiptInventPosition = new ReceiptInventPosition(
                    "Заявка на наставничество перед регистрацией id" + payment.preUserWithAppointment.id,
                    payment.price,
                    1,
                    payment.preUserWithAppointment.id.ToString()
                );
            }
            else if (payment.statement != null)
            {
                receiptInventPosition = new ReceiptInventPosition(
                    "Заявка на наставничество id" + payment.statement.id,
                    payment.price,
                    1,
                    payment.statement.id.ToString()
                );
            }
            if (receiptInventPosition == null) return null;

            receiptInventPosition.vatTag = varTagToNDS;

            return receiptInventPosition;
        }
    }
}
