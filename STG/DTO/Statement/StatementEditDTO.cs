using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace STG.DTO.Statement
{
    public class StatementEditDTO
    {
        [Required(ErrorMessage = "no id")]
        public int id { get; set; }

        public int id_of_curator { get; set; }
    }
}
