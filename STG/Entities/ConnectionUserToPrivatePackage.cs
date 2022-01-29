using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using STG.Entities;

namespace STG.Entities
{
    [Table("connection_user_to_private_package")]
    public class ConnectionUserToPrivatePackage
    {
        [Key, Column(Order = 0)]
        public int id { get; set; }

        [Column("id_of_user")]
        public User user { get; set; }

        [Column("id_of_package")]
        public Package package { get; set; }

        [Column("date_of_add")]
        public DateTime? date_of_add { get; set; }
    }
}
