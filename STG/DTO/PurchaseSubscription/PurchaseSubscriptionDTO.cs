using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace STG.DTO.PurchaseSubscription
{
    public class PurchaseSubscriptionDTO
    {
        [Required(ErrorMessage = "no id")]
        public int id { get; set; }

        [Required(ErrorMessage = "no id_of_subscription")]
        public int id_of_subscription { get; set; }

        public int active { get; set; }
        public int days { get; set; }

        public int date_buy_day { get; set; }
        public int date_buy_month { get; set; }
        public int date_buy_year { get; set; }

        public int date_active_day { get; set; }
        public int date_active_month { get; set; }
        public int date_active_year { get; set; }

        public int date_must_be_used_to_day { get; set; }
        public int date_must_be_used_to_month { get; set; }
        public int date_must_be_used_to_year { get; set; }
    }
}
