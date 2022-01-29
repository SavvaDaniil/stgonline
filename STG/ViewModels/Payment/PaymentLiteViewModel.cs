using STG.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace STG.ViewModels.Payment
{
    public class PaymentLiteViewModel
    {
        public int id { get; set; }
        public STG.Entities.Lesson lesson { get; set; }
        public STG.Entities.Subscription subscription { get; set; }
        public STG.Entities.Package package { get; set; }
        public STG.Entities.PreUserWithAppointment preUserWithAppointment { get; set; }
        public STG.Entities.Statement statement { get; set; }
        public int tinkoffPaymentId { get; set; }

        public string paymentURL { get; set; }
        public int errorCode { get; set; }

        public bool status { get; set; }

        public PaymentLiteViewModel(int id)
        {
            this.id = id;
        }

        public PaymentLiteViewModel(int id, STG.Entities.Lesson lesson)
        {
            this.id = id;
            this.lesson = lesson;
        }

        public PaymentLiteViewModel(int id, STG.Entities.Subscription subscription)
        {
            this.id = id;
            this.subscription = subscription;
        }

        public PaymentLiteViewModel(int id, STG.Entities.Package package)
        {
            this.id = id;
            this.package = package;
        }

        public PaymentLiteViewModel(int id, STG.Entities.Statement statement) : this(id)
        {
            this.statement = statement;
        }

        public PaymentLiteViewModel(int id, PreUserWithAppointment preUserWithAppointment) : this(id)
        {
            this.preUserWithAppointment = preUserWithAppointment;
        }

        public PaymentLiteViewModel(int id, bool status) : this(id)
        {
            this.status = status;
        }

        public PaymentLiteViewModel(int id, int errorCode, bool status) : this(id)
        {
            this.errorCode = errorCode;
            this.status = status;
        }

        public PaymentLiteViewModel(int id, int tinkoffPaymentId, string paymentURL, int errorCode, bool status) : this(id)
        {
            this.tinkoffPaymentId = tinkoffPaymentId;
            this.paymentURL = paymentURL;
            this.errorCode = errorCode;
            this.status = status;
        }

        //для Робокассы
        public PaymentLiteViewModel(string paymentURL, int errorCode, bool status)
        {
            this.paymentURL = paymentURL;
            this.errorCode = errorCode;
            this.status = status;
        }
    }
}
