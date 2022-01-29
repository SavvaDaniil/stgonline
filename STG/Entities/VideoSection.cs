using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace STG.Entities
{
    [Table("video_section")]
    public class VideoSection
    {
        [Key, Column(Order = 0)]
        public int id { get; set; }

        [Column("id_of_video")]
        public Video video { get; set; }

        [Column(TypeName = "varchar(256)")]
        public string name { get; set; }

        [Column("order_in_list", TypeName = "int")]
        [System.ComponentModel.DefaultValue("0")]
        public int orderInList { get; set; }

    }
}
