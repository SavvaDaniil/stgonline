using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace STG.DTO.Package
{
    public class PackageDayNewDTO
    {
        [Required(ErrorMessage = "no id_of_package")]
        public int id_of_package { get; set; }

        public string name { get; set; }
    }
}
