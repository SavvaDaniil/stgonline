using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace STG.DTO.Video
{
    public class VideosubsectionNewDTO
    {
        [Required(ErrorMessage = "no id_of_video")]
        public int id_of_video { get; set; }

        [Required(ErrorMessage = "no id_of_section")]
        public int id_of_section { get; set; }

        public string name { get; set; }
    }
}
