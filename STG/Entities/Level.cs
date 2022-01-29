using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace STG.Entities
{
    [Table("level")]
    public class Level
    {
        [Key, Column("id", Order = 0)]
        public int Id { get; set; }

        [Column("name",TypeName = "varchar(255)")]
        public string Name { get; set; }

        [Column(TypeName = "int(1)")]
        [System.ComponentModel.DefaultValue("1")]
        public int active { get; set; }

        [Column("order_in_list", TypeName = "int")]
        [System.ComponentModel.DefaultValue("0")]
        public int orderInList { get; set; }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}
