using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace STG.ViewModels.Subscription
{
    public class SubscriptionLiteViewModel
    {
        public int id { get; set; }
        public string name { get; set; }
        public int price { get; set; }
        public int days { get; set; }
        public int prolongation { get; set; }

        public SubscriptionLiteViewModel(int id, string name, int price, int days, int prolongation)
        {
            this.id = id;
            this.name = name;
            this.price = price;
            this.days = days;
            this.prolongation = prolongation;
        }
    }
}
