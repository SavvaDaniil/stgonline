using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using STG.Entities;

namespace STG.Interface.Factory
{
    interface IPurchaseSubscriptionFactory
    {
        PurchaseLesson createIfNotExistOrUpdate(User user, Subscription subscription, Payment payment);
    }
}
