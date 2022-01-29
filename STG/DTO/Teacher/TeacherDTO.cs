using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace STG.DTO.Teacher
{
    public class TeacherDTO
    {
        [Required(ErrorMessage = "не указан Id")]
        public int id { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public string instagram { get; set; }
        public string shortDescription { get; set; }
        public string description { get; set; }
        public int active { get; set; }
        public string mentorBio { get; set; }
        public string mentorAwards { get; set; }

        public int is_curator { get; set; }
        public int price_curator { get; set; }
        public int price_tariff_1 { get; set; }
        public int price_tariff_2 { get; set; }
        public int price_tariff_3 { get; set; }

        public IFormFile avatarFile { get; set; }
        public int isPosterDelete { get; set; }
    }
}
