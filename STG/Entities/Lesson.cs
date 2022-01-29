using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace STG.Entities
{
    [Table("lesson")]
    public class Lesson
    {
        [Key, Column(Order = 0)]
        public int id { get; set; }

        [Column(TypeName = "varchar(512)")]
        public string name { get; set; }

        [Column("short_name", TypeName = "varchar(516)")]
        public string shortName { get; set; }

        [Column("music_name", TypeName = "varchar(516)")]
        public string musicName { get; set; }

        [Column(TypeName = "text")]
        public string description { get; set; }

        [Column("lengthInSeconds", TypeName = "varchar(16)")]
        public string lengthInSeconds { get; set; }

        [Column(TypeName = "int")]
        [System.ComponentModel.DefaultValue("0")]
        public int price { get; set; }

        [Column(TypeName = "int(1)")]
        [System.ComponentModel.DefaultValue("0")]
        public int active { get; set; }

        [Column("is_visible", TypeName = "int(1)")]
        [System.ComponentModel.DefaultValue("0")]
        public int isVisible { get; set; }

        [Column("is_free", TypeName = "int(1)")]
        [System.ComponentModel.DefaultValue("0")]
        public int isFree { get; set; }

        [Column("days", TypeName = "int(3)")]
        [System.ComponentModel.DefaultValue("0")]
        public int days { get; set; }

        [Column("id_of_level")]
        public Level level { get; set; }
        [Column("id_of_lessonType")]
        public LessonType lessonType { get; set; }
        [Column("id_of_teacher")]
        public Teacher teacher { get; set; }
        [Column("id_of_style")]
        public Style style { get; set; }
        [Column("id_of_video")]
        public Video video { get; set; }

        [Column("order_in_list", TypeName = "int")]
        [System.ComponentModel.DefaultValue("0")]
        public int orderInList { get; set; }

        [Column("date_of_add")]
        public DateTime? dateOfAdd { get; set; }

    }
}
