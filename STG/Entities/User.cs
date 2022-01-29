using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace STG.Entities
{
    [Table("user")]
    public class User
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key, Column(Order = 0)]
        public int Id { get; set; }

        [Column(TypeName = "varchar(256)")]
        public string Username { get; set; }

        [Column(TypeName = "varchar(256)")]
        public string Password { get; set; }

        [Column("auth_key", TypeName = "varchar(256)")]
        public string AuthKey { get; set; }

        [Column("access_token", TypeName = "varchar(256)")]
        public string AccessToken { get; set; }

        [Column(TypeName = "int(1)")]
        [System.ComponentModel.DefaultValue("0")]
        public int active { get; set; }

        [Column("firstname", TypeName = "varchar(256)")]
        public string firstname { get; set; }

        [Column("secondname", TypeName = "varchar(256)")]
        public string secondname { get; set; }

        [Column("date_of_birthday")]
        public DateTime? date_of_birthday { get; set; }

        [Column(TypeName = "int(1)")]
        [System.ComponentModel.DefaultValue("0")]
        public int sex { get; set; }

        [Column("is_test", TypeName = "int(1)")]
        [System.ComponentModel.DefaultValue("0")]
        public int isTest { get; set; }

        [Column("is_test_prolongation", TypeName = "int(1)")]
        [System.ComponentModel.DefaultValue("0")]
        public int isTestProlongation { get; set; }

        [Column("id_of_region")]
        public Region region { get; set; }

        [Column(TypeName = "varchar(256)")]
        public string instagram { get; set; }

        [Column("prolongation", TypeName = "int(1)")]
        [System.ComponentModel.DefaultValue("0")]
        public int prolongation { get; set; }

        [Column("forget_count", TypeName = "int(1)")]
        [System.ComponentModel.DefaultValue("0")]
        public int forget_count { get; set; }

        [Column("forget_code", TypeName = "varchar(6)")]
        public string forget_code { get; set; }

        [Column("date_of_last_try")]
        public DateTime? forgetDateOfLastTry { get; set; }

        [Column("date_of_add")]
        public DateTime? dateOfAdd { get; set; }

        [Column("date_of_last_update_profile")]
        public DateTime? dateOfLastUpdateProfile { get; set; }

        [Column("id_in_amocrm", TypeName = "int(1)")]
        [System.ComponentModel.DefaultValue("0")]
        public int id_in_amocrm { get; set; }

        [Column("amocrm_cansel", TypeName = "int(1)")]
        [System.ComponentModel.DefaultValue("0")]
        public int amocrm_cansel { get; set; }

        [Column("is_lesson_full_access", TypeName = "int(1)")]
        [System.ComponentModel.DefaultValue("0")]
        public int isLessonFullAccess { get; set; }
    }
}
