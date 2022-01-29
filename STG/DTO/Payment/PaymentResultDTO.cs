using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace STG.DTO.Payment
{
    public class PaymentResultDTO
    {
        [Required(ErrorMessage = "no id")]
        public int id_of_payment { get; set; }
    }
}
