using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using STG.Abstract;
using System.ComponentModel;

namespace STG.Entities
{
    [Table("purchase_subscription")]
    public class PurchaseSubscription : PurchaseAbstract
    {
        public Subscription subscription { get; set; }

        [Column("is_prolongation", TypeName = "int(1)")]
        [DefaultValue("0")]
        public int isProlongation { get; set; }

        public DateTime? expirationDate { get; set; }

        /*
        [Key, Column(Order = 0)]
        public int id { get; set; }

        public User user { get; set; }
        public Subscription subscription { get; set; }
        public Payment payment { get; set; }

        [Column(TypeName = "int(3)")]
        public int days { get; set; }

        [Column(TypeName = "int(1)")]
        [System.ComponentModel.DefaultValue("1")]
        public int active { get; set; }

        [Column(TypeName = "int(1)")]
        [System.ComponentModel.DefaultValue("0")]
        public int isPayed { get; set; }

        public DateTime dateOfAdd { get; set; }
        public DateTime dateOfActivation { get; set; }
        public DateTime dateOfMustBeUsedTo { get; set; }
        public DateTime expirationDate { get; set; }
        */
    }
}
