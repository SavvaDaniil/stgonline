using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace STG.Entities
{
    [Table("pre_user_with_appointment")]
    public class PreUserWithAppointment
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key, Column(Order = 0)]
        public int id { get; set; }

        [Column("username", TypeName = "varchar(256)")]
        public string username { get; set; }

        [Column(TypeName = "varchar(256)")]
        public string firstname { get; set; }

        [Column("secondname", TypeName = "varchar(256)")]
        public string secondname { get; set; }

        [Column(TypeName = "varchar(256)")]
        public string instagram { get; set; }

        [Column("date_of_birthday")]
        public DateTime? date_of_birthday { get; set; }

        [Column(TypeName = "varchar(256)")]
        [DataType(DataType.Password)]
        public string password { get; set; }

        [Column(TypeName = "int(1)")]
        public int is_need_curator { get; set; }

        [Column("id_of_region", TypeName = "int(11)")]
        public Region region { get; set; }

        [Column(TypeName = "int(1)")]
        public int experience { get; set; }

        [Column(TypeName = "int(1)")]
        public int expectations { get; set; }

        [Column(TypeName = "int(1)")]
        public int expected_time_for_lessons { get; set; }

        [Column(TypeName = "text")]
        public string idols { get; set; }

        [Column(TypeName = "varchar(256)")]
        public string link1 { get; set; }

        [Column(TypeName = "varchar(256)")]
        public string link2 { get; set; }

        [Column(TypeName = "varchar(256)")]
        public string link3 { get; set; }

        [Column(TypeName = "int(1)")]
        [DefaultValue(0)]
        public int status { get; set; }

        [Column("teachers", TypeName = "varchar(256)")]
        public string listOfTeachers { get; set; }

        public DateTime? dateOfAdd { get; set; }
        public DateTime? dateOfRegistration { get; set; }
    }
}
