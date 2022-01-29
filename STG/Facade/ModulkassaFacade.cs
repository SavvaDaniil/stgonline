using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using STG.Component;
using STG.Data;
using STG.Entities;
using STG.Factory.Modulkassa;
using STG.Models.Modulkassa;
using STG.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace STG.Facade
{
    public class ModulkassaFacade
    {
        private ApplicationDbContext _dbc;
        public ModulkassaFacade(ApplicationDbContext dbc)
        {
            _dbc = dbc;
        }

        private const string urlSale = "https://service.modulpos.ru/api/fn";
        private const string urlDemoSale = "https://demo.modulpos.ru/api/fn";

        private const string authUsername = "1ff2acdb-d66d-48da-87c3-7ae88bc0ae3f";
        private const string authPassword = "nLfL8SWqAhFWmQ3C";
        private const string authUsernameDemo = "bdc55881-11d6-41f9-bf00-c00fe44e948e";
        private const string authPasswordDemo = "NxpWh1indwN46gjV";

        public async Task<ReceiptDocResponse> sell(Payment payment)
        {
            LoggerComponent.writeToLogModulkassa("ModulkassaFacade sell payment.id : " + payment.id);
            string username = "";
            //проверяем на пустоту user в чеке, и проверяем, может это заявка
            if (payment.user == null)
            {
                if (payment.preUserWithAppointment != null) {
                    PreUserWithAppointmentService preUserWithAppointmentService = new PreUserWithAppointmentService(_dbc);
                    PreUserWithAppointment preUserWithAppointment = await preUserWithAppointmentService.findById(payment.preUserWithAppointment.id);
                    if (preUserWithAppointment == null)
                    {
                        LoggerComponent.writeToLogModulkassa("ModulkassaFacade cell нет пользователя и заявки 1 payment.id : " + payment.id);
                        return null;
                    }
                    username = preUserWithAppointment.username;
                } else
                {
                    //нет не пользователя, ни заявки
                    LoggerComponent.writeToLogModulkassa("ModulkassaFacade cell нет пользователя и заявки 2 payment.id : " + payment.id);
                    return null;
                }
            } else
            {
                username = payment.user.Username;
            }

            ReceiptDocFactory receiptDocFactory = new ReceiptDocFactory(_dbc);
            ReceiptDoc receiptDoc = receiptDocFactory.createSale(payment, username);
            if (receiptDoc == null) return null;
            LoggerComponent.writeToLogModulkassa("ModulkassaFacade cell ReceiptDoc : " + receiptDoc.ToString());


            var stringPayload = await Task.Run(() => JsonConvert.SerializeObject(receiptDoc));
            var httpContent = new StringContent(stringPayload, Encoding.UTF8, "application/json");


            using (var httpClient = new HttpClient())
            {
                var byteArray = Encoding.ASCII.GetBytes((payment.isTest == 1 ? authUsernameDemo : authUsername)
                    + ":" + (payment.isTest == 1 ? authPasswordDemo : authPassword));
                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));

                var httpResponse = await httpClient.PostAsync((payment.isTest == 1 ? urlDemoSale : urlSale) + "/v2/doc", httpContent);

                string responseAsString = await httpResponse.Content.ReadAsStringAsync();
                System.Diagnostics.Debug.WriteLine("Ответ от ModulkassaFacade cell : " + responseAsString);
                LoggerComponent.writeToLogModulkassa("Ответ от отправки чека ModulkassaFacade cell : " + responseAsString);

                if (!httpResponse.IsSuccessStatusCode) return null;
                if (httpResponse.Content == null) return null;


                JObject responseAsJson = JObject.Parse(responseAsString);

                try
                {
                    return new ReceiptDocResponse(
                        (string)responseAsJson["status"],
                        (string)responseAsJson["fnState"],
                        (string)responseAsJson["fiscalInfo"],
                        (string)responseAsJson["failureInfo"],
                        (string)responseAsJson["message"],
                        (string)responseAsJson["timeStatusChanged"]
                    );
                }
                catch(Exception ex)
                {
                    LoggerComponent.writeToLogModulkassa("Ошибка отправки чека ModulkassaFacade cell : " + responseAsString);
                    System.Diagnostics.Debug.WriteLine("Ошибка ModulkassaFacade cell : " + ex.ToString()) ;
                    return null;
                }
            }
        }

    }
}
