﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace STG.DTO.Lesson
{
    public class LessonNewDTO
    {
        [Required(ErrorMessage = "Не указано название")]
        public string name { get; set; }
    }
}
