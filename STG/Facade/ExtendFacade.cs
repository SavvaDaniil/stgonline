using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using STG.Data;
using STG.DTO.PurchaseSubscription;
using STG.Entities;
using STG.Models;
using STG.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace STG.Facade
{
    public class ExtendFacade
    {
        private ApplicationDbContext _dbc;
        private IServiceScopeFactory _serviceScopeFactory;
        public ExtendFacade(ApplicationDbContext dbc, IServiceScopeFactory serviceScopeFactory)
        {
            this._dbc = dbc;
            _serviceScopeFactory = serviceScopeFactory;
        }


        public async Task checkExtendsAndLaunch(string domainHost)
        {
            ExtendService extendService = new ExtendService(_dbc);
            IEnumerable<Extend> extendsNotFinished = await extendService.listAllNotFinished();
            PurchaseSubscriptionService purchaseSubscriptionService = new PurchaseSubscriptionService(_dbc);
            PurchaseSubscription purchaseSubscription;
            foreach (Extend extend in extendsNotFinished)
            {
                System.Diagnostics.Debug.WriteLine("------------- ExtendFacade checkExtendsAndLaunch extend: " + extend.id);
                purchaseSubscription = await purchaseSubscriptionService.findById(extend.id_of_purchase_subscription);
                if (purchaseSubscription == null || purchaseSubscription.user.prolongation == 0
                    || purchaseSubscription.isProlongation == 0 || purchaseSubscription.payment.isProlongation == 0) continue;
                await addLaunchExtendThread(domainHost, extend.user, purchaseSubscription, extend);
            }
        }

        public async Task<bool> checkAndLaunchIfNot(string domainHost, User user, PurchaseSubscription purchaseSubscription)
        {
            System.Diagnostics.Debug.WriteLine("ExtendFacade checkAndLaunchIfNot");
            if (purchaseSubscription.dateOfActivation != null)
            {
                //System.Diagnostics.Debug.WriteLine("checkAndLaunchIfNot Даты неправильно");
                //return false;
            }

            ExtendService extendService = new ExtendService(_dbc);

            //проверяем, может вдруг уже есть
            Extend extendAlreadyExist = await extendService.findNotFinished(user, purchaseSubscription.id);
            if (extendAlreadyExist != null)
            {
                System.Diagnostics.Debug.WriteLine("-------------- ExtendFacade extendAlreadyExist уже запущен");
                return true;
            }

            Extend extend = await extendService.add(user, purchaseSubscription.id);
            return await addLaunchExtendThread(domainHost, user, purchaseSubscription, extend);
        }

        private async Task<bool> addLaunchExtendThread(string domainHost, User user, PurchaseSubscription purchaseSubscription, Extend extend)
        {

            PurchaseSubscriptionFacade purchaseSubscriptionFacade = new PurchaseSubscriptionFacade(_dbc, _serviceScopeFactory);

            DateTime date_must_be_used_to = (DateTime)(purchaseSubscription.dateOfMustBeUsedTo);
            if(date_must_be_used_to == null)
            {
                System.Diagnostics.Debug.WriteLine("-------------- ExtendFacade date_must_be_used_to IS NULL");
                return false;
            }

            int secondsThreadMustLaunchExtend = 0;


            if (extend.date_of_thread_must_be_finished == null)
            {
                if (((DateTimeOffset)date_must_be_used_to).ToUnixTimeSeconds() - ((DateTimeOffset)DateTime.Now).ToUnixTimeSeconds() < 0)
                {
                    secondsThreadMustLaunchExtend = 10;
                }
                else
                {
                    secondsThreadMustLaunchExtend = (user.isTestProlongation == 1
                        ? 1 * 60
                        : (int)(((DateTimeOffset)date_must_be_used_to).ToUnixTimeSeconds() - ((DateTimeOffset)DateTime.Now).ToUnixTimeSeconds())
                    );
                }
                //int secondsThreadMustLaunchExtend = 3000;

                ExtendService extendService = new ExtendService(_dbc);
                if (!await extendService.updateStartThread(
                    extend,
                    DateTime.Now,
                    DateTime.Now.AddSeconds(secondsThreadMustLaunchExtend))
                )
                {
                    System.Diagnostics.Debug.WriteLine("--------------- ExtendFacade checkAndLaunchIfNot updateStartThread");
                    return false;
                }
            } else
            {
                if (((DateTimeOffset)extend.date_of_thread_must_be_finished).ToUnixTimeSeconds() - ((DateTimeOffset)DateTime.Now).ToUnixTimeSeconds() < 0)
                {
                    secondsThreadMustLaunchExtend = 10;
                }
                else
                {
                    secondsThreadMustLaunchExtend = (int)(((DateTimeOffset)extend.date_of_thread_must_be_finished).ToUnixTimeSeconds() - ((DateTimeOffset)DateTime.Now).ToUnixTimeSeconds());
                }
            }

            string th_domainHost = domainHost;
            Extend th_extend = extend;
            PurchaseSubscription th_purchaseSubscription = purchaseSubscription;

            System.Diagnostics.Debug.WriteLine("secondsThreadMustLaunchExtend: " + secondsThreadMustLaunchExtend);

            Thread extendThread = new Thread(() => {
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    try
                    {
                        //var dbc = scope.ServiceProvider.GetService<ApplicationDbContext>();

                        System.Timers.Timer aTimer = new System.Timers.Timer(secondsThreadMustLaunchExtend * 1000);
                        aTimer.AutoReset = false;
                        aTimer.Elapsed += async (sender, e) => await launchThreadCallPostExtend(sender, e, th_domainHost, th_extend.id, th_purchaseSubscription.id);
                        aTimer.Start();

                        //await thProcess(dbc, th_domainHost, th_extend, th_purchaseSubscription, secondsThreadMustLaunchExtend);
                        System.Diagnostics.Debug.WriteLine("--------------- ExtendFacade extendThread FINISH");

                    } catch(Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine("addLaunchExtendThread Error: " + ex.ToString());
                    }
                    scope.Dispose();
                }

            });
            extendThread.Start();


            return true;
        }

        private async Task launchThreadCallPostExtend(object sender, ElapsedEventArgs e, string domainHost, int id_of_extend, int id_of_purchase_subscription)
        {
            ExtendOfPurchaseSubscription extendOfPurchaseSubscription = new ExtendOfPurchaseSubscription();
            extendOfPurchaseSubscription.id_of_extend = id_of_extend;
            extendOfPurchaseSubscription.id_of_purchase_subscription = id_of_purchase_subscription;

            var stringPayload = await Task.Run(() => JsonConvert.SerializeObject(extendOfPurchaseSubscription));
            var httpContent = new StringContent(stringPayload, Encoding.UTF8, "application/json");

            using (var httpClient = new HttpClient())
            {
                System.Diagnostics.Debug.WriteLine("https://" + domainHost + "/api/purchase_subscription/extend");
                var httpResponse = await httpClient.PostAsync("https://" + domainHost + "/api/purchase_subscription/extend", httpContent);

                if (!httpResponse.IsSuccessStatusCode) return;
                if (httpResponse.Content == null) return;

                string responseAsString = await httpResponse.Content.ReadAsStringAsync();
                System.Diagnostics.Debug.WriteLine("Ответ от launchThreadCallPostExtend: " + responseAsString);
                httpClient.Dispose();
            }
        }







        public async Task<bool> thProcess(ApplicationDbContext dbc, string domainHost, Extend extend, PurchaseSubscription purchaseSubscription)
        {
            System.Diagnostics.Debug.WriteLine("--------------- ExtendFacade thProcess launch ");

            try
            {
                PurchaseSubscriptionFacade purchaseSubscriptionFacade = new PurchaseSubscriptionFacade(dbc);

                bool isSuccessfullExtend = await purchaseSubscriptionFacade.initExtend(domainHost, purchaseSubscription.id);

                //проверяем, не отменили ли продление
                ExtendService extendService = new ExtendService(dbc);
                extend = await extendService.findByIdNotFinished(extend.id);
                if (extend == null)
                {
                    System.Diagnostics.Debug.WriteLine("--------------- ExtendFacade thProcess: extend не найден");
                    return false;
                }
                await extendService.updateFinishThread(extend, DateTime.Now);

                if (isSuccessfullExtend)
                {
                    await extendService.updateResultSuccessfull(extend);
                    await tryRelaunchThroughPostRequestToSelf(domainHost, purchaseSubscription.id);
                }

                //запустить еще раз thread продление
                //await purchaseSubscriptionFacade.relaunchExtendThread(domainHost, purchaseSubscription.user, purchaseSubscription);

            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("--------------- ExtendFacade Exception: " + ex.Message);
                return false;
            }

            return true;
        }

        public async Task<bool> canselExtendsByUser(User user)
        {
            ExtendService extendService = new ExtendService(_dbc);
            return await extendService.canselExtendsByUser(user);
        }

        private async Task tryRelaunchThroughPostRequestToSelf(string domainHost, int id_of_purchaseSubscription)
        {
            System.Diagnostics.Debug.WriteLine("tryRelaunchThroughPostRequestToSelf id_of_purchaseSubscription: " + id_of_purchaseSubscription);
            PurchaseSubscriptionIdDTO purchaseSubscriptionIdDTO = new PurchaseSubscriptionIdDTO();
            purchaseSubscriptionIdDTO.id = id_of_purchaseSubscription;

            var stringPayload = await Task.Run(() => JsonConvert.SerializeObject(purchaseSubscriptionIdDTO));
            var httpContent = new StringContent(stringPayload, Encoding.UTF8, "application/json");

            using (var httpClient = new HttpClient())
            {

                System.Diagnostics.Debug.WriteLine("https://" + domainHost + "/api/purchase_subscription/relaunch_extend");
                var httpResponse = await httpClient.PostAsync("https://" + domainHost + "/api/purchase_subscription/relaunch_extend", httpContent);

                if (!httpResponse.IsSuccessStatusCode) return;
                if (httpResponse.Content == null) return;

                string responseAsString = await httpResponse.Content.ReadAsStringAsync();
                System.Diagnostics.Debug.WriteLine("Ответ от tryRelaunchThroughPostRequestToSelf: " + responseAsString);
                httpClient.Dispose();
            }
        }
    }
}
