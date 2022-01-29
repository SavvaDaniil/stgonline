using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace STG.DTO.Package
{
    public class PackageDTO
    {
        [Required(ErrorMessage = "no id")]
        public int id { get; set; }
        public string name { get; set; }
        public int active { get; set; }
        public int price { get; set; }
        public int days { get; set; }
        public string description { get; set; }


        public int id_of_level { get; set; }
        public int id_of_style { get; set; }
        public int id_of_teacher { get; set; }
        public int id_of_tariff { get; set; }

        public int statusOfChatNone0Homework1AndChat2 { get; set; }

        public IFormFile posterFile { get; set; }
        public int isPosterDelete { get; set; }


    }
}
