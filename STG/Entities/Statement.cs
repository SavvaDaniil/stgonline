using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace STG.Entities
{
    [Table("statement")]
    public class Statement
    {
        [Key, Column(Order = 0)]
        public int id { get; set; }

        [Column("id_of_user")]
        public User user { get; set; }

        [Column("is_need_curator", TypeName = "int(1)")]
        [System.ComponentModel.DefaultValue("0")]
        public int is_need_curator { get; set; }

        [Column("experience", TypeName = "int(1)")]
        [System.ComponentModel.DefaultValue("0")]
        public int experience { get; set; }

        [Column("expectations", TypeName = "int(1)")]
        [System.ComponentModel.DefaultValue("0")]
        public int expectations { get; set; }

        [Column("expected_time_for_lessons", TypeName = "int(1)")]
        [System.ComponentModel.DefaultValue("0")]
        public int expected_time_for_lessons { get; set; }

        [Column("idols", TypeName = "Text")]
        public string idols { get; set; }

        [Column("teachers", TypeName = "varchar(256)")]
        public string listOfTeachers { get; set; }

        [Column("link1", TypeName = "varchar(512)")]
        public string link1 { get; set; }

        [Column("link2", TypeName = "varchar(512)")]
        public string link2 { get; set; }

        [Column("link3", TypeName = "varchar(512)")]
        public string link3 { get; set; }

        [Column("is_payed", TypeName = "int(1)")]
        [System.ComponentModel.DefaultValue("0")]
        public int is_payed { get; set; }

        [Column("status", TypeName = "int(1)")]
        [System.ComponentModel.DefaultValue("0")]
        public int status { get; set; }

        [Column("id_of_teacher")]
        public Teacher teacher { get; set; }

        [Column("date_of_add")]
        public DateTime? date_of_add { get; set; }

        [Column("date_of_payed")]
        public DateTime? date_of_payed { get; set; }

        [Column("date_of_active")]
        public DateTime? date_of_active { get; set; }
    }
}
