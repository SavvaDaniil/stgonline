using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using STG.Entities;

namespace STG.Interface.Facade
{
    interface IPurchaseLessonFacade
    {
        PurchaseLesson prepareForBuy(User user, Lesson lesson, Payment payment);
        PurchaseLesson setBuyed(User user, Lesson lesson);
    }
}
