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

        public Tariff findById(int id)
        {
            return _dbc.Tariffs
                .OrderByDescending(p => p.orderInList)
                .FirstOrDefault(p => p.id == id);
        }

        public List<Tariff> listAll()
        {
            return _dbc.Tariffs
                .OrderByDescending(p => p.orderInList)
                .ToList();
        }

        public IEnumerable<Tariff> enumAll()
        {
            return _dbc.Tariffs
                .OrderByDescending(p => p.orderInList)
                .ToList();
        }
    }
}
