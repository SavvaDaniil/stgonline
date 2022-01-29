using Microsoft.EntityFrameworkCore;
using STG.Data;
using STG.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace STG.Service
{
    public class TechDataService
    {
        private ApplicationDbContext _dbc;
        public TechDataService(ApplicationDbContext dbc)
        {
            this._dbc = dbc;
        }


        public async Task<bool> isRobokassaActive()
        {
            int isRobokassaActive = await getIntValue("is_robokassa_active");
            return (isRobokassaActive == 1 ? true : false);
        }

        public async Task launchRobokassa()
        {
            await update("is_robokassa_active", null, 1);
        }


        private async Task<string> getStrValue(string name)
        {
            TechData techData = await _dbc.TechDatas
                .Where(p => p.name == name)
                .OrderByDescending(p => p.id)
                .FirstOrDefaultAsync();

            if (techData == null)
            {
                await insertLostData(name, null, 0);
                return null;
            }

            return techData.strValue;
        }

        private async Task<int> getIntValue(string name)
        {
            TechData techData = await _dbc.TechDatas
                .Where(p => p.name == name)
                .OrderByDescending(p => p.id)
                .FirstOrDefaultAsync();

            if (techData == null)
            {
                await insertLostData(name, null, 0);
                return 0;
            }

            return techData.intValue;
        }

        private async Task<bool> update(string name, string strValue, int intValue = 0)
        {
            TechData techData = await _dbc.TechDatas
                .Where(p => p.name == name)
                .OrderByDescending(p => p.id)
                .FirstOrDefaultAsync();

            if (techData == null)
            {
                if (strValue != null) await insertLostData(name, strValue, 0);
                if (intValue != 0) await insertLostData(name, null, intValue);

                return true;
            }
            if (strValue != null) techData.strValue = strValue;
            if (intValue != 0) techData.intValue = intValue;
            await _dbc.SaveChangesAsync();

            return true;
        }

        private async Task<bool> insertLostData(string name, string strValue = null, int intValue = 0)
        {
            TechData techData = new TechData();
            techData.name = name;
            techData.intValue = 0;
            if (strValue != null) techData.strValue = strValue;
            if (intValue != 0) techData.intValue = intValue;
            await _dbc.TechDatas.AddAsync(techData);
            await _dbc.SaveChangesAsync();
            return true;
        }
    }
}
