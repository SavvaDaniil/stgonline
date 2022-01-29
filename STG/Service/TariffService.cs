using Microsoft.EntityFrameworkCore;
using STG.Data;
using STG.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace STG.Service
{
    public class TariffService
    {
        private ApplicationDbContext _dbc;

        public TariffService(ApplicationDbContext dbc)
        {
            this._dbc = dbc;
        }

        public async Task<Tariff> findById(int id)
        {
            return await _dbc.Tariffs
                .OrderByDescending(p => p.orderInList)
                .FirstOrDefaultAsync(p => p.id == id);
        }

        public async Task<List<Tariff>> listAll()
        {
            return await _dbc.Tariffs
                .OrderByDescending(p => p.orderInList)
                .ToListAsync();
        }

        public async Task<IEnumerable<Tariff>> enumAll()
        {
            return await _dbc.Tariffs
                .OrderByDescending(p => p.orderInList)
                .ToListAsync();
        }
    }
}
