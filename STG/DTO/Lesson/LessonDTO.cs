using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace STG.DTO.Lesson
{
    public class LessonDTO
    {
        public int id { get; set; }
        public string name { get; set; }
        public string shortName { get; set; }
        public string musicName { get; set; }
        public string description { get; set; }
        public int price { get; set; }
        public int days { get; set; }
        public int active { get; set; }
        public int isVisible { get; set; }
        public int isFree { get; set; }

        public IFormFile teaserFile { get; set; }
        public int isDeleteTeaser { get; set; }

        public string posterSrc { get; set; }
        public IFormFile posterFile { get; set; }
        public int isDeletePoster { get; set; }
        
        public int idOfVideo { get; set; }
        public int idOfStyle { get; set; }
        public int idOfLevel { get; set; }
        public int idOfLessonType { get; set; }
        public int idOfTeacher { get; set; }

        public string levels { get; set; }
    }
}
