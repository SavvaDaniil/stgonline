using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace STG.DTO.Package
{
    public class PackageDayIdDTO
    {
        [Required(ErrorMessage = "no id")]
        public int id { get; set; }
    }
}
