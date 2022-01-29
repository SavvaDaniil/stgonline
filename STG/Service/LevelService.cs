using Microsoft.EntityFrameworkCore;
using STG.Data;
using STG.Interface.Service;
using STG.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace STG.Service
{
    public class LevelService : ILevelService
    {
        private ApplicationDbContext _dbc;

        public LevelService(ApplicationDbContext dbc)
        {
            this._dbc = dbc;
        }

        public async Task<Level> findById(int id)
        {
            return await this._dbc.Levels.SingleOrDefaultAsync(p => p.Id == id);
        }

        public async Task<List<Level>> listAll()
        {
            return await this._dbc.Levels.OrderBy(p => p.orderInList).ToListAsync();
        }
    }
}
