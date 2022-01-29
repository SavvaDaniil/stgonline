using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace STG.DTO.Statement
{
    public class StatementSearchDTO
    {
        public string queryString { get; set; }
        public int active { get; set; }
        public int page { get; set; }
    }
}
