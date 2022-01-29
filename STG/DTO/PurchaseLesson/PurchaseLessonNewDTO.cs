using STG.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace STG.DTO.PurchaseLesson
{
    public class PurchaseLessonNewDTO
    {
        [Required(ErrorMessage = "no user")]
        public STG.Entities.User user { get; set; }

        [Required(ErrorMessage = "no lesson")]
        public STG.Entities.Lesson lesson { get; set; }

        [Required(ErrorMessage = "no payment")]
        public STG.Entities.Payment payment { get; set; }

    }
}
