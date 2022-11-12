using Microsoft.EntityFrameworkCore;
using STG.Data;
using STG.DTO.PurchaseLesson;
using STG.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace STG.Service
{
    public class PurchaseLessonService
    {
        private ApplicationDbContext _dbc;

        public PurchaseLessonService(ApplicationDbContext dbc)
        {
            this._dbc = dbc;
        }

        public List<PurchaseLesson> listAllActive()
        {
            return _dbc.PurchaseLessons.Where(p => p.active == 1).ToList();
        }

        public List<PurchaseLesson> listAllActiveByUser(User user)
        {
            return _dbc.PurchaseLessons.Where(p => p.user == user && p.active == 1 && (p.dateOfActivation == null || p.dateOfMustBeUsedTo > DateTime.Now.Date)).ToList();
        }
        public IEnumerable<PurchaseLesson> enumAllActiveByUser(User user)
        {
            return _dbc.PurchaseLessons
                .Include(p => p.lesson)
                .Where(p => p.user == user && p.active == 1 && (p.dateOfActivation == null || p.dateOfMustBeUsedTo > DateTime.Now.Date))
                .ToList();
        }

        public PurchaseLesson firstActive(User user, Lesson lesson)
        {
            return _dbc.PurchaseLessons
                .Where(p => p.user == user && p.lesson == lesson && p.active == 1 && (p.dateOfActivation == null || p.dateOfMustBeUsedTo > DateTime.Now.Date))
                .OrderBy(p => p.id)
                .FirstOrDefault();
        }

        public PurchaseLesson add(Payment payment, User user, Lesson lesson)
        {
            PurchaseLesson purchaseLesson = new PurchaseLesson();
            purchaseLesson.user = user;
            purchaseLesson.lesson = lesson;
            purchaseLesson.payment = payment;
            purchaseLesson.dateOfAdd = DateTime.Now;
            purchaseLesson.isPayed = 1;
            purchaseLesson.active = 1;
            purchaseLesson.days = lesson.days;

            _dbc.PurchaseLessons.Add(purchaseLesson);
            _dbc.SaveChanges();
            purchaseLesson.orderInList = purchaseLesson.id;
            _dbc.SaveChanges();

            return purchaseLesson;
        }

        public PurchaseLesson isAlreadyExist(User user, Lesson lesson)
        {
            return _dbc.PurchaseLessons.Where(p => p.user == user && p.lesson == lesson && p.active == 1).OrderBy(p => p.id).FirstOrDefault();
        }

        public void activate(PurchaseLesson purchaseLesson)
        {
            purchaseLesson.dateOfActivation = DateTime.Now.Date;
            if (purchaseLesson.days != 0)
            {
                purchaseLesson.dateOfMustBeUsedTo = DateTime.Now.Date.AddDays(purchaseLesson.days);
            }
            _dbc.SaveChanges();
        }

        public bool setCanselByPayment(Payment payment)
        {
            List<PurchaseLesson> purchaseLessons = _dbc.PurchaseLessons
                .Where(p => p.payment == payment)
                .ToList();
            foreach (PurchaseLesson purchaseLesson in purchaseLessons)
            {
                purchaseLesson.active = 0;
                purchaseLesson.dateOfActivation = null;
                purchaseLesson.dateOfMustBeUsedTo = null;
                _dbc.SaveChanges();
            }
            return true;
        }

        public List<PurchaseLesson> listAllPayedByDates(DateTime dateFrom, DateTime dateTo)
        {
            return _dbc.PurchaseLessons
                .Where(p => p.isPayed == 1 && p.active == 1 && p.dateOfAdd >= dateFrom && p.dateOfAdd <= dateTo)
                .OrderByDescending(p => p.id)
                .ToList();
        }
    }
}
