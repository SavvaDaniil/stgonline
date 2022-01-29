using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace STG.DTO.Payment
{
    public class PaymentIdForProlongationDTO
    {
        [Required(ErrorMessage = "no id")]
        public int id { get; set; }
    }
}
