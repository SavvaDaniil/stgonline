using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace STG.Entities
{

    [Table("video_subsection")]
    public class VideoSubsection
    {
        [Key, Column(Order = 0)]
        public int id { get; set; }

        [Column("id_of_video")]
        public Video video { get; set; }

        [Column("id_of_section")]
        public VideoSection videoSection { get; set; }

        [Column(TypeName = "varchar(256)")]
        public string name { get; set; }

        [Column("timing_minutes", TypeName = "int(3)")]
        [DefaultValue("0")]
        public int timingMinutes { get; set; }

        [Column("timing_seconds", TypeName = "int(2)")]
        [DefaultValue("0")]
        public int timingSeconds { get; set; }

        [Column("order_in_list", TypeName = "int")]
        [System.ComponentModel.DefaultValue("0")]
        public int orderInList { get; set; }

    }
}
