using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using STG.Entities;

namespace STG.Entities
{
    [Table("extend")]
    public class Extend
    {
        [Key, Column(Order = 0)]
        public int id { get; set; }

        [Column("id_of_user")]
        public User user { get; set; }

        [Column("id_of_purchase_subscription", TypeName = "int")]
        [DefaultValue("0")]
        public int id_of_purchase_subscription { get; set; }
        
        [Column("status", TypeName = "int(1)")]
        [DefaultValue("0")]
        public int status { get; set; }

        [Column("success", TypeName = "int(1)")]
        [DefaultValue("0")]
        public int success { get; set; }

        [Column("date_of_add")]
        public DateTime? date_of_add { get; set; }

        [Column("date_of_thread_start")]
        public DateTime? date_of_thread_start { get; set; }

        [Column("date_of_thread_must_be_finished")]
        public DateTime? date_of_thread_must_be_finished { get; set; }

        [Column("date_of_thread_finished")]
        public DateTime? date_of_thread_finished { get; set; }
    }
}
