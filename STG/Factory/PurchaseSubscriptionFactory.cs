using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using STG.Entities;
using STG.Interface.Factory;
using STG.Data;
using STG.DTO.PurchaseSubscription;
using STG.Service;

namespace STG.Factory
{
    public class PurchaseSubscriptionFactory
    {
        private ApplicationDbContext _dbc;
        public PurchaseSubscriptionFactory(ApplicationDbContext dbc)
        {
            this._dbc = dbc;
        }

        public async Task<PurchaseSubscription> create(Payment payment)
        {
            PurchaseSubscriptionService purchaseSubscriptionService = new PurchaseSubscriptionService(_dbc);
            return await purchaseSubscriptionService.add(payment);
        }
    }
}
