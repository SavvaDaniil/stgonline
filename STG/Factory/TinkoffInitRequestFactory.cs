using STG.Data;
using STG.Models;
using STG.Models.Tinkoff;
using STG.ViewModels.Payment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using STG.Entities;

namespace STG.Factory
{
    public class TinkoffInitRequestFactory
    {
        private const string TerminalKeyDEMO = "XXXXXXXXXXXXXXXXXXXXX";
        private const string PasswordDEMO = "XXXXXXXXXXXXXXXXXXXXX";
        private const string TerminalKey = "XXXXXXXXXXXXXXXXXXXXX";
        private const string Password = "XXXXXXXXXXXXXXXXXXXXX";

        public TinkoffInitRequest createForPreUserWithAppointment(string domainHost, string useremail, Payment payment, string terminalKey)
        {
            return create(domainHost, payment.preUserWithAppointment.id , useremail, payment, terminalKey);

        }
        public TinkoffInitRequest createForUsual(string domainHost, Payment payment, string terminalKey)
        {
            return create(domainHost, payment.user.Id, payment.user.Username, payment, terminalKey);
        }

        private TinkoffInitRequest create(string domainHost, int id_of_user, string useremail, Payment payment, string terminalKey)
        {
            int amountPennies = payment.price * 100;
            string name_of_item;
            if (payment.lesson != null)
            {
                name_of_item = payment.lesson.name;
            } else if (payment.subscription != null && payment.isItProlongation == 1)
            {
                name_of_item = "Автопродление подписки: " + payment.subscription.name;
            } else if (payment.subscription != null)
            {
                name_of_item = payment.subscription.name;
            } else if(payment.preUserWithAppointment != null)
            {
                name_of_item = "Заявка на наставничество №" + payment.preUserWithAppointment.id;
            }
            else if (payment.package != null)
            {
                name_of_item = "Оплата программы: id"+ payment.package.id +" "+ payment.package.name;
            }
            else if (payment.statement != null)
            {
                name_of_item = "Оплата за наставничества: id" + payment.statement.id;
            }
            else
            {
                name_of_item = "Ошибка имени товара";
            }


            TinkoffReceipt tinkoffReceipt = new TinkoffReceipt(
                useremail,
                name_of_item,
                1,
                amountPennies
            );

            string successURL = "https://"+ domainHost + "/payment/result_tinkoff/" + payment.id;
            string notificationURL = "https://stgonline.pro/api/payment/notification";

            //new List<string> { (payment.isProlongation == 1 ? "T" : "O") },
            return new TinkoffInitRequest(
                amountPennies.ToString(),
                id_of_user.ToString(),
                payment.id.ToString(),
                (payment.isProlongation == 1 ? "Y" : null),
                successURL,
                notificationURL,
                terminalKey,
                tinkoffReceipt
            );
        }


        public TinkoffChargeRequest createForProlongationCharge(bool isTest, string useremail, int tinkoffPaymentId, string tinkoffRebuildId)
        {

            var crypt = new System.Security.Cryptography.SHA256Managed();
            StringBuilder hash = new System.Text.StringBuilder();
            byte[] crypto = crypt.ComputeHash(Encoding.UTF8.GetBytes(
                useremail
                + (isTest ? PasswordDEMO : Password)
                + tinkoffPaymentId
                + tinkoffRebuildId
                + "true"
                + (isTest ? TerminalKeyDEMO : TerminalKey))
            );
            foreach (byte theByte in crypto)
            {
                hash.Append(theByte.ToString("x2"));
            }
            string token = hash.ToString();
            System.Diagnostics.Debug.WriteLine("token: " + token);

            return new TinkoffChargeRequest(
                useremail,
                tinkoffPaymentId,
                tinkoffRebuildId,
                true,
                (isTest ? TerminalKeyDEMO : TerminalKey),
                token
            );
        }

        public TinkoffCanselRequest createForCansel(Payment payment)
        {
            var crypt = new System.Security.Cryptography.SHA256Managed();
            StringBuilder hash = new System.Text.StringBuilder();
            byte[] crypto = crypt.ComputeHash(Encoding.UTF8.GetBytes(
                (payment.isTest == 1 ? PasswordDEMO : Password)
                + payment.tinkoffPaymentId
                + (payment.isTest == 1 ? TerminalKeyDEMO : TerminalKey))
            );
            foreach (byte theByte in crypto)
            {
                hash.Append(theByte.ToString("x2"));
            }
            string token = hash.ToString();

            return new TinkoffCanselRequest(
                payment.tinkoffPaymentId,
                (payment.isTest == 1 ? TerminalKeyDEMO : TerminalKey),
                token
            );
        }
    }
}
