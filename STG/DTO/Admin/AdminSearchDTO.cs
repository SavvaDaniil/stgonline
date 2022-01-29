using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace STG.DTO.Admin
{
    public class AdminSearchDTO
    {
        public int page { get; set; }
        public string queryString { get; set; }
    }
}
