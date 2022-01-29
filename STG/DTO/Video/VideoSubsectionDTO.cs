using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace STG.DTO.Video
{
    public class VideoSubsectionDTO
    {
        [Required(ErrorMessage = "no id")]
        public int id { get; set; }

        public string field { get; set; }
        public string value { get; set; }
    }
}
