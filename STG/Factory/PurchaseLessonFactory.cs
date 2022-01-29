using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using STG.Entities;
using STG.Interface.Factory;
using STG.Service;
using STG.Data;
using STG.DTO.PurchaseLesson;

namespace STG.Factory
{
    public class PurchaseLessonFactory
    {
        private ApplicationDbContext _dbc;
        public PurchaseLessonFactory(ApplicationDbContext dbc)
        {
            this._dbc = dbc;
        }

        public async Task<PurchaseLesson> create(PurchaseLessonNewDTO purchaseLessonNewDTO)
        {
            PurchaseLessonService purchaseLessonService = new PurchaseLessonService(_dbc);
            return await purchaseLessonService.add(purchaseLessonNewDTO.payment, purchaseLessonNewDTO.user, purchaseLessonNewDTO.lesson);
        }


        public async Task<PurchaseLesson> create(Payment payment, User user, Lesson lesson)
        {
            PurchaseLessonService purchaseLessonService = new PurchaseLessonService(_dbc);
            return await purchaseLessonService.add(payment, user, lesson);
        }
    }
}
