using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace STG.DTO.Admin
{
    public class AdminSearchIdDTO
    {
        [Required(ErrorMessage = ("no id"))]
        public int id { get; set; }
    }
}
