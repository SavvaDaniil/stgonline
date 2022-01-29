using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace STG.DTO.Video
{
    public class VideoFileDTO
    {
        public int id { get; set; }
        public int offset { get; set; }
        public int dzchunkbyteoffset { get; set; }

        public int isDeleteContent { get; set; }
        public IFormFile videoFile { get; set; }
        
        public string blobFileContent { get; set; }

        public int isStart { get; set; }
    }
}
