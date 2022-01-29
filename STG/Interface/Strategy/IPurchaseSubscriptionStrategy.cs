using STG.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace STG.Interface.Strategy
{
    interface IPurchaseSubscriptionStrategy
    {
        Subscription buySubscription(User user, Subscription subscription, Payment payment);
    }
}
