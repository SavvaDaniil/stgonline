using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace STG.ViewModels.PurchaseSubscription
{
    public class PurchaseSubscriptionViewModel
    {
        public int id { get; set; }
        public int id_of_subscription { get; set; }
        public string name { get; set; }
        public int active { get; set; }
        public int days { get; set; }
        public int isPayed { get; set; }
        public string dateOfAdd { get; set; }
        public string dateOfActivation { get; set; }
        public string dateOfMustBeUsedTo { get; set; }

        public PurchaseSubscriptionViewModel(int id, int id_of_subscription, string name, int active, int days, int isPayed, string dateOfAdd, string dateOfActivation, string dateOfMustBeUsedTo)
        {
            this.id = id;
            this.id_of_subscription = id_of_subscription;
            this.name = name;
            this.active = active;
            this.days = days;
            this.isPayed = isPayed;
            this.dateOfAdd = dateOfAdd;
            this.dateOfActivation = dateOfActivation;
            this.dateOfMustBeUsedTo = dateOfMustBeUsedTo;
        }
    }
}
