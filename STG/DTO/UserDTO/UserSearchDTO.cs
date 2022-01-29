using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace STG.DTO.UserDTO
{
    public class UserSearchDTO
    {
        public int page { get; set; }
        public string queryString { get; set; }
    }
}
