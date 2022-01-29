using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace STG.DTO.Package
{
    public class PackageLessonNewDTO
    {
        [Required(ErrorMessage = "no id_of_package")]
        public int id_of_package { get; set; }
        [Required(ErrorMessage = "no id_of_package_day")]
        public int id_of_packageday { get; set; }
    }
}
