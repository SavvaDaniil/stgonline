using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace STG.DTO.Package
{
    public class PackageNewDTO
    {
        [Required(ErrorMessage = "no name")]
        public string name { get; set; }
    }
}
