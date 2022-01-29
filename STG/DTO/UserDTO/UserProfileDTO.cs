using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace STG.DTO.UserDTO
{
    public class UserProfileDTO
    {
        [Required(ErrorMessage = "Не указан Username")]
        public string username { get; set; }

        public string firstname { get; set; }

        public string secondname { get; set; }

        public string instagram { get; set; }

        public DateTime? date_of_birthday { get; set; }

        public int prolongation { get; set; }

        [DataType(DataType.Password)]
        public string newPassword { get; set; }

        [DataType(DataType.Password)]
        public string newPasswordAgain { get; set; }

        [DataType(DataType.Password)]
        public string currentPassword { get; set; }
    }
}
