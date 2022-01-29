using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace STG.DTO.Extend
{
    public class ExtendOfPurchaseSubscriptionDTO
    {
        [Required(ErrorMessage = "no id_of_extend")]
        public int id_of_extend { get; set; }

        [Required(ErrorMessage = "no id_of_purchase_subscription")]
        public int id_of_purchase_subscription { get; set; }


    }
}
