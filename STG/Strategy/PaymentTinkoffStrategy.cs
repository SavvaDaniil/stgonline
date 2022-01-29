using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using STG.DTO.Payment;
using STG.Facade;
using STG.Factory;
using STG.Entities;
using STG.Models.Tinkoff;
using STG.ViewModels.Payment;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using STG.Component;
using System;

namespace STG.Strategy
{
    public class PaymentTinkoffStrategy
    {
        private const string tinkoffUrltInit = "https://securepay.tinkoff.ru/v2/Init";
        private const string tinkoffUrltGetState = "https://securepay.tinkoff.ru/v2/GetState";
        private const string tinkoffUrltCharge = "https://securepay.tinkoff.ru/v2/Charge";
        private const string tinkoffUrltCansel = "https://securepay.tinkoff.ru/v2/Cancel";
        private const string TerminalKeyDEMO = "XXXXXXXXXXX";
        private const string PasswordDEMO = "XXXXXXXXXXX";
        private const string TerminalKey = "XXXXXXXXXXX";
        private const string Password = "XXXXXXXXXXX";

        public async Task<TinkoffInitResponse> init(bool isTest, string domainHost, Payment payment)
        {
            string terminalKey = isTest ? TerminalKeyDEMO : TerminalKey;

            TinkoffInitRequestFactory tinkoffInitRequestFactory = new TinkoffInitRequestFactory();
            TinkoffInitRequest tinkoffInitViewModel = payment.preUserWithAppointment != null
                ? tinkoffInitRequestFactory.createForPreUserWithAppointment(domainHost, payment.preUserWithAppointment.username, payment, terminalKey)
                : tinkoffInitRequestFactory.createForUsual(domainHost, payment, terminalKey);


            System.Diagnostics.Debug.WriteLine("tinkoffInitViewModel Init: " + tinkoffInitViewModel.Recurrent);

            var stringPayload = await Task.Run(() => JsonConvert.SerializeObject(tinkoffInitViewModel));
            var httpContent = new StringContent(stringPayload, Encoding.UTF8, "application/json");

            using (var httpClient = new HttpClient())
            {

                var httpResponse = await httpClient.PostAsync(tinkoffUrltInit, httpContent);

                if (!httpResponse.IsSuccessStatusCode) return null;
                if (httpResponse.Content == null) return null;

                string responseAsString = await httpResponse.Content.ReadAsStringAsync();
                System.Diagnostics.Debug.WriteLine("Ответ от init: " + responseAsString);

                JObject responseAsJson = JObject.Parse(responseAsString);

                try
                {
                    string rebillID = "";
                    if (responseAsJson["RebillID"] != null)
                    {
                        rebillID = (string)responseAsJson["RebillID"];
                    }
                    TinkoffInitResponse tinkoffInitDTO = new TinkoffInitResponse(
                        (bool)responseAsJson["Success"],
                        (int)responseAsJson["PaymentId"],
                        (string)responseAsJson["PaymentURL"],
                        (string)responseAsJson["Message"],
                        (string)responseAsJson["Details"],
                        (int)responseAsJson["ErrorCode"],
                        rebillID
                    );
                    return tinkoffInitDTO;
                } catch 
                {
                    //System.Diagnostics.Debug.WriteLine("Ошибка создания TinkoffInitDTO : " + ex.ToString()) ;
                    return null;
                }
            }
        }


        public async Task<TinkoffGetStateDTO> getState(bool is_test, Payment payment)
        {
            var crypt = new System.Security.Cryptography.SHA256Managed();
            StringBuilder hash = new System.Text.StringBuilder();
            byte[] crypto = crypt.ComputeHash(Encoding.UTF8.GetBytes((is_test ? PasswordDEMO : Password) + payment.tinkoffPaymentId + (is_test ? TerminalKeyDEMO : TerminalKey)));
            foreach (byte theByte in crypto)
            {
                hash.Append(theByte.ToString("x2"));
            }
            string token = hash.ToString();
            System.Diagnostics.Debug.WriteLine("token: " + token);

            TinkoffGetStateViewModel tinkoffGetStateViewModel = new TinkoffGetStateViewModel(
                (is_test ? TerminalKeyDEMO : TerminalKey),
                payment.tinkoffPaymentId,
                token
            );

            var tinkoffGetStateDTOJson = await Task.Run(() => JsonConvert.SerializeObject(tinkoffGetStateViewModel));
            var httpContent = new StringContent(tinkoffGetStateDTOJson, Encoding.UTF8, "application/json");

            using (var httpClient = new HttpClient())
            {

                var httpResponse = await httpClient.PostAsync(tinkoffUrltGetState, httpContent);

                if (!httpResponse.IsSuccessStatusCode) return null;
                if (httpResponse.Content == null) return null;

                string responseAsString = await httpResponse.Content.ReadAsStringAsync();
                System.Diagnostics.Debug.WriteLine("getState: " + responseAsString);
                LoggerComponent.writeToLogTinkoff("getState payment.id: " + payment.id + " responseAsString: " + responseAsString);

                JObject responseAsJson = JObject.Parse(responseAsString);

                try
                {
                    TinkoffGetStateDTO tinkoffGetStateDTO = new TinkoffGetStateDTO(
                        (bool)responseAsJson["Success"],
                        (string)responseAsJson["Status"],
                        (int)responseAsJson["ErrorCode"],
                        (string)responseAsJson["Message"],
                        (string)responseAsJson["Details"]
                    );
                    return tinkoffGetStateDTO;
                }
                catch
                {
                    //System.Diagnostics.Debug.WriteLine("Ошибка создания TinkoffInitDTO : " + ex.ToString()) ;
                    return null;
                }
            }
        }


        public async Task<TinkoffInitResponse> initExtendCharge(bool isTest, User user, int tinkoffPaymentIdOfNewPayment, string tinkoffRebillID)
        {
            string terminalKey = isTest ? TerminalKeyDEMO : TerminalKey;

            TinkoffInitRequestFactory tinkoffInitRequestFactory = new TinkoffInitRequestFactory();
            TinkoffChargeRequest tinkoffChargeRequest = tinkoffInitRequestFactory.createForProlongationCharge(
                isTest,
                user.Username,
                tinkoffPaymentIdOfNewPayment,
                tinkoffRebillID
            );

            var tinkoffGetStateDTOJson = await Task.Run(() => JsonConvert.SerializeObject(tinkoffChargeRequest));
            var httpContent = new StringContent(tinkoffGetStateDTOJson, Encoding.UTF8, "application/json");

            using (var httpClient = new HttpClient())
            {

                var httpResponse = await httpClient.PostAsync(tinkoffUrltCharge, httpContent);

                if (!httpResponse.IsSuccessStatusCode) return null;
                if (httpResponse.Content == null) return null;

                string responseAsString = await httpResponse.Content.ReadAsStringAsync();
                System.Diagnostics.Debug.WriteLine("initExtendCharge responseAsString: " + responseAsString);

                JObject responseAsJson = JObject.Parse(responseAsString);

                try
                {
                    TinkoffInitResponse tinkoffInitDTO = new TinkoffInitResponse(
                        (bool)responseAsJson["Success"],
                        (int)responseAsJson["PaymentId"],
                        (string)responseAsJson["PaymentURL"],
                        (string)responseAsJson["Message"],
                        (string)responseAsJson["Details"],
                        (int)responseAsJson["ErrorCode"]
                    );

                    return tinkoffInitDTO;
                }
                catch
                {
                    //System.Diagnostics.Debug.WriteLine("Ошибка создания TinkoffInitDTO : " + ex.ToString()) ;
                    return null;
                }
            }
        }



        public async Task<TinkoffCanselResponse> initCansel(Payment payment)
        {

            TinkoffInitRequestFactory tinkoffInitRequestFactory = new TinkoffInitRequestFactory();
            TinkoffCanselRequest tinkoffCanselRequest = tinkoffInitRequestFactory.createForCansel(payment);

            var tinkoffGetStateDTOJson = await Task.Run(() => JsonConvert.SerializeObject(tinkoffCanselRequest));
            var httpContent = new StringContent(tinkoffGetStateDTOJson, Encoding.UTF8, "application/json");

            using (var httpClient = new HttpClient())
            {

                var httpResponse = await httpClient.PostAsync(tinkoffUrltCansel, httpContent);

                string responseAsString = await httpResponse.Content.ReadAsStringAsync();
                System.Diagnostics.Debug.WriteLine("initCansel responseAsString: " + responseAsString);
                LoggerComponent.writeToLogTinkoff("Отмена платежа payment.id: " + payment.id + " responseAsString: " + responseAsString);

                if (!httpResponse.IsSuccessStatusCode) return null;
                if (httpResponse.Content == null) return null;


                JObject responseAsJson = JObject.Parse(responseAsString);

                try
                {
                    TinkoffCanselResponse tinkoffCanselResponse = new TinkoffCanselResponse(
                        (bool)responseAsJson["Success"],
                        (string)responseAsJson["ErrorCode"],
                        (string)responseAsJson["TerminalKey"],
                        (string)responseAsJson["Status"],
                        (string)responseAsJson["PaymentId"],
                        (string)responseAsJson["OrderId"],
                        (int)responseAsJson["OriginalAmount"],
                        (int)responseAsJson["NewAmount"]
                    );

                    return tinkoffCanselResponse;
                }
                catch(Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("Ошибка initCansel : " + ex.ToString()) ;
                    LoggerComponent.writeToLogTinkoff("Ошибка отмены платежа payment.id: " + payment.id + " ex: " + ex.ToString());
                    return null;
                }
            }
        }
    }
}
