using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace STG.Entities
{
    [Table("tariff")]
    public class Tariff
    {
        [Key, Column(Order = 0)]
        public int id { get; set; }

        [Column(TypeName = "varchar(256)")]
        public string name { get; set; }

        [Column(TypeName = "int(1)")]
        [DefaultValue("0")]
        public int active { get; set; }

        [Column("order_in_list", TypeName = "int")]
        [DefaultValue("0")]
        public int orderInList { get; set; }
    }
}
