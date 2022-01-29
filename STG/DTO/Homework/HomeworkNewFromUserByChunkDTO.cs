using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace STG.DTO.Homework
{
    public class HomeworkNewFromUserByChunkDTO
    {
        [Required(ErrorMessage = "no id_of_package_lesson")]
        public int id_of_package_lesson { get; set; }

        public IFormFile videofile { get; set; }
        public string comment { get; set; }
        public int index_of_chunk { get; set; }
        public int is_last_chunk { get; set; }
    }
}
