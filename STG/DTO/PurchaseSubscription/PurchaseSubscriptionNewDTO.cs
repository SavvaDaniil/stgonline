using STG.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace STG.DTO.PurchaseSubscription
{
    public class PurchaseSubscriptionNewDTO
    {
        [Required(ErrorMessage = "no user")]
        public STG.Entities.User user { get; set; }

        [Required(ErrorMessage = "no lesson")]
        public STG.Entities.Subscription subscription { get; set; }

        [Required(ErrorMessage = "no payment")]
        public STG.Entities.Payment payment { get; set; }
    }
}
