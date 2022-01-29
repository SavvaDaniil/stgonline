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
    [Table("package_chat")]
    public class PackageChat
    {
        [Key, Column(Order = 0)]
        public int id { get; set; }

        [Column("id_of_user")]
        public User user { get; set; }

        //[Column("id_of_connection_user_to_private_package")]
        //public ConnectionUserToPrivatePackage connectionUserToPrivatePackage { get; set; }

        [Column("id_of_package")]
        public Package package { get; set; }

        [Column("id_of_admin")]
        public Admin admin { get; set; }

        [Column("id_of_teacher")]
        public Teacher teacher { get; set; }

        [Column("user0_teacher1", TypeName = "int(1)")]
        [DefaultValue("0")]
        public int user0_teacher1 { get; set; }

        [Column("message", TypeName = "Text")]
        public string message { get; set; }

        [Column("is_read", TypeName = "int(1)")]
        [DefaultValue("0")]
        public int is_read { get; set; }

        [Column("date_of_add")]
        public DateTime? date_of_add { get; set; }

        [Column("date_of_read")]
        public DateTime? date_of_read { get; set; }
    }
}
