using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace STG.Entities
{
    [Table("package")]
    public class Package
    {
        [Key, Column(Order = 0)]
        public int id { get; set; }

        [Column(TypeName = "varchar(256)")]
        public string name { get; set; }

        [Column(TypeName = "text")]
        public string description { get; set; }

        [Column(TypeName = "int(11)")]
        [DefaultValue("0")]
        public int price { get; set; }

        [Column(TypeName = "int(3)")]
        [DefaultValue("90")]
        public int days { get; set; }

        [Column(TypeName = "int(1)")]
        [DefaultValue("0")]
        public int active { get; set; }

        [Column("id_of_level")]
        public Level level { get; set; }
        [Column("id_of_style")]
        public Style style { get; set; }
        [Column("id_of_teacher")]
        public Teacher teacher { get; set; }

        [Column("id_of_tariff")]
        public Tariff tariff { get; set; }

        [Column("status_of_chat_none0_homework1_and_chat2", TypeName = "int(1)")]
        [DefaultValue("0")]
        public int statusOfChatNone0Homework1AndChat2 { get; set; }


        [Column("order_in_list", TypeName = "int")]
        [DefaultValue("0")]
        public int orderInList { get; set; }
    }
}
