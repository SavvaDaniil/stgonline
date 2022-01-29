using STG.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace STG.Interface
{
    interface IPurchaseSubscriptionService
    {
        PurchaseSubscription add();
        PurchaseSubscription save(PurchaseSubscription purchaseSubscription);
        bool delete();
        List<PurchaseSubscription> listAllActiveOfUser(User user);
        List<PurchaseSubscription> listAllOfUser(User user);
    }
}
