using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace STG.DTO.ObserverLessonUser
{
    public class ObserverLessonUserDTO
    {
        [Required(ErrorMessage = "no id_of_lesson")]
        public int id_of_lesson { get; set; }

        [Required(ErrorMessage = "no currentTime")]
        public int currentTime { get; set; }

        [Required(ErrorMessage = "no length")]
        public int length { get; set; }
    }
}
