using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using STG.Entities;

namespace STG.Interface.Facade
{
    interface IPaymentFacade
    {
        Payment generate(User user, Lesson lesson, Subscription subscription);
        Payment successPay(Payment payment);
    }
}
