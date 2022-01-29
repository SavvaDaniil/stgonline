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

        public async Task<Region> findById(int id)
        {
            return await this._dbc.Regions.SingleOrDefaultAsync(p => p.id == id);
        }

        public async Task<Region> find(User user)
        {
            if (user.region == null) return null;
            return await _dbc.Regions.FirstOrDefaultAsync(p => p.id == user.region.id);
        }

        public async Task<List<Region>> listAll()
        {
            return await this._dbc.Regions
                .OrderByDescending(p => p.orderInList)
                .ThenBy(p => p.name)
                .ToListAsync();
        }
    }
}
