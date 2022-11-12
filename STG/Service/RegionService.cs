using Microsoft.EntityFrameworkCore;
using STG.Data;
using STG.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace STG.Service
{
    public class RegionService
    {
        private ApplicationDbContext _dbc;

        public RegionService(ApplicationDbContext dbc)
        {
            this._dbc = dbc;
        }

        public Region findById(int id)
        {
            return this._dbc.Regions.SingleOrDefault(p => p.id == id);
        }

        public Region find(User user)
        {
            if (user.region == null) return null;
            return _dbc.Regions.FirstOrDefault(p => p.id == user.region.id);
        }

        public List<Region> listAll()
        {
            return this._dbc.Regions
                .OrderByDescending(p => p.orderInList)
                .ThenBy(p => p.name)
                .ToList();
        }
    }
}
