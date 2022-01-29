using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;

namespace STG.Entities
{
    [Table("subscription")]
    public class Subscription
    {
        [Key, Column(Order = 0)]
        public int id { get; set; }

        [Column(TypeName = "varchar(516)")]
        public string name { get; set; }

        [Column("price_for_first_time", TypeName = "int(11)")]
        [DefaultValue("0")]
        public int price_for_first_time { get; set; }

        [Column("price", TypeName = "int(11)")]
        [DefaultValue("0")]
        public int price { get; set; }

        [Column("is_discount_for_first_time", TypeName = "int(1)")]
        [DefaultValue("0")]
        public int is_discount_for_first_time { get; set; }

        [Column(TypeName = "int(3)")]
        [DefaultValue("0")]
        public int days { get; set; }

        [Column(TypeName = "int(1)")]
        [DefaultValue("0")]
        public int prolongation { get; set; }

        [Column(TypeName = "int(1)")]
        [DefaultValue("0")]
        public int active { get; set; }

        [Column("order_in_list", TypeName = "int")]
        [DefaultValue("0")]
        public int orderInList { get; set; }
    }
}
