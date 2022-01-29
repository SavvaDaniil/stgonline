using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace STG.Entities
{
    [Table("region")]
    public class Region
    {
        [Key, Column(Order = 0)]
        public int id { get; set; }

        [Column(TypeName = "varchar(256)")]
        public string name { get; set; }

        [Column("order_in_list", TypeName = "int")]
        [System.ComponentModel.DefaultValue("0")]
        public int orderInList { get; set; }
    }
}
