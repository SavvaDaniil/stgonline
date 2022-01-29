using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace STG.DTO
{
    public class VideoDTO
    {
        public int id { get; set; }
        public string name { get; set; }
        public string hashPath { get; set; }

        public int durationHours { get; set; }
        public int durationMinutes { get; set; }
        public int durationSeconds { get; set; }

        public int isDeletePoster { get; set; }

        public IFormFile posterFile { get; set; }

        public int isDeleteVideoContentApi { get; set; }
        
    }
}
