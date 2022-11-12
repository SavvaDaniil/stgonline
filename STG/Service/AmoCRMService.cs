using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using STG.Component;
using STG.Data;
using STG.DTO.AmoCRM;
using STG.Entities;
using STG.Factory;
using STG.Models;
using STG.Models.AmoCRM;
using STG.ViewModels.AmoCRM;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace STG.Service
{
    public class AmoCRMService
    {
        private const string _amoCRMUrlAppenPathLeads = "/api/v4/leads";
        private const string _amoCRMUrlAppenPathContacts = "/api/v4/contacts";

        //private const string amoCRMBaseDomain = "https://irinakostoyakova.amocrm.ru";
        //private string amocrmPath = Directory.GetCurrentDirectory() + "\\amocrm.xml";
        private const string _amoCRMUrlAppenPathRefreshToken = "/oauth2/access_token";
        //private const string amoCRMClientID = "def1920f-edff-42f8-991c-aa72ea685cbd";
        //private const string amoCRMSecretKey = "rUwlvZ7w01QeJodPZP5ARiFiuyauk9cq8bCziCOVR2zihJ6y6gxIjhEV5fxJEabS";
        //private const string amoCRMRedirectUri = "https://stgonline.pro";



        private ApplicationDbContext _dbc;
        public AmoCRMService(ApplicationDbContext dbc)
        {
            this._dbc = dbc;
        }

        //newContactToUpdate
        public AmoCRMNewContactResponse addUserToAmoCRMAndGetIdInAmoCRM(AmoCRMModel amoCRMModel, User user)
        {
            return null;
            /*
            AmoCRMFactory amoCRMFactory = new AmoCRMFactory();

            AmoCRMNewContact[] contacts = new AmoCRMNewContact[] {
                amoCRMFactory.createAmoCRMNewContact(
                    user.Id.ToString(),
                    user.firstname,
                    user.secondname,
                    user.Username,
                    user.phone,
                    user.dateOfAdd
                )
            };

            var stringPayload = Task.Run(() => JsonConvert.SerializeObject(contacts));
            var httpContent = new StringContent(stringPayload, Encoding.UTF8, "application/json");

            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", amoCRMModel.access_token);
                var httpResponse = httpClient.Post(amoCRMModel.baseURL + _amoCRMUrlAppenPathContacts, httpContent);

                if (!httpResponse.IsSuccessStatusCode)
                {
                    //System.Diagnostics.Debug.WriteLine("httpResponse.IsSuccessStatusCode: " + httpResponse.IsSuccessStatusCode);
                    return null;
                }
                if (httpResponse.Content == null) return null;

                string responseAsString = httpResponse.Content.ReadAsString();
                //System.Diagnostics.Debug.WriteLine("Ответ от newContact: " + responseAsString);

                JObject responseAsJson = JObject.Parse(responseAsString);

                try
                {
                    AmoCRMNewContactResponse amoCRMNewContactResponse = new AmoCRMNewContactResponse(
                        (int)responseAsJson["_embedded"]["contacts"][0]["id"]
                    );
                    httpClient.Dispose();
                    return amoCRMNewContactResponse;
                }
                catch(Exception ex)
                {
                    httpClient.Dispose();
                    System.Diagnostics.Debug.WriteLine("Ошибка создания AmoCRMNewContactResponse : " + ex.Message) ;
                    return null;
                }
            }
            */
        }




        public AmoCRMLeadsViewModel getLeads(AmoCRMModel amoCRMModel)
        {
            return null;
            /*
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", amoCRMModel.access_token);
                var httpResponse = httpClient.Get(amoCRMModel.baseURL + _amoCRMUrlAppenPathLeads);

                if (!httpResponse.IsSuccessStatusCode)
                {
                    System.Diagnostics.Debug.WriteLine("httpResponse.IsSuccessStatusCode: " + httpResponse.IsSuccessStatusCode);
                    return null;
                }
                if (httpResponse.Content == null) return null;

                string responseAsString = httpResponse.Content.ReadAsString();

                //JObject responseAsJson = JObject.Parse(responseAsString);

                try
                {
                    AmoCRMLeadsViewModel amoCRMLeadsViewModel = new AmoCRMLeadsViewModel();

                    dynamic responseAsJson = JsonConvert.DeserializeObject(responseAsString);

                    try
                    {
                        foreach (var amoCRMLead in responseAsJson._embedded.leads)
                        {
                            System.Diagnostics.Debug.WriteLine((string)amoCRMLead.name);
                        }

                    } catch(Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine(ex.Message);
                    }

                    return null;
                }
                catch
                {
                    //System.Diagnostics.Debug.WriteLine("Ошибка создания TinkoffInitDTO : " + ex.ToString()) ;
                    return null;
                }
            }
            */
        }



        public int getUserIdInAmoCRM(AmoCRMModel amoCRMModel, int id_in_amocrm)
        {
            /*
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", amoCRMModel.access_token);
                var httpResponse = await httpClient.Get(amoCRMModel.baseURL + _amoCRMUrlAppenPathContacts + "/"+ id_in_amocrm);

                if (!httpResponse.IsSuccessStatusCode)
                {
                    //System.Diagnostics.Debug.WriteLine("httpResponse.IsSuccessStatusCode: " + httpResponse.IsSuccessStatusCode);
                    return 0;
                }
                if (httpResponse.Content == null) return 0;

                string responseAsString = httpResponse.Content.ReadAsString();
                //System.Diagnostics.Debug.WriteLine("Ответ от getUserIdInAmoCRM: " + responseAsString);

                try
                {
                    JObject responseAsJson = JObject.Parse(responseAsString);

                    httpClient.Dispose();
                    if (responseAsJson["id"] == null) return 0;
                    return (int)responseAsJson["id"];
                }
                catch (Exception ex)
                {
                    httpClient.Dispose();
                    System.Diagnostics.Debug.WriteLine("Ошибка создания getUserIdInAmoCRM : " + ex.Message);
                    return 0;
                }
            }
            */
            return 0;
        }





        public bool newLead(AmoCRMModel amoCRMModel, AmoCRMNewLead[] leads)
        {
            return false;
            /*
            var stringPayload = Task.Run(() => JsonConvert.SerializeObject(leads));
            var httpContent = new StringContent(stringPayload, Encoding.UTF8, "application/json");

            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", amoCRMModel.access_token);
                var httpResponse = httpClient.Post(amoCRMModel.baseURL + _amoCRMUrlAppenPathLeads, httpContent);

                string responseAsString = httpResponse.Content.ReadAsString();
                //System.Diagnostics.Debug.WriteLine("Ответ от newLeadNewUser: " + responseAsString);
                LoggerComponent.writeToLog("Ответ от newLeadNewUser: " + responseAsString);

                if (!httpResponse.IsSuccessStatusCode)
                {
                    //System.Diagnostics.Debug.WriteLine("httpResponse.IsSuccessStatusCode: " + httpResponse.IsSuccessStatusCode);
                    LoggerComponent.writeToLog("newLead httpResponse.IsSuccessStatusCode: " + httpResponse.IsSuccessStatusCode);
                    return false;
                }
                if (httpResponse.Content == null) return false;


                //JObject responseAsJson = JObject.Parse(responseAsString);

                httpClient.Dispose();
                return true;
            }
            */
        }




        public AmoCRMModel getState()
        {
            int expires_in = getIntValue("expires_in");
            string token_type = getStrValue("token_type");
            string access_token = getStrValue("access_token");
            string refresh_token = getStrValue("refresh_token");
            int date_of_set_str = getIntValue("date_of_set_str");

            string baseURL = getStrValue("baseURL");
            string clientId = getStrValue("clientId");
            string secretKey = getStrValue("secretKey");
            string redirectUri = getStrValue("redirectUri");

            if (expires_in == 0 || access_token == null || refresh_token == null || date_of_set_str == 0)
            {
                return null;
            }

            //проверяем на срок годности
            if (date_of_set_str + expires_in <= (int)(((DateTimeOffset)DateTime.Now).ToUnixTimeSeconds()))
            {
                return initRefreshTokenRequest(refresh_token, baseURL, clientId, secretKey, redirectUri);
            }

            return new AmoCRMModel(
                true,
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

        private AmoCRMModel initRefreshTokenRequest(string refresh_token, string baseURL, string clientId, string secretKey, string redirectUri)
        {
            return null;
            /*
            System.Diagnostics.Debug.WriteLine("Вызов initRefreshTokenRequest");

            AmoCRMRefreshTokenRequest amoCRMRefreshTokenRequest = new AmoCRMRefreshTokenRequest(
                clientId,
                secretKey,
                "refresh_token",
                refresh_token,
                redirectUri
            );

            var tinkoffGetStateDTOJson = Task.Run(() => JsonConvert.SerializeObject(amoCRMRefreshTokenRequest));
            var httpContent = new StringContent(tinkoffGetStateDTOJson, Encoding.UTF8, "application/json");

            using (var httpClient = new HttpClient())
            {
                var httpResponse = await httpClient.PostAsync(baseURL + _amoCRMUrlAppenPathRefreshToken, httpContent);

                string responseAsString = httpResponse.Content.ReadAsString();
                System.Diagnostics.Debug.WriteLine("AmoCRM refresh token: " + responseAsString);

                if (!httpResponse.IsSuccessStatusCode) return null;
                if (httpResponse.Content == null) return null;

                JObject responseAsJson = JObject.Parse(responseAsString);

                try
                {
                    AmoCRMRefreshTokenResponse amoCRMRefreshTokenResponse = new AmoCRMRefreshTokenResponse(
                        (string)responseAsJson["token_type"],
                        (int)responseAsJson["expires_in"],
                        (string)responseAsJson["access_token"],
                        (string)responseAsJson["refresh_token"]
                    );
                    httpClient.Dispose();

                    return updateAfterRefreshToken(amoCRMRefreshTokenResponse);
                }
                catch (Exception ex)
                {
                    httpClient.Dispose();
                    System.Diagnostics.Debug.WriteLine("Ошибка AmoCRM refresh token: : " + ex.ToString());
                    return null;
                }
            }
            */
        }

        private AmoCRMModel updateAfterRefreshToken(AmoCRMRefreshTokenResponse amoCRMRefreshTokenResponse)
        {
            DateTime dateNow = DateTime.Now;
            int date_of_set_str = (int)(((DateTimeOffset)DateTime.Now).ToUnixTimeSeconds());
            update("token_type", amoCRMRefreshTokenResponse.token_type);
            update("expires_in", null, amoCRMRefreshTokenResponse.expires_in);
            update("access_token", amoCRMRefreshTokenResponse.access_token);
            update("refresh_token", amoCRMRefreshTokenResponse.refresh_token);
            update("date_of_set_str", null, (int)(((DateTimeOffset)dateNow).ToUnixTimeSeconds()));
            update("date_of_set_str", dateNow.ToString("yyyy-MM-dd HH:ii:mm"));

            /*
            return new AmoCRMModel(
                true,
                amoCRMRefreshTokenResponse.expires_in,
                amoCRMRefreshTokenResponse.token_type,
                amoCRMRefreshTokenResponse.access_token,
                amoCRMRefreshTokenResponse.refresh_token,
                date_of_set_str
            );
            */
            return getState();
        }








        private string getStrValue(string name)
        {
            AmoCRMData amoCRMData = _dbc.AmoCRMDatas
                .Where(p => p.name == name)
                .OrderByDescending(p => p.id)
                .FirstOrDefault();

            if(amoCRMData == null)
            {
                insertLostData(name, null, 0);
                return null;
            }

            return amoCRMData.strValue;
        }

        private int getIntValue(string name)
        {
            AmoCRMData amoCRMData = _dbc.AmoCRMDatas
                .Where(p => p.name == name)
                .OrderByDescending(p => p.id)
                .FirstOrDefault();

            if (amoCRMData == null)
            {
                insertLostData(name, null, 0);
                return 0;
            }

            return amoCRMData.intValue;
        }

        private bool update(string name, string strValue, int intValue = 0)
        {
            AmoCRMData amoCRMData = _dbc.AmoCRMDatas
                .Where(p => p.name == name)
                .OrderByDescending(p => p.id)
                .FirstOrDefault();

            if (amoCRMData == null)
            {
                if (strValue != null)insertLostData(name, strValue, 0);
                if (intValue != 0) insertLostData(name, null, intValue);

                return true;
            }
            if (strValue != null) amoCRMData.strValue = strValue;
            if (intValue != 0) amoCRMData.intValue = intValue;
            _dbc.SaveChanges();

            return true;
        }

        private bool insertLostData(string name, string strValue = null, int intValue = 0)
        {
            AmoCRMData amoCRMData = new AmoCRMData();
            amoCRMData.name = name;
            amoCRMData.intValue = 0;
            if (strValue != null) amoCRMData.strValue = strValue;
            if (intValue != 0) amoCRMData.intValue = intValue;
            _dbc.AmoCRMDatas.Add(amoCRMData);
            _dbc.SaveChanges();
            return true;
        }


    }
}




/*
public  Task<AmoCRMStatusViewModel> initRefreshTokenRequest(AmoCRMXML amoCRMXML)
{
    System.Diagnostics.Debug.WriteLine("Вызов initRefreshTokenRequest");

    AmoCRMRefreshTokenRequest amoCRMRefreshTokenRequest = new AmoCRMRefreshTokenRequest(
        amoCRMClientID,
        amoCRMSecretKey,
        "refresh_token",
        amoCRMXML.refresh_token,
        amoCRMRedirectUri
    );

    var tinkoffGetStateDTOJson = Task.Run(() => JsonConvert.SerializeObject(amoCRMRefreshTokenRequest));
    var httpContent = new StringContent(tinkoffGetStateDTOJson, Encoding.UTF8, "application/json");

    using (var httpClient = new HttpClient())
    {

        var httpResponse = httpClient.Post(amoCRMUrlRefreshToken, httpContent);

        if (!httpResponse.IsSuccessStatusCode) return null;
        if (httpResponse.Content == null) return null;

        string responseAsString = httpResponse.Content.ReadAsString();
        System.Diagnostics.Debug.WriteLine("AmoCRM refresh token: " + responseAsString);

        JObject responseAsJson = JObject.Parse(responseAsString);

        try
        {
            AmoCRMRefreshTokenResponse amoCRMRefreshTokenResponse = new AmoCRMRefreshTokenResponse(
                (string)responseAsJson["token_type"],
                (int)responseAsJson["expires_in"],
                (string)responseAsJson["access_token"],
                (string)responseAsJson["refresh_token"]
            );
            httpClient.Dispose();

            if(! updateAfterRefreshToken(amoCRMRefreshTokenResponse))return null;


            return new AmoCRMStatusViewModel(
                "success",
                null,
                true,
                amoCRMRefreshTokenResponse.access_token
            );
        }
        catch
        {
            httpClient.Dispose();
            //System.Diagnostics.Debug.WriteLine("Ошибка создания TinkoffInitDTO : " + ex.ToString()) ;
            return null;
        }
    }
}


private bool updateAfterRefreshToken(AmoCRMRefreshTokenResponse amoCRMRefreshTokenResponse)
{
    if (File.Exists(amocrmPath))
    {
        File.Delete(amocrmPath);
    }

    AmoCRMXML amoCRMXML = new AmoCRMXML();
    amoCRMXML.date_of_set_str = (int)((DateTimeOffset)DateTime.Now).ToUnixTimeSeconds();
    amoCRMXML.expires_in = amoCRMRefreshTokenResponse.expires_in;
    amoCRMXML.access_token = amoCRMRefreshTokenResponse.access_token;
    amoCRMXML.refresh_token = amoCRMRefreshTokenResponse.refresh_token;
    amoCRMXML.status = true;

    XmlSerializer formatter = new XmlSerializer(typeof(AmoCRMXML));
    System.Diagnostics.Debug.WriteLine("updateAfterRefreshToken Готовится сериализация");

    using (FileStream fs = new FileStream(amocrmPath, FileMode.OpenOrCreate))
    {
        formatter.Serialize(fs, amoCRMXML);
        System.Diagnostics.Debug.WriteLine("updateAfterRefreshToken Объект сериализован");
        fs.Dispose();
    }

    return true;
}
*/
