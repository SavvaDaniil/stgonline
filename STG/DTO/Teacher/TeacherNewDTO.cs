using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace STG.DTO.Teacher
{
    public class TeacherNewDTO
    {
        [Required(ErrorMessage = "Не указано ФИО")]
        public string fio { get; set; }
        
        public string instagram { get; set; }
    }
}
