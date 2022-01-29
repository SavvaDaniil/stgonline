using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using STG.Entities;

namespace STG.Interface.Factory
{
    interface IPurchaseLessonFactory
    {
        PurchaseLesson createIfNotExistOrUpdate(User user, Lesson lesson, Payment payment);
    }
}
