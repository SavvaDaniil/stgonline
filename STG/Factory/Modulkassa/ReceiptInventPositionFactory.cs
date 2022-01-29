using STG.Data;
using STG.Entities;
using STG.Models.Modulkassa;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace STG.Factory.Modulkassa
{
    public class ReceiptInventPositionFactory
    {
        private ApplicationDbContext _dbc;
        public ReceiptInventPositionFactory(ApplicationDbContext dbc)
        {
            _dbc = dbc;
        }
        private const int varTagToNDS = 1104;


    }
}
