using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace STG.DTO.Admin
{
    public class AdminEditDTO
    {
        [Required(ErrorMessage = "no id")]
        public int id { get; set; }
        public int active { get; set; }
        public string username { get; set; }
        public string position { get; set; }

        public string new_password { get; set; }
        

        public int panel_users { get; set; }
        public int panel_statements { get; set; }
        public int panel_mentoring { get; set; }
        public int panel_homeworks { get; set; }
        public int panel_lessons { get; set; }
        public int panel_packages { get; set; }
        public int panel_videos { get; set; }
        public int panel_teachers { get; set; }
        public int panel_styles { get; set; }
        public int panel_lessontypes { get; set; }
        public int panel_admins { get; set; }

    }
}
