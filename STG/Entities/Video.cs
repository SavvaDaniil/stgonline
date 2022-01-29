using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace STG.Entities
{
    [Table("video")]
    public class Video
    {
        [Key, Column(Order = 0)]
        public int id { get; set; }

        [Column(TypeName = "varchar(512)")]
        public string name { get; set; }

        [Column(TypeName = "varchar(32)")]
        public string hashPath { get; set; }
        
        public DateTime? dateOfAdd { get; set; }

        [Column("duration", TypeName = "int(32)")]
        public int duration { get; set; }
    }
}
