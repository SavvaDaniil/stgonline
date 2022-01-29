using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using STG.Entities;

namespace STG.Interface.Facade
{
    interface IPurchaseSubscriptionFacade
    {
        PurchaseSubscription prepareForBuy(User user, Subscription subscription, Payment payment);
        PurchaseSubscription setBuyed(User user, Subscription subscription);
    }
}
