using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace STG.DTO.UserDTO
{
    public class UserForgetDTO
    {
        [Required(ErrorMessage = "no step")]
        public int step { get; set; }
        public string username { get; set; }
        public string code { get; set; }
        public int forget_id { get; set; }
    }
}
