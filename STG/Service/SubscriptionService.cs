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

        public Subscription findById(int id)
        {
            return this._dbc.Subscriptions
                .FirstOrDefault(p => p.id == id);
        }
        public Subscription findActiveById(int id)
        {
            return this._dbc.Subscriptions
                .Where(p => p.active == 1)
                .FirstOrDefault(p => p.id == id);
        }

        public List<Subscription> listAll()
        {
            return _dbc.Subscriptions
                .OrderByDescending(p => p.orderInList)
                .ToList();
        }

        public List<Subscription> listAllActive()
        {
            return _dbc.Subscriptions
                .Where(p => p.active == 1)
                .OrderByDescending(p => p.orderInList)
                .ToList();
        }


    }
}
