using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace STG.DTO.Package
{
    public class PackageLessonDTO
    {
        [Required(ErrorMessage = "no id")]
        public int id { get; set; }
        [Required(ErrorMessage = "no id_of_package")]
        public int id_of_package { get; set; }
        [Required(ErrorMessage = "no id_of_package_day")]
        public int id_of_package_day { get; set; }

        public int id_of_lesson { get; set; }
        public int homework_status { get; set; }
        public string homework_text { get; set; }
    }
}
