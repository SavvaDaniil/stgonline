using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using STG.Component;
using STG.Data;
using STG.Factory;
using STG.Models;
using STG.Entities;
using STG.Models.AmoCRM;
using STG.Service;
using STG.ViewModels.AmoCRM;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace STG.Facade
{
    public class AmoCRMFacade
    {
        private const string amoCRMUrlBase = "XXXXXXXXXXX";
        private string amocrmPath = Directory.GetCurrentDirectory() + "\\amocrm.xml";
        private const string amoCRMUrlRefreshToken = amoCRMUrlBase + "/oauth2/access_token";
        private const string amoCRMClientID = "XXXXXXXXXXX";
        private const string amoCRMSecretKey = "XXXXXXXXXXX";
        private const string amoCRMRedirectUri = "https://stgonline.pro";


        private Dictionary<string, int> dictOfKeyLeads = new Dictionary<string, int> {
            { "newUser", 0 },
            { "newStatement", 0},
            { "newPurchaseLesson", 0},
            { "newPurchaseSubscriptionWithoutProlongation", 0},
            { "newPurchaseSubscriptionWithProlongation", 0},
            { "test", 0}
        };


        private ApplicationDbContext _dbc;
        public AmoCRMFacade(ApplicationDbContext dbc)
        {
            this._dbc = dbc;
        }


        public async Task<AmoCRMStatusViewModel> test()
        {
            /*
            AmoCRMStatusViewModel amoCRMStatusViewModel = await getState();
            if (amoCRMStatusViewModel == null)
            {
                return new AmoCRMStatusViewModel("error", "Ошибка в файле");
            }
            */

            UserService userService = new UserService(_dbc);
            AmoCRMService amoCRMService = new AmoCRMService(_dbc);
            AmoCRMModel amoCRMModel = await amoCRMService.getState();

            User user = await userService.findById(7);

            //await amoCRMService.getLeads(amoCRMXML);
            if (user != null)
            {
                //await amoCRMService.newContact(amoCRMStatusViewModel, user);
                //await newLeadTest(user);
                //await newPurchaseSubscription(amoCRMStatusViewModel, user, null);

            }


            return new AmoCRMStatusViewModel("success", null);
        }

        public async Task<bool> newLeadTest(User user)
        {
            AmoCRMService amoCRMService = new AmoCRMService(_dbc);
            AmoCRMModel amoCRMModel = await amoCRMService.getState();
            if (amoCRMModel == null) return false;

            int id_in_amocrm = await getIdInAmoCRM(amoCRMModel, user);
            user.id_in_amocrm = id_in_amocrm;

            if (dictOfKeyLeads["test"] == 0) return false;
            AmoCRMFactory amoCRMFactory = new AmoCRMFactory();
            AmoCRMNewLead[] leads = new AmoCRMNewLead[] {
                amoCRMFactory.createLeadTest(
                    user,
                    dictOfKeyLeads["test"]
                )
            };

            return await amoCRMService.newLead(amoCRMModel, leads);
        }


        public async Task<bool> newLeadNewUser(User user)
        {
            AmoCRMService amoCRMService = new AmoCRMService(_dbc);
            AmoCRMModel amoCRMModel = await amoCRMService.getState();
            if (amoCRMModel == null) return false;

            int id_in_amocrm = await getIdInAmoCRM(amoCRMModel, user);
            user.id_in_amocrm = id_in_amocrm;

            if (dictOfKeyLeads["newUser"] == 0) return false;
            AmoCRMFactory amoCRMFactory = new AmoCRMFactory();
            AmoCRMNewLead[] leads = new AmoCRMNewLead[] {
                amoCRMFactory.createLeadNewUser(
                    user,
                    dictOfKeyLeads["newUser"]
                )
            };

            return await amoCRMService.newLead(amoCRMModel, leads);
        }


        public async Task<bool> newLeadNewStatement(User user, Payment payment, Statement statement)
        {
            AmoCRMService amoCRMService = new AmoCRMService(_dbc);
            AmoCRMModel amoCRMModel = await amoCRMService.getState();
            if (amoCRMModel == null) return false;

            int id_in_amocrm = await getIdInAmoCRM(amoCRMModel, user);
            user.id_in_amocrm = id_in_amocrm;

            if (dictOfKeyLeads["newStatement"] == 0) return false;
            AmoCRMFactory amoCRMFactory = new AmoCRMFactory();
            AmoCRMNewLead[] leads = new AmoCRMNewLead[] {
                amoCRMFactory.createLeadNewStatement(
                    user,
                    payment,
                    statement,
                    dictOfKeyLeads["newStatement"]
                )
            };

            return await amoCRMService.newLead(amoCRMModel, leads);
        }


        public async Task<bool> newPurchaseLesson(User user, Payment payment, Lesson lesson)
        {
            AmoCRMService amoCRMService = new AmoCRMService(_dbc);
            AmoCRMModel amoCRMModel = await amoCRMService.getState();
            if (amoCRMModel == null) return false;

            int id_in_amocrm = await getIdInAmoCRM(amoCRMModel, user);
            user.id_in_amocrm = id_in_amocrm;

            if (dictOfKeyLeads["newPurchaseLesson"] == 0) return false;
            AmoCRMFactory amoCRMFactory = new AmoCRMFactory();
            AmoCRMNewLead[] leads = new AmoCRMNewLead[] {
                amoCRMFactory.createLeadNewPurchaseLesson(
                    user,
                    payment,
                    lesson,
                    dictOfKeyLeads["newPurchaseLesson"]
                )
            };

            Thread th = new Thread(async () => await amoCRMService.newLead(amoCRMModel, leads));
            th.Start();

            return true;
        }

        public async Task<bool> newPurchaseSubscription(User user, Payment payment, Subscription subscription)
        {
            System.Diagnostics.Debug.WriteLine("newPurchaseSubscription");
            AmoCRMService amoCRMService = new AmoCRMService(_dbc);
            AmoCRMModel amoCRMModel = await amoCRMService.getState();
            if (amoCRMModel == null)
            {
                LoggerComponent.writeToLog("amoCRMStatusViewModel == null");
                return false;
            }

            int id_in_amocrm = await getIdInAmoCRM(amoCRMModel, user);
            user.id_in_amocrm = id_in_amocrm;

            if (dictOfKeyLeads["newPurchaseSubscriptionWithoutProlongation"] == 0 || dictOfKeyLeads["newPurchaseSubscriptionWithProlongation"] == 0) return false;

            int id_of_lead = (payment.isProlongation == 0
                ? dictOfKeyLeads["newPurchaseSubscriptionWithoutProlongation"]
                : dictOfKeyLeads["newPurchaseSubscriptionWithProlongation"]
            );

            AmoCRMFactory amoCRMFactory = new AmoCRMFactory();
            AmoCRMNewLead[] leads = new AmoCRMNewLead[] {
                amoCRMFactory.createLeadNewPurchaseSubscription(
                    user,
                    payment,
                    subscription,
                    id_of_lead
                )
            };

            Thread th = new Thread(async () => await amoCRMService.newLead(amoCRMModel, leads));
            th.Start();

            return true;
        }

        public async Task<bool> newLeadExtend(User user, Payment payment, Subscription subscription)
        {
            AmoCRMService amoCRMService = new AmoCRMService(_dbc);
            AmoCRMModel amoCRMModel = await amoCRMService.getState();
            if (amoCRMModel == null) return false;

            int id_in_amocrm = await getIdInAmoCRM(amoCRMModel, user);
            user.id_in_amocrm = id_in_amocrm;

            AmoCRMFactory amoCRMFactory = new AmoCRMFactory();
            AmoCRMNewLead[] leads = new AmoCRMNewLead[] {
                amoCRMFactory.createLeadExtend(
                    user,
                    payment,
                    subscription
                )
            };

            return await amoCRMService.newLead(amoCRMModel, leads);
        }





        private async Task<int> getIdInAmoCRM(AmoCRMModel amoCRMModel, User user)
        {
            int id_in_amocrm = 0;
            AmoCRMService amoCRMService = new AmoCRMService(_dbc);
            if (user.id_in_amocrm == 0)
            {
                id_in_amocrm = await updateUserIdInAmoCRM(amoCRMModel, user);
            } else
            {
                id_in_amocrm = await amoCRMService.getUserIdInAmoCRM(amoCRMModel, user.id_in_amocrm);
                if (id_in_amocrm == 0) id_in_amocrm = await updateUserIdInAmoCRM(amoCRMModel, user);
            }
            return id_in_amocrm;
        }

        private async Task<int> updateUserIdInAmoCRM(AmoCRMModel amoCRMModel, User user)
        {
            AmoCRMService amoCRMService = new AmoCRMService(_dbc);
            AmoCRMNewContactResponse amoCRMNewContactResponse = await amoCRMService.newContactToUpdate(amoCRMModel, user);
            if (amoCRMNewContactResponse == null) return 0;
            UserFacade userFacade = new UserFacade(_dbc);
            if (!await userFacade.updateUserIdInAmoCRM(user, amoCRMNewContactResponse.id)) return 0;
            user.id_in_amocrm = amoCRMNewContactResponse.id;
            return amoCRMNewContactResponse.id;
        }






        public void createEmpty()
        {
            System.Diagnostics.Debug.WriteLine("createEmpty()");
            AmoCRMXML amoCRMXML = new AmoCRMXML();
            amoCRMXML.date_of_set_str = (int)((DateTimeOffset)DateTime.Now).ToUnixTimeSeconds();
            amoCRMXML.access_token = "XXXXXXXX";
            amoCRMXML.expires_in = 0;
            amoCRMXML.refresh_token = "0";
            amoCRMXML.token_type = "XXXXXXXX";
            amoCRMXML.status = false;

            XmlSerializer formatter = new XmlSerializer(typeof(AmoCRMXML));
            using (FileStream fs = new FileStream(amocrmPath, FileMode.OpenOrCreate))
            {
                formatter.Serialize(fs, amoCRMXML);
            }
        }

        public bool saveState()
        {
            AmoCRMXML amoCRMXML = new AmoCRMXML();
            amoCRMXML.date_of_set_str = (int)((DateTimeOffset)DateTime.Now).ToUnixTimeSeconds();

            XmlSerializer formatter = new XmlSerializer(typeof(AmoCRMXML));
            System.Diagnostics.Debug.WriteLine("Готовится сериализация");

            // получаем поток, куда будем записывать сериализованный объект
            using (FileStream fs = new FileStream(amocrmPath, FileMode.OpenOrCreate))
            {
                formatter.Serialize(fs, amoCRMXML);
                System.Diagnostics.Debug.WriteLine("Объект сериализован");
            }

            return true;
        }

        public void updateAccessTokenByRefreshToken()
        {
            AmoCRMService amoCRMService = new AmoCRMService(_dbc);

        }

    }
}
