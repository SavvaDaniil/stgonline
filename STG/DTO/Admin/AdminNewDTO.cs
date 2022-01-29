using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace STG.DTO.Admin
{
    public class AdminNewDTO
    {
        [Required(ErrorMessage = "Не указан Email")]
        public string username { get; set; }

        /*
        [Required(ErrorMessage = "Не указан пароль")]
        [DataType(DataType.Password)]
        public string password { get; set; }
        */
    }
}
