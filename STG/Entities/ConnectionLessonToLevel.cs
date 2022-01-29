using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace STG.Entities
{
    [Table("connection_lesson_to_level")]
    public class ConnectionLessonToLevel
    {
        [Key, Column(Order = 0)]
        public int id { get; set; }

        [Column("id_of_level")]
        public Level level { get; set; }

        [Column("id_of_lesson")]
        public Lesson lesson { get; set; }

        [Column("date_of_add")]
        public DateTime? dateOfAdd { get; set; }
    }
}
