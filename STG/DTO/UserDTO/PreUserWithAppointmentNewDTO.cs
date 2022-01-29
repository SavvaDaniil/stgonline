using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace STG.DTO.UserDTO
{
    public class PreUserWithAppointmentNewDTO
    {
        [Required(ErrorMessage = "Не указан Email")]
        public string username { get; set; }

        [Required(ErrorMessage = "Не указано Имя")]
        public string firstname { get; set; }

        [Required(ErrorMessage = "Не указан Instagram")]
        public string instagram { get; set; }

        [Required(ErrorMessage = "Не указан пароль")]
        [DataType(DataType.Password)]
        public string password { get; set; }

        [Required(ErrorMessage = "Не указан пароль повторно")]
        [DataType(DataType.Password)]
        public string password_again { get; set; }

        [Required(ErrorMessage = "Не указано, нужен ли куратор")]
        public int is_need_curator { get; set; }

        public int id_of_region { get; set; }

        public int experience { get; set; }
        public int expectations { get; set; }
        public int expected_time_for_lessons { get; set; }
        public string idols { get; set; }
        public string link1 { get; set; }
        public string link2 { get; set; }
        public string link3 { get; set; }
    }
}
