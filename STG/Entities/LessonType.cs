using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace STG.Entities
{
    [Table("lesson_type")]
    public class LessonType
    {
        [Key, Column(Order = 0)]
        public int id { get; set; }

        [Column(TypeName = "varchar(256)")]
        public string name { get; set; }

        [Column(TypeName = "int(1)")]
        [System.ComponentModel.DefaultValue("1")]
        public int active { get; set; }

        [Column("order_in_list", TypeName = "int")]
        [System.ComponentModel.DefaultValue("0")]
        public int orderInList { get; set; }
    }
}
