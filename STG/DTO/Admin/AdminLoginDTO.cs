using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace STG.DTO.Admin
{
    public class AdminLoginDTO
    {
        [Required(ErrorMessage = "Не указан username")]
        public string username { get; set; }

        [Required(ErrorMessage ="Не указан пароль")]
        [DataType(DataType.Password)]
        public string password { get; set; }
    }
}
