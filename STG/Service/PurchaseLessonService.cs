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

        public async Task<List<PurchaseLesson>> listAllActive()
        {
            return await _dbc.PurchaseLessons.Where(p => p.active == 1).ToListAsync();
        }

        public async Task<List<PurchaseLesson>> listAllActiveByUser(User user)
        {
            return await _dbc.PurchaseLessons.Where(p => p.user == user && p.active == 1 && (p.dateOfActivation == null || p.dateOfMustBeUsedTo > DateTime.Now.Date)).ToListAsync();
        }
        public async Task<IEnumerable<PurchaseLesson>> enumAllActiveByUser(User user)
        {
            return await _dbc.PurchaseLessons
                .Include(p => p.lesson)
                .Where(p => p.user == user && p.active == 1 && (p.dateOfActivation == null || p.dateOfMustBeUsedTo > DateTime.Now.Date))
                .ToListAsync();
        }

        public async Task<PurchaseLesson> firstActive(User user, Lesson lesson)
        {
            return await _dbc.PurchaseLessons
                .Where(p => p.user == user && p.lesson == lesson && p.active == 1 && (p.dateOfActivation == null || p.dateOfMustBeUsedTo > DateTime.Now.Date))
                .OrderBy(p => p.id)
                .FirstOrDefaultAsync();
        }

        public async Task<PurchaseLesson> add(Payment payment, User user, Lesson lesson)
        {
            PurchaseLesson purchaseLesson = new PurchaseLesson();
            purchaseLesson.user = user;
            purchaseLesson.lesson = lesson;
            purchaseLesson.payment = payment;
            purchaseLesson.dateOfAdd = DateTime.Now;
            purchaseLesson.isPayed = 1;
            purchaseLesson.active = 1;
            purchaseLesson.days = lesson.days;

            await _dbc.PurchaseLessons.AddAsync(purchaseLesson);
            await _dbc.SaveChangesAsync();
            purchaseLesson.orderInList = purchaseLesson.id;
            await _dbc.SaveChangesAsync();

            return purchaseLesson;
        }

        public async Task<PurchaseLesson> isAlreadyExist(User user, Lesson lesson)
        {
            return await _dbc.PurchaseLessons.Where(p => p.user == user && p.lesson == lesson && p.active == 1).OrderBy(p => p.id).FirstOrDefaultAsync();
        }

        public async Task activate(PurchaseLesson purchaseLesson)
        {
            purchaseLesson.dateOfActivation = DateTime.Now.Date;
            if (purchaseLesson.days != 0)
            {
                purchaseLesson.dateOfMustBeUsedTo = DateTime.Now.Date.AddDays(purchaseLesson.days);
            }
            await _dbc.SaveChangesAsync();
        }

        public async Task<bool> setCanselByPayment(Payment payment)
        {
            List<PurchaseLesson> purchaseLessons = await _dbc.PurchaseLessons
                .Where(p => p.payment == payment)
                .ToListAsync();
            foreach (PurchaseLesson purchaseLesson in purchaseLessons)
            {
                purchaseLesson.active = 0;
                purchaseLesson.dateOfActivation = null;
                purchaseLesson.dateOfMustBeUsedTo = null;
                await _dbc.SaveChangesAsync();
            }
            return true;
        }
    }
}
