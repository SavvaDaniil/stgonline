using STG.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace STG.Interface
{
    interface IPurchaseLessonService
    {
        PurchaseLesson add();
        PurchaseLesson save(PurchaseLesson purchaseLesson);
        bool delete();
        List<PurchaseLesson> listAllActiveOfUser(User user);
        List<PurchaseLesson> listAllOfUser(User user);
    }

}
