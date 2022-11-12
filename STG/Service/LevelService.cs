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

        public Level findById(int id)
        {
            return this._dbc.Levels.SingleOrDefault(p => p.Id == id);
        }

        public List<Level> listAll()
        {
            return this._dbc.Levels.OrderBy(p => p.orderInList).ToList();
        }
    }
}
