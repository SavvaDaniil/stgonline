using STG.ViewModels.PurchaseSubscription;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace STG.ViewModels.Json
{
    public class JsonAdminAnswer
    {
        public string status { get; }
        public string errors { get; }

        public PurchaseSubscriptionViewModel purchaseSubscription { get; }
        public List<STG.Entities.Subscription> subscriptions { get; }

        public List<PurchaseSubscriptionViewModel> purchaseSubscriptions { get; }

        public JsonAdminAnswer(string status, string errors)
        {
            this.status = status;
            this.errors = errors;
        }

        public JsonAdminAnswer(string status, string errors, List<PurchaseSubscriptionViewModel> purchaseSubscriptions) : this(status, errors)
        {
            this.purchaseSubscriptions = purchaseSubscriptions;
        }

        public JsonAdminAnswer(string status, string errors, List<Entities.Subscription> subscriptions) : this(status, errors)
        {
            this.subscriptions = subscriptions;
        }

        public JsonAdminAnswer(string status, string errors, PurchaseSubscriptionViewModel purchaseSubscription, List<Entities.Subscription> subscriptions) : this(status, errors)
        {
            this.purchaseSubscription = purchaseSubscription;
            this.subscriptions = subscriptions;
        }
    }
}
