using STG.Data;
using STG.Entities;
using STG.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace STG.Facade
{
    public class RegionFacade
    {
        private ApplicationDbContext _dbc;
        public RegionFacade(ApplicationDbContext dbc)
        {
            _dbc = dbc;
        }

        public async Task<Region> getRegionByUser(User user)
        {
            RegionService regionService = new RegionService(_dbc);
            return await regionService.find(user);
        }

    }
}
