using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace STG.DTO.UserDTO
{
    public class UserNewDTO
    {

        [Required(ErrorMessage = "Не указан Email")]
        public string username { get; set; }

        [Required(ErrorMessage = "Не указано Имя")]
        public string firstname { get; set; }

        [Required(ErrorMessage = "Не указана Фамилия")]
        public string secondname { get; set; }

        public string instagram { get; set; }

        [Required(ErrorMessage = "Не указан пароль")]
        [DataType(DataType.Password)]
        public string password { get; set; }

        [Required(ErrorMessage = "Не указан пароль повторно")]
        [DataType(DataType.Password)]
        public string password_again { get; set; }

        public int is_need_curator { get; set; }

        [Required(ErrorMessage = "Не указан Регион")]
        public int id_of_region { get; set; }


        public DateTime? date_of_birthday { get; set; }


        public int experience { get; set; }
        public int expectations { get; set; }
        public int expected_time_for_lessons { get; set; }
        public string idols { get; set; }
        public string link1 { get; set; }
        public string link2 { get; set; }
        public string link3 { get; set; }

        public string curators { get; set; }
        public List<int> list_id_of_curators { get; set; }

    }
}
