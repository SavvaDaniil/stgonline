using STG.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace STG.Entities
{
    [Table("homework")]
    public class Homework
    {
        [Key, Column(Order = 0)]
        public int id { get; set; }

        [Column("id_of_user")]
        public User user { get; set; }

        [Column("id_of_package_lesson")]
        public PackageLesson packageLesson { get; set; }

        [Column("comment", TypeName = "Text")]
        public string comment { get; set; }

        [Column("hash", TypeName = "varchar(64)")]
        public string hash { get; set; }

        [Column("filename", TypeName = "varchar(64)")]
        public string filename { get; set; }

        [Column("status", TypeName = "int(1)")]
        [DefaultValue("0")]
        public int status { get; set; }

        [Column("status_of_seen", TypeName = "int(1)")]
        [DefaultValue("0")]
        public int statusOfSeen { get; set; }

        [Column("status_of_upload", TypeName = "int(1)")]
        [DefaultValue("0")]
        public int statusOfUpload { get; set; }

        [Column("date_of_add")]
        public DateTime? date_of_add { get; set; }
        [Column("date_of_update")]
        public DateTime? date_of_update { get; set; }
        [Column("date_of_seen_by_admin")]
        public DateTime? date_of_seen_by_admin { get; set; }



        [Column("answer_from_teacher", TypeName = "Text")]
        public string answer_from_teacher { get; set; }

        [Column("date_of_update_of_teacher")]
        public DateTime? date_of_update_of_teacher { get; set; }

        [Column("status_of_seen_of_message_from_teacher", TypeName = "int(1)")]
        [DefaultValue("0")]
        public int status_of_seen_of_message_from_teacher { get; set; }
    }
}
