using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using STG.Data;
using STG.DTO.Payment;
using STG.Factory;
using STG.Interface.Facade;
using STG.Entities;
using STG.Service;
using STG.DTO.Extend;
using STG.ViewModels.Json;
using STG.ViewModels.PurchaseSubscription;
using STG.DTO.PurchaseSubscription;

namespace STG.Facade
{
    public class PurchaseSubscriptionFacade
    {
        private ApplicationDbContext _dbc;
        private IServiceScopeFactory _serviceScopeFactory;
        public PurchaseSubscriptionFacade(ApplicationDbContext dbc)
        {
            this._dbc = dbc;
        }
        public PurchaseSubscriptionFacade(ApplicationDbContext dbc, IServiceScopeFactory serviceScopeFactory)
        {
            this._dbc = dbc;
            _serviceScopeFactory = serviceScopeFactory;
        }

        public async Task<bool> isAnyActive(User user)
        {
            PurchaseSubscriptionService purchaseSubscriptionService = new PurchaseSubscriptionService(_dbc);
            return await purchaseSubscriptionService.isAnyActive(user);
        }

        public async Task<PurchaseSubscription> getFirstActiveAndActivateIfNull(HttpContext httpContext, string domainHost)
        {
            PurchaseSubscriptionService purchaseSubscriptionService = new PurchaseSubscriptionService(_dbc);

            UserFacade userFacade = new UserFacade(_dbc);
            User user = await userFacade.getCurrentOrNull(httpContext);
            if (user == null)
            {
                UserService userService = new UserService(_dbc);
                user = await userService.findById(7);
                if (user == null) return null;
            }

            List<PurchaseSubscription> purchaseSubscriptionListFirst2 = await purchaseSubscriptionService.first2Active(user);
            if (purchaseSubscriptionListFirst2.Count() == 0) return null;

            if (purchaseSubscriptionListFirst2[0].dateOfActivation == null)
            {
                await purchaseSubscriptionService.activate(purchaseSubscriptionListFirst2[0]);

                //обязательно проверить перед этим, а есть ли еще подписки, если есть, то не ставим автопродление
                if (purchaseSubscriptionListFirst2.Count() == 1)
                {
                    await checkAndLaunchExtendThread(domainHost, user, purchaseSubscriptionListFirst2[0]);
                }
            }

            return purchaseSubscriptionListFirst2[0];
        }

        public async Task<bool> relaunchExtendThread(string domainHost, int id_of_purchaseSubscription)
        {
            PurchaseSubscriptionService purchaseSubscriptionService = new PurchaseSubscriptionService(_dbc);
            PurchaseSubscription purchaseSubscription = await purchaseSubscriptionService.findById(id_of_purchaseSubscription);
            if (purchaseSubscription == null)
            {
                System.Diagnostics.Debug.WriteLine("relaunchExtendThread purchaseSubscription не найден, id_of_purchaseSubscription: " + id_of_purchaseSubscription);
                return false;
            }
            return await checkAndLaunchExtendThread(domainHost, purchaseSubscription.user, purchaseSubscription);
        }

        private async Task<bool> checkAndLaunchExtendThread(string domainHost, User user, PurchaseSubscription purchaseSubscription)
        {
            System.Diagnostics.Debug.WriteLine("checkAndLaunchExtendThread");
            if (user.prolongation == 0 || purchaseSubscription.isProlongation == 0 || purchaseSubscription.payment.isProlongation == 0)
            {
                System.Diagnostics.Debug.WriteLine("checkAndLaunchExtendThread ------------ СБРОШЕНО из-за запрета");
                return false;
            }
            ApplicationDbContext dbc = _dbc;
            ExtendFacade extendFacade = new ExtendFacade(dbc, _serviceScopeFactory);
            return await extendFacade.checkAndLaunchIfNot(domainHost, user, purchaseSubscription);
        }


        public async Task<PurchaseSubscription> buyAfterSuccessfullPayment(Payment payment)
        {
            //отключение продления предыдущих подписок!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            ExtendFacade extendFacade = new ExtendFacade(_dbc, _serviceScopeFactory);
            await extendFacade.canselExtendsByUser(payment.user);

            PurchaseSubscriptionFactory purchaseSubscriptionFactory = new PurchaseSubscriptionFactory(_dbc);
            return await purchaseSubscriptionFactory.create(payment);
        }



        public async Task<bool> initExtend(string domainHost, int id_of_purchaseSubscription)
        {
            System.Diagnostics.Debug.WriteLine("----------- Вызов автопродления");

            PurchaseSubscriptionService purchaseSubscriptionService = new PurchaseSubscriptionService(_dbc);
            PurchaseSubscription purchaseSubscription = await purchaseSubscriptionService.findById(id_of_purchaseSubscription);


            if (purchaseSubscription.isProlongation == 0) {
                System.Diagnostics.Debug.WriteLine("Подписки не найдено isProlongation");
                return false;
            }
            if (purchaseSubscription.payment.isProlongation == 0)
            {
                System.Diagnostics.Debug.WriteLine("Подписки не найдено ayment.isProlongation");
                return false;
            }
            if (purchaseSubscription.payment.isItProlongation == 1)
            {
                System.Diagnostics.Debug.WriteLine("Подписки не найдено payment.isItProlongation");
                return false;
            }
            if (purchaseSubscription.payment.tinkoffCanselRecurrent == 1)
            {
                System.Diagnostics.Debug.WriteLine("Подписки не найдено payment.tinkoffCanselRecurrent");
                return false;
            }
            if (purchaseSubscription.payment.tinkoffRebillID == null)
            {
                System.Diagnostics.Debug.WriteLine("Подписки не найдено payment.tinkoffRebillID = null");
                return false;
            }
            if (purchaseSubscription.payment.tinkoffRebillID == "")
            {
                System.Diagnostics.Debug.WriteLine("Подписки не найдено payment.tinkoffRebillID = ''");
                return false;
            }
            if (purchaseSubscription.payment.tinkoffCanselRecurrent == 1)
            {
                System.Diagnostics.Debug.WriteLine("Подписки не найдено payment.tinkoffCanselRecurrent");
                return false;
            }
            if (purchaseSubscription.dateOfMustBeUsedTo == null)
            {
                System.Diagnostics.Debug.WriteLine("Подписки не найдено dateOfMustBeUsedTo");
                return false;
            }
            if (purchaseSubscription.user.prolongation == 0)
            {
                System.Diagnostics.Debug.WriteLine("Подписки не найдено user.prolongation");
                return false;
            }


            PaymentFacade paymentFacade = new PaymentFacade(_dbc);

            TinkoffInitResponse tinkoffInitResponse = await paymentFacade.initExtendForSubscription(
                (purchaseSubscription.payment.isTest == 1 ? true : false),
                domainHost,
                purchaseSubscription.payment,
                purchaseSubscription.subscription
            );

            if (tinkoffInitResponse == null)
            {
                System.Diagnostics.Debug.WriteLine("Ошибка при транзакции tinkoffInitResponse == null");
                return false;
            }

            if (!tinkoffInitResponse.Success)
            {
                System.Diagnostics.Debug.WriteLine("Ошибка при транзакции");
                await canselProlongationBecauseError(purchaseSubscription);
                return false;
            }

            if (!await purchaseSubscriptionService.extendForDaysSelf(purchaseSubscription))
            {
                System.Diagnostics.Debug.WriteLine("Ошибка при попытке продлить");
                return false;
            }

            //запустить еще раз thread продление
            //await checkAndLaunchExtendThread(domainHost, purchaseSubscription.user, purchaseSubscription);

            return true;
        }

        private async Task<bool> canselProlongationBecauseError(PurchaseSubscription purchaseSubscription)
        {
            PurchaseSubscriptionService purchaseSubscriptionService = new PurchaseSubscriptionService(_dbc);
            return await purchaseSubscriptionService.canselProlongation(purchaseSubscription);
        }




        public async Task<bool> lauchExtendFromApiExtend(string domainHost, ExtendOfPurchaseSubscriptionDTO extendOfPurchaseSubscriptionDTO)
        {
            System.Diagnostics.Debug.WriteLine("lauchExtendFromApiExtend");
            PurchaseSubscriptionService purchaseSubscriptionService = new PurchaseSubscriptionService(_dbc);
            PurchaseSubscription purchaseSubscription = await purchaseSubscriptionService.findById(extendOfPurchaseSubscriptionDTO.id_of_purchase_subscription);
            if (purchaseSubscription == null) return false;

            ExtendService extendService = new ExtendService(_dbc);
            Extend extend = await extendService.findById(extendOfPurchaseSubscriptionDTO.id_of_extend);
            if (purchaseSubscription == null) return false;

            ExtendFacade extendFacade = new ExtendFacade(_dbc, _serviceScopeFactory);
            return await extendFacade.thProcess(_dbc, domainHost, extend, purchaseSubscription);
        }


        public async Task<JsonAdminAnswer> listAllUserPurchasesSubscription(int id_of_user)
        {
            UserService userService = new UserService(_dbc);
            User user = await userService.findById(id_of_user);
            if (user == null) return new JsonAdminAnswer("error", "not_found_user");

            SubscriptionService subscriptionService = new SubscriptionService(_dbc);
            List<Subscription> subscriptionsDefault = await subscriptionService.listAll();

            PurchaseSubscriptionService purchaseSubscriptionService = new PurchaseSubscriptionService(_dbc);
            List<PurchaseSubscription> purchaseSubscriptions = await purchaseSubscriptionService.listAllAnyByUser(user);
            List<PurchaseSubscriptionViewModel> purchaseSubscriptionViewModels = new List<PurchaseSubscriptionViewModel>();

            foreach (PurchaseSubscription purchaseSubscription in purchaseSubscriptions)
            {
                string nameOfSubscription = (purchaseSubscription.subscription != null ? purchaseSubscription.subscription.name : null);

                purchaseSubscriptionViewModels.Add(
                    new PurchaseSubscriptionViewModel(
                        purchaseSubscription.id,
                        (purchaseSubscription.subscription != null ? purchaseSubscription.subscription.id : 0),
                        nameOfSubscription,
                        purchaseSubscription.active,
                        purchaseSubscription.days,
                        purchaseSubscription.isPayed,
                        (purchaseSubscription.dateOfAdd != null ? ((DateTime)purchaseSubscription.dateOfAdd.Value).Date.ToString("yyyy-MM-dd") : null),
                        (purchaseSubscription.dateOfActivation != null ? ((DateTime)purchaseSubscription.dateOfActivation.Value).Date.ToString("yyyy-MM-dd") : null),
                        (purchaseSubscription.dateOfMustBeUsedTo != null ? ((DateTime)purchaseSubscription.dateOfMustBeUsedTo.Value).Date.ToString("yyyy-MM-dd") : null)
                    )    
                );
            }

            return new JsonAdminAnswer("success", null, purchaseSubscriptionViewModels);
        }


        public async Task<JsonAdminAnswer> getOfUser(int id_of_purchase_subscription)
        {
            PurchaseSubscriptionService purchaseSubscriptionService = new PurchaseSubscriptionService(_dbc);
            PurchaseSubscription purchaseSubscription = await purchaseSubscriptionService.findById(id_of_purchase_subscription);
            if (purchaseSubscription == null) return new JsonAdminAnswer("error","not_found");

            SubscriptionService subscriptionService = new SubscriptionService(_dbc);
            List<Subscription> subscriptions = await subscriptionService.listAllActive();

            return new JsonAdminAnswer(
                "success",
                null,
                new PurchaseSubscriptionViewModel(
                    purchaseSubscription.id,
                    (purchaseSubscription.subscription != null ? purchaseSubscription.subscription.id : 0),
                    (purchaseSubscription.subscription != null ? purchaseSubscription.subscription.name : null),
                    purchaseSubscription.active,
                    purchaseSubscription.days,
                    purchaseSubscription.isPayed,
                    (purchaseSubscription.dateOfAdd != null ? ((DateTime)purchaseSubscription.dateOfAdd.Value).Date.ToString("yyyy-MM-dd") : null),
                    (purchaseSubscription.dateOfActivation != null ? ((DateTime)purchaseSubscription.dateOfActivation.Value).Date.ToString("yyyy-MM-dd") : null),
                    (purchaseSubscription.dateOfMustBeUsedTo != null ? ((DateTime)purchaseSubscription.dateOfMustBeUsedTo.Value).Date.ToString("yyyy-MM-dd") : null)
                ),
                subscriptions
            );
        }


        public async Task<JsonAdminAnswer> saveOfUser(PurchaseSubscriptionDTO purchaseSubscriptionDTO)
        {
            PurchaseSubscriptionService purchaseSubscriptionService = new PurchaseSubscriptionService(_dbc);
            PurchaseSubscription purchaseSubscription = await purchaseSubscriptionService.findById(purchaseSubscriptionDTO.id);
            if (purchaseSubscription == null) return new JsonAdminAnswer("error", "no_purchase_subscription");

            SubscriptionService subscriptionService = new SubscriptionService(_dbc);
            Subscription subscriptionBasic = await subscriptionService.findById(purchaseSubscriptionDTO.id_of_subscription);
            if (subscriptionBasic == null) return new JsonAdminAnswer("error", "no subscription");

            return (await purchaseSubscriptionService.edit(purchaseSubscription, purchaseSubscriptionDTO, subscriptionBasic)
                ?   new JsonAdminAnswer("success", null)
                :   new JsonAdminAnswer("error", "unknown_when_save")
            );
        }

        public async Task<JsonAdminAnswer> addByAdmin(PurchaseSubscriptionNewByAdminDTO purchaseSubscriptionNewByAdminDTO)
        {
            UserService userService = new UserService(_dbc);
            User user = await userService.findById(purchaseSubscriptionNewByAdminDTO.id_of_user);
            if (user == null) return new JsonAdminAnswer("error", "no_user");

            SubscriptionService subscriptionService = new SubscriptionService(_dbc);
            Subscription subscriptionBasic = await subscriptionService.findById(purchaseSubscriptionNewByAdminDTO.id_of_subscription);
            if (subscriptionBasic == null) return new JsonAdminAnswer("error", "no subscription");

            PurchaseSubscriptionService purchaseSubscriptionService = new PurchaseSubscriptionService(_dbc);

            return (await purchaseSubscriptionService.addByAdmin(purchaseSubscriptionNewByAdminDTO, user, subscriptionBasic)
                ? new JsonAdminAnswer("success", null)
                : new JsonAdminAnswer("error", "unknown_when_save")
            );
        }


        public async Task<JsonAdminAnswer> deleteByAdmin(int id_of_purchase_subscription)
        {
            PurchaseSubscriptionService purchaseSubscriptionService = new PurchaseSubscriptionService(_dbc);
            PurchaseSubscription purchaseSubscription = await purchaseSubscriptionService.findById(id_of_purchase_subscription);
            if (purchaseSubscription == null) return new JsonAdminAnswer("error", "no_purchase_subscription");

            return (await purchaseSubscriptionService.delete(purchaseSubscription)
                ? new JsonAdminAnswer("success", null)
                : new JsonAdminAnswer("error", "unknown_when_delete")
            );
        }

    }
}
