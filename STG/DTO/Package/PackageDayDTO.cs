using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace STG.DTO.Package
{
    public class PackageDayDTO
    {
        [Required(ErrorMessage = "no id")]
        public int id { get; set; }

        [Required(ErrorMessage = "no id_of_package")]
        public int id_of_package { get; set; }

        public string name { get; set; }
    }
}
