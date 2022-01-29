using STG.Data;
using STG.Entities;
using STG.Service;
using STG.ViewModels.Json;
using STG.ViewModels.Subscription;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace STG.Facade
{
    public class SubscriptionFacade
    {
        private ApplicationDbContext _dbc;
        public SubscriptionFacade(ApplicationDbContext dbc)
        {
            this._dbc = dbc;
        }

        public async Task<List<SubscriptionLiteViewModel>> listAllActiveForAnyLesson(User user)
        {
            SubscriptionService subscriptionService = new SubscriptionService(_dbc);
            List<Subscription> subscriptions = await subscriptionService.listAllActive();
            List<SubscriptionLiteViewModel> subscriptionLiteViewModels = new List<SubscriptionLiteViewModel>();

            bool isAnyAlreadyBuyed = false;
            if(user != null)
            {
                PurchaseSubscriptionService purchaseSubscriptionService = new PurchaseSubscriptionService(_dbc);
                isAnyAlreadyBuyed = await purchaseSubscriptionService.isAnyBuyedBeforeByUser(user);
            }

            foreach (Subscription subscription in subscriptions)
            {
                subscriptionLiteViewModels.Add(
                    new SubscriptionLiteViewModel(
                        subscription.id,
                        subscription.name,
                        (!isAnyAlreadyBuyed && subscription.is_discount_for_first_time == 1 ? subscription.price_for_first_time : subscription.price),
                        subscription.days,
                        subscription.prolongation
                    )
                );
            }

            return subscriptionLiteViewModels;
        }

        public async Task<JsonAdminAnswer> listAll()
        {
            SubscriptionService subscriptionService = new SubscriptionService(_dbc);
            return new JsonAdminAnswer("success", null, await  subscriptionService.listAll());
        }
    }
}
