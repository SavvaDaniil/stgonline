using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;

namespace STG.Entities
{
    [Table("payment")]
    public class Payment
    {
        [Key, Column(Order = 0)]
        public int id { get; set; }

        [Column("id_of_user")]
        public User user { get; set; }

        [Column("id_of_lesson")]
        public Lesson lesson { get; set; }

        [Column("id_of_subscription")]
        public Subscription subscription { get; set; }

        [Column("id_of_package")]
        public Package package { get; set; }

        [Column("id_of_preuser_with_appointment")]
        public PreUserWithAppointment preUserWithAppointment { get; set; }

        [Column("id_of_statement")]
        public Statement statement { get; set; }

        [Column("is_prolongation", TypeName = "int(1)")]
        [DefaultValue("0")]
        public int isProlongation { get; set; }

        [Column("is_it_prolongation", TypeName = "int(1)")]
        [DefaultValue("0")]
        public int isItProlongation { get; set; }

        [Column("id_of_payment_that_extended")]
        public Payment id_of_payment_that_extended { get; set; }


        [Column("status", TypeName = "int(1)")]
        [DefaultValue("1")]
        public int status { get; set; }
        
        public DateTime? dateOfAdd { get; set; }

        public DateTime? dateOfPayed { get; set; }


        [Column("is_test", TypeName = "int(1)")]
        [DefaultValue("0")]
        public int isTest { get; set; }

        [Column("price", TypeName = "int(11)")]
        [DefaultValue("0")]
        public int price { get; set; }

        [Column("amount", TypeName = "int(32)")]
        [DefaultValue("0")]
        public int amount { get; set; }

        [Column("tinkoffPaymentId", TypeName = "int(32)")]
        [DefaultValue("0")]
        public int tinkoffPaymentId { get; set; }

        [Column("tinkoffPaymentURL")]
        public string tinkoffPaymentURL { get; set; }

        [Column("tinkoffErrorCode", TypeName = "int(32)")]
        [DefaultValue("0")]
        public int tinkoffErrorCode { get; set; }

        [Column("tinkoffRebillID", TypeName = "varchar(256)")]
        public string tinkoffRebillID { get; set; }

        [Column("tinkoffCanselRecurrent", TypeName = "int(1)")]
        [DefaultValue("0")]
        public int tinkoffCanselRecurrent { get; set; }

        [Column("is_reseipt_send", TypeName = "int(1)")]
        [DefaultValue("0")]
        public int isReceiptSend { get; set; }

        public DateTime? dateOfSendReseipt { get; set; }

        [Column("is_robokassa", TypeName = "int(1)")]
        [DefaultValue("0")]
        public int isRobokassa { get; set; }

        [Column("is_receipt_error", TypeName = "int(1)")]
        [DefaultValue("0")]
        public int isReceiptError { get; set; }
        public DateTime? dateOfReceiptError { get; set; }

        [Column("is_cansel", TypeName = "int(1)")]
        [DefaultValue("0")]
        public int isCansel { get; set; }
        public DateTime? dateOfCansel { get; set; }
    }
}
