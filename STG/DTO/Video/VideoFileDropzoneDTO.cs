using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace STG.DTO.Video
{
    public class VideoFileDropzoneDTO
    {
        [Required(ErrorMessage = "no id")]
        public int id { get; set; }
        public string hashPath { get; set; }

        public int isDeleteContent { get; set; }
        [Required(ErrorMessage = "no videoFile")]
        public IFormFile videoFile { get; set; }

        public int dzchunkbyteoffset { get; set; }
        public int dztotalfilesize { get; set; }
        public int dztotalchunkcount { get; set; }
        public int dzchunkindex { get; set; }
    }
}
