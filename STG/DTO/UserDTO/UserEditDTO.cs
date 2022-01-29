using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace STG.DTO.UserDTO
{
    public class UserEditDTO
    {
        [Required(ErrorMessage = "no id")]
        public int id { get; set; }
        public int active { get; set; }
        public string username { get; set; }
        public string firstname { get; set; }
        public string secondname { get; set; }
        public string instagram { get; set; }
        public DateTime? date_of_birthday { get; set; }
        public int is_test { get; set; }
        public int is_lesson_full_access { get; set; }

        public string new_password { get; set; }

        public int prolongation { get; set; }
    }
}
