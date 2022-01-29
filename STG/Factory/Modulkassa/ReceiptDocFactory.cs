using STG.Data;
using STG.Entities;
using STG.Models.Modulkassa;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace STG.Factory.Modulkassa
{
    public class ReceiptDocFactory
    {
        private ApplicationDbContext _dbc;
        public ReceiptDocFactory(ApplicationDbContext dbc)
        {
            _dbc = dbc;
        }

    }
}
