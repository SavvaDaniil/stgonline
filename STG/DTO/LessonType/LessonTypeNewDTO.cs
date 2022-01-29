using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace STG.DTO.LessonType
{
    public class LessonTypeNewDTO
    {
        [Required(ErrorMessage = "Не указано наименование")]
        public string name { get; set; }
    }
}
