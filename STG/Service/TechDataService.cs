using Microsoft.EntityFrameworkCore;
using STG.Data;
using STG.Entities;
using STG.Models.AmoCRM;
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


        public bool isRobokassaActive()
        {
            int isRobokassaActive = getIntValue("is_robokassa_active");
            return (isRobokassaActive == 1 ? true : false);
        }

        public void launchRobokassa()
        {
            update("is_robokassa_active", null, 1);
        }

        public AmoCRMModel getAmoCRMModelData()
        {
            int expires_in = getIntValue("amocrm_expires_in");
            string token_type = getStrValue("amocrm_token_type");
            string access_token = getStrValue("amocrm_access_token");
            string refresh_token = getStrValue("amocrm_refresh_token");
            int date_of_set_str = getIntValue("amocrm_date_of_set_str");

            string baseURL = getStrValue("amocrm_baseURL");
            string clientId = getStrValue("amocrm_clientId");
            string secretKey = getStrValue("amocrm_secretKey");
            string redirectUri = getStrValue("amocrm_redirectUri");

            return new AmoCRMModel(
                false,
                expires_in,
                token_type,
                access_token,
                refresh_token,
                date_of_set_str,
                baseURL,
                clientId,
                secretKey,
                redirectUri
            );
        }



        private string getStrValue(string name)
        {
            TechData techData = _dbc.TechDatas
                .Where(p => p.name == name)
                .OrderByDescending(p => p.id)
                .FirstOrDefault();

            if (techData == null)
            {
                insertLostData(name, null, 0);
                return null;
            }

            return techData.strValue;
        }

        private int getIntValue(string name)
        {
            TechData techData = _dbc.TechDatas
                .Where(p => p.name == name)
                .OrderByDescending(p => p.id)
                .FirstOrDefault();

            if (techData == null)
            {
                insertLostData(name, null, 0);
                return 0;
            }

            return techData.intValue;
        }

        private bool update(string name, string strValue, int intValue = 0)
        {
            TechData techData = _dbc.TechDatas
                .Where(p => p.name == name)
                .OrderByDescending(p => p.id)
                .FirstOrDefault();

            if (techData == null)
            {
                if (strValue != null) insertLostData(name, strValue, 0);
                if (intValue != 0) insertLostData(name, null, intValue);

                return true;
            }
            if (strValue != null) techData.strValue = strValue;
            if (intValue != 0) techData.intValue = intValue;
            _dbc.SaveChanges();

            return true;
        }

        private bool insertLostData(string name, string strValue = null, int intValue = 0)
        {
            TechData techData = new TechData();
            techData.name = name;
            techData.intValue = 0;
            if (strValue != null) techData.strValue = strValue;
            if (intValue != 0) techData.intValue = intValue;
            _dbc.TechDatas.Add(techData);
            _dbc.SaveChanges();
            return true;
        }
    }
}
