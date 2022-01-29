using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using STG.Entities;

namespace STG.Interface.Factory
{
    interface IPaymentFactory
    {
        Payment create(User user, PurchaseLesson purchaseLesson, PurchaseSubscription purchaseSubscription);
    }
}
