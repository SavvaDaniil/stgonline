using STG.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace STG.DTO.Lesson
{
    public class LessonFilterDTO
    {
        public string name { get; set; }
        public STG.Entities.Style style { get; set; }
        public int id_of_level { get; set; }
        public int id_of_teacher { get; set; }
        public int skip { get; set; }
        public int take { get; set; }
        public int isFree { get; set; }
    }
}
