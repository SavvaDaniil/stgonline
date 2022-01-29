using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using STG.Models;
using STG.Entities;

namespace STG.Abstract
{
    abstract public class PurchaseAbstract
    {
        [Key, Column(Order = 0)]
        public int id { get; set; }

        public User user { get; set; }
        public Payment payment { get; set; }

        [Column(TypeName = "int(1)")]
        [DefaultValue("1")]
        public int active { get; set; }

        [Column(TypeName = "int(3)")]
        [DefaultValue("0")]
        public int days { get; set; }

        [Column(TypeName = "int(1)")]
        [DefaultValue("0")]
        public int isPayed { get; set; }

        [Column("order_in_list", TypeName = "int")]
        [DefaultValue("0")]
        public int orderInList { get; set; }

        public DateTime? dateOfAdd { get; set; }
        public DateTime? dateOfActivation { get; set; }
        public DateTime? dateOfMustBeUsedTo { get; set; }
    }
}
