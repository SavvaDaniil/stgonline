using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace STG.DTO.Homework
{
    public class HomeworkIdDTO
    {
        [Required(ErrorMessage = "no id")]
        public int id { get; set; }
    }
}
