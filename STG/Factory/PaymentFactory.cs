using STG.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using STG.Interface.Factory;
using STG.Service;
using STG.Data;
using STG.DTO.Payment;

namespace STG.Factory
{
    public class PaymentFactory
    {
        private ApplicationDbContext _dbc;
        public PaymentFactory(ApplicationDbContext dbc)
        {
            this._dbc = dbc;
        }

        public async Task<Payment> createSingleForLesson(User user, Lesson lesson, int is_prolongation)
        {
            return await create(user, lesson, null, null, is_prolongation);
        }

        public async Task<Payment> createForSubscription(User user, Subscription subscription, int is_prolongation, bool isAnyBuyedBefore = false)
        {
            return await create(user, null, subscription, null, is_prolongation, isAnyBuyedBefore);
        }
        public async Task<Payment> createForSubscriptionExtend(User user, Subscription subscription, PaymentNewDTO paymentNewDTO)
        {
            return await createForExtend(user, subscription, paymentNewDTO);
        }

        public async Task<Payment> createSingleForPackage(User user, Package package)
        {
            return await create(user, null, null, package, 0);
        }


        private async Task<Payment> create(User user, Lesson lesson, Subscription subscription, Package package, int is_prolongation, bool isAnyBuyedBefore = false)
        {
            PaymentService paymentService = new PaymentService(_dbc);
            return await paymentService.add(user, lesson, subscription, package, is_prolongation, isAnyBuyedBefore);
        }

        private async Task<Payment> createForExtend(User user, Subscription subscription, PaymentNewDTO paymentNewDTO)
        {
            PaymentService paymentService = new PaymentService(_dbc);
            return await paymentService.addForExtended(user, subscription, paymentNewDTO);
        }
    }
}
