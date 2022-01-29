using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace STG.DTO.Admin
{
    public class AdminDTO
    {
        [Required(ErrorMessage = "Не указан Username")]
        public string username { get; set; }

        public string position { get; set; }

        [DataType(DataType.Password)]
        public string passwordNew { get; set; }

        [DataType(DataType.Password)]
        public string passwordNewAgain { get; set; }

        [DataType(DataType.Password)]
        public string passwordCurrent { get; set; }
    }
}
