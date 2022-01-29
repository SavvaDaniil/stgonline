using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace STG.DTO.UserDTO
{
    public class UserIdDTO
    {
        [Required(ErrorMessage = "no id")]
        public int id { get; set; }
    }
}
