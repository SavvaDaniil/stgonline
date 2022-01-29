using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using STG.Data;
using STG.Factory;
using STG.Interface.Facade;
using STG.Entities;
using STG.Service;
using STG.ViewModels.PurchaseLesson;

namespace STG.Facade
{
    public class PurchaseLessonFacade : IPurchaseLessonFacade
    {
        private ApplicationDbContext _dbc;
        public PurchaseLessonFacade(ApplicationDbContext dbc)
        {
            this._dbc = dbc;
        }

        public PurchaseLesson prepareForBuy(User user, Lesson lesson, Payment payment)
        {
            throw new NotImplementedException();
        }

        public PurchaseLesson setBuyed(User user, Lesson lesson)
        {
            throw new NotImplementedException();
        }

        public async Task<PurchaseLesson> getLastActive(User user, Lesson lesson)
        {
            PurchaseLessonService purchaseLessonService = new PurchaseLessonService(_dbc);
            return await purchaseLessonService.isAlreadyExist(user, lesson);
        }

        public async Task<PurchaseLesson> getFirstActiveAndActivateIfNull(HttpContext httpContext, int id_of_lesson)
        {
            PurchaseLessonService purchaseLessonService = new PurchaseLessonService(_dbc);
            UserFacade userFacade = new UserFacade(_dbc);
            User user = await userFacade.getCurrentOrNull(httpContext);
            if (user == null) return null;

            LessonService lessonService = new LessonService(_dbc);
            Lesson lesson = await lessonService.findById(id_of_lesson);
            if (lesson == null) return null;

            PurchaseLesson purchaseLesson = await getFirstActive(user, lesson);
            if (purchaseLesson == null) return null;
            if(purchaseLesson.dateOfActivation == null)
            {
                await purchaseLessonService.activate(purchaseLesson);
            }

            return purchaseLesson;
        }

        private async Task<PurchaseLesson> getFirstActive(User user, Lesson lesson)
        {
            PurchaseLessonService purchaseLessonService = new PurchaseLessonService(_dbc);
            return await purchaseLessonService.firstActive(user, lesson);
        }


        public async Task<HashSet<int>> getAllAvaiableHashSetOfIdLessonByUser(User user)
        {
            PurchaseLessonService purchaseLessonService = new PurchaseLessonService(_dbc);
            HashSet<int> setIdOfAvailableLessons = new HashSet<int>();
            IEnumerable<PurchaseLesson> enumPurchaseLessons = await purchaseLessonService.enumAllActiveByUser(user);
            foreach(PurchaseLesson purchaseLesson in enumPurchaseLessons)
            {
                setIdOfAvailableLessons.Add(purchaseLesson.lesson.id);
            }
            purchaseLessonService = null;
            enumPurchaseLessons = null;

            return setIdOfAvailableLessons;
        }


        public async Task<PurchaseLesson> buyAfterSuccessfullPayment(Payment payment, User user, Lesson lesson)
        {
            PurchaseLessonFactory purchaseLessonFactory = new PurchaseLessonFactory(_dbc);
            return await purchaseLessonFactory.create(payment, user, lesson);
        }
    }
}
