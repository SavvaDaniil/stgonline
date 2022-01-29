using STG.Data;
using STG.Entities;
using STG.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace STG.Factory
{
    public class PurchasePackageFactory
    {
        private ApplicationDbContext _dbc;
        public PurchasePackageFactory(ApplicationDbContext dbc)
        {
            this._dbc = dbc;
        }

        public async Task<PurchasePackage> create(Payment payment)
        {
            PurchasePackageService purchasePackageService = new PurchasePackageService(_dbc);
            return await purchasePackageService.add(payment);
        }
    }
}
