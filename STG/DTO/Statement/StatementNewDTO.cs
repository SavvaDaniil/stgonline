using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace STG.DTO.Statement
{
    public class StatementNewDTO
    {
        [Required(ErrorMessage = "no is_need_curator")]

        public int is_need_curator { get; set; }

        public int experience { get; set; }

        public int expectations { get; set; }

        public int expected_time_for_lessons { get; set; }

        public string idols { get; set; }

        public string link1 { get; set; }

        public string link2 { get; set; }

        public string link3 { get; set; }

        public string curators { get; set; }
    }
}
