using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace STG.DTO
{
    public class VideoNewDTO
    {
        [Required(ErrorMessage = "Не указано наименование")]
        public string name { get; set; }
    }
}
