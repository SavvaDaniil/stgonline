using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace STG.DTO.Homework
{
    public class HomeworkAnswerFromAdminDTO
    {
        [Required(ErrorMessage = "no id_of_homework")]
        public int id_of_homework { get; set; }

        public string answer { get; set; }
    }
}
