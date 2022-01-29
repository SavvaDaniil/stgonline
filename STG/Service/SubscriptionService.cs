using Microsoft.EntityFrameworkCore;
using STG.Data;
using STG.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace STG.Service
{
    public class SubscriptionService
    {
        private ApplicationDbContext _dbc;
        public SubscriptionService(ApplicationDbContext dbc)
        {
            this._dbc = dbc;
        }

        public async Task<Subscription> findById(int id)
        {
            return await this._dbc.Subscriptions
                .FirstOrDefaultAsync(p => p.id == id);
        }
        public async Task<Subscription> findActiveById(int id)
        {
            return await this._dbc.Subscriptions
                .Where(p => p.active == 1)
                .FirstOrDefaultAsync(p => p.id == id);
        }

        public async Task<List<Subscription>> listAll()
        {
            return await _dbc.Subscriptions
                .OrderByDescending(p => p.orderInList)
                .ToListAsync();
        }

        public async Task<List<Subscription>> listAllActive()
        {
            return await _dbc.Subscriptions
                .Where(p => p.active == 1).ToListAsync();
        }


    }
}
