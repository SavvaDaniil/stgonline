using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace STG.Entities
{
    [Table("observer_lesson_user")]
    public class ObserverLessonUser
    {
        [Key, Column("id", Order = 0)]
        public int id { get; set; }

        [Column("id_of_lesson")]
        public Lesson lesson { get; set; }

        [Column("id_of_user")]
        public User user { get; set; }

        [Column("current_time", TypeName = "int(11)")]
        [DefaultValue("0")]
        public int currentTime { get; set; }

        [Column("max_viewed_time", TypeName = "int(11)")]
        [DefaultValue("0")]
        public int maxViewedTime { get; set; }

        [Column("length", TypeName = "int(11)")]
        [DefaultValue("0")]
        public int length { get; set; }

        [Column("date_of_add")]
        public DateTime? date_of_add { get; set; }
        [Column("date_of_update")]
        public DateTime? date_of_update { get; set; }
    }
}
