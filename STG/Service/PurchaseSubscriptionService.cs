using Microsoft.EntityFrameworkCore;
using STG.Data;
using STG.DTO.PurchaseSubscription;
using STG.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace STG.Service
{
    public class PurchaseSubscriptionService
    {
        private ApplicationDbContext _dbc;
        public PurchaseSubscriptionService(ApplicationDbContext dbc)
        {
            this._dbc = dbc;
        }

        public async Task<PurchaseSubscription> findById(int id)
        {
            return await _dbc.PurchaseSubscriptions
                .Include(p => p.subscription)
                .Include(p => p.payment)
                .Include(p => p.user)
                .Where(p => p.id == id)
                .FirstOrDefaultAsync();
        }

        public async Task<bool> isAnyBuyedBeforeByUser(User user)
        {
            return await _dbc.PurchaseSubscriptions
                .Where(p => p.user == user && p.active == 1)
                .OrderBy(p => p.id)
                .AnyAsync();
        }

        public async Task<List<PurchaseSubscription>> listAllByUser(User user)
        {
            return await _dbc.PurchaseSubscriptions
                .Include(p => p.subscription)
                .Include(p => p.payment)
                .Where(p => p.user == user && p.active == 1).ToListAsync();
        }

        public async Task<List<PurchaseSubscription>> listAllAnyByUser(User user)
        {
            return await _dbc.PurchaseSubscriptions
                .Include(p => p.subscription)
                .Include(p => p.payment)
                .Where(p => p.user == user)
                .OrderByDescending(p => p.id)
                .ToListAsync();
        }

        public async Task<List<PurchaseSubscription>> first2Active(User user)
        {
            return await _dbc.PurchaseSubscriptions
                .Where(p => p.user == user && p.active == 1 && (p.dateOfActivation == null || p.dateOfMustBeUsedTo > DateTime.Now.Date))
                .Include(p => p.user)
                .Include(p => p.subscription)
                .Include(p => p.payment)
                .OrderBy(p => p.id)
                .Take(2)
                .ToListAsync();
        }

        public async Task<bool> isAnyActive(User user)
        {
            return await _dbc.PurchaseSubscriptions
                .Where(p => p.user == user && p.active == 1 && (p.dateOfActivation == null || p.dateOfMustBeUsedTo > DateTime.Now.Date))
                .OrderBy(p => p.id)
                .AnyAsync();
        }

        public async Task<PurchaseSubscription> add(Payment payment)
        {
            PurchaseSubscription purchaseSubscription = new PurchaseSubscription();
            purchaseSubscription.user = payment.user;
            purchaseSubscription.subscription = payment.subscription;
            purchaseSubscription.payment = payment;
            purchaseSubscription.dateOfAdd = DateTime.Now;
            purchaseSubscription.isPayed = 1;
            purchaseSubscription.active = 1;
            purchaseSubscription.isProlongation = payment.isProlongation;
            purchaseSubscription.days = payment.subscription.days;

            await _dbc.PurchaseSubscriptions.AddAsync(purchaseSubscription);
            await _dbc.SaveChangesAsync();
            purchaseSubscription.orderInList = purchaseSubscription.id;
            await _dbc.SaveChangesAsync();

            return purchaseSubscription;
        }

        public async Task activate(PurchaseSubscription purchaseSubscription)
        {
            purchaseSubscription.dateOfActivation = DateTime.Now.Date;
            if (purchaseSubscription.days != 0)
            {
                purchaseSubscription.dateOfMustBeUsedTo = DateTime.Now.Date.AddDays(purchaseSubscription.days);
            }
            await _dbc.SaveChangesAsync();
        }


        public async Task<bool> canselProlongation(PurchaseSubscription purchaseSubscription)
        {
            purchaseSubscription.isProlongation = 0;
            await _dbc.SaveChangesAsync();
            return true;
        }

        public async Task<bool> extendForDaysSelf(PurchaseSubscription purchaseSubscription)
        {
            if (purchaseSubscription.days == 0 || purchaseSubscription.dateOfActivation == null) return false;

            DateTime date_must_be_used = (DateTime)purchaseSubscription.dateOfMustBeUsedTo;

            purchaseSubscription.dateOfMustBeUsedTo = date_must_be_used.AddDays(purchaseSubscription.days);

            await _dbc.SaveChangesAsync();
            return true;
        }

        public async Task<bool> setCanselByPayment(Payment payment)
        {
            List<PurchaseSubscription> purchaseSubscriptions = await _dbc.PurchaseSubscriptions
                .Where(p => p.payment == payment)
                .ToListAsync();
            foreach (PurchaseSubscription purchase in purchaseSubscriptions)
            {
                purchase.active = 0;
                purchase.dateOfActivation = null;
                purchase.dateOfMustBeUsedTo = null;
                await _dbc.SaveChangesAsync();
            }
            return true;
        }


        public async Task<bool> edit(PurchaseSubscription purchaseSubscription, PurchaseSubscriptionDTO purchaseSubscriptionDTO, Subscription subscription)
        {
            if (purchaseSubscriptionDTO.days < 0) purchaseSubscriptionDTO.days = 1;

            purchaseSubscription.subscription = subscription;
            purchaseSubscription.active = purchaseSubscriptionDTO.active;
            purchaseSubscription.days = purchaseSubscriptionDTO.days;

            if (purchaseSubscriptionDTO.date_buy_day != 0 && purchaseSubscriptionDTO.date_buy_month != 0 && purchaseSubscriptionDTO.date_buy_year != 0)
            {
                purchaseSubscription.dateOfAdd = DateTime.Parse(purchaseSubscriptionDTO.date_buy_year + "-" + purchaseSubscriptionDTO.date_buy_month + "-" + purchaseSubscriptionDTO.date_buy_day);
            } else
            {
                purchaseSubscription.dateOfAdd = null;
            }

            if (purchaseSubscriptionDTO.date_active_day != 0 && purchaseSubscriptionDTO.date_active_month != 0 && purchaseSubscriptionDTO.date_active_year != 0)
            {
                purchaseSubscription.dateOfActivation = DateTime.Parse(purchaseSubscriptionDTO.date_active_year + "-" + purchaseSubscriptionDTO.date_active_month + "-" + purchaseSubscriptionDTO.date_active_day);
            }
            else
            {
                purchaseSubscription.dateOfActivation = null;
            }

            if (purchaseSubscriptionDTO.date_must_be_used_to_day != 0 && purchaseSubscriptionDTO.date_must_be_used_to_month != 0 && purchaseSubscriptionDTO.date_must_be_used_to_year != 0)
            {
                purchaseSubscription.dateOfMustBeUsedTo = DateTime.Parse(
                    purchaseSubscriptionDTO.date_must_be_used_to_year + "-"
                    + purchaseSubscriptionDTO.date_must_be_used_to_month + "-"
                    + purchaseSubscriptionDTO.date_must_be_used_to_day);
            }
            else
            {
                purchaseSubscription.dateOfMustBeUsedTo = null;
            }

            await _dbc.SaveChangesAsync();

            return true;
        }


        public async Task<bool> addByAdmin(PurchaseSubscriptionNewByAdminDTO purchaseSubscriptionNewByAdminDTO, User user, Subscription subscription)
        {
            PurchaseSubscription purchaseSubscription = new PurchaseSubscription();
            if (purchaseSubscriptionNewByAdminDTO.days < 0) purchaseSubscriptionNewByAdminDTO.days = 1;
            purchaseSubscription.user = user;
            purchaseSubscription.subscription = subscription;
            purchaseSubscription.active = purchaseSubscriptionNewByAdminDTO.active;
            purchaseSubscription.days = purchaseSubscriptionNewByAdminDTO.days;


            if (purchaseSubscriptionNewByAdminDTO.date_buy_day != 0 && purchaseSubscriptionNewByAdminDTO.date_buy_month != 0 && purchaseSubscriptionNewByAdminDTO.date_buy_year != 0)
            {
                purchaseSubscription.dateOfAdd = DateTime.Parse(
                    purchaseSubscriptionNewByAdminDTO.date_buy_year + "-" 
                    + purchaseSubscriptionNewByAdminDTO.date_buy_month + "-" 
                    + purchaseSubscriptionNewByAdminDTO.date_buy_day
                );
            }
            else
            {
                purchaseSubscription.dateOfAdd = null;
            }

            if (purchaseSubscriptionNewByAdminDTO.date_active_day != 0 && purchaseSubscriptionNewByAdminDTO.date_active_month != 0 && purchaseSubscriptionNewByAdminDTO.date_active_year != 0)
            {
                purchaseSubscription.dateOfActivation = DateTime.Parse(
                    purchaseSubscriptionNewByAdminDTO.date_active_year + "-" 
                    + purchaseSubscriptionNewByAdminDTO.date_active_month + "-" 
                    + purchaseSubscriptionNewByAdminDTO.date_active_day
                );
            }
            else
            {
                purchaseSubscription.dateOfActivation = null;
            }

            if (purchaseSubscriptionNewByAdminDTO.date_must_be_used_to_day != 0 && purchaseSubscriptionNewByAdminDTO.date_must_be_used_to_month != 0 && purchaseSubscriptionNewByAdminDTO.date_must_be_used_to_year != 0)
            {
                purchaseSubscription.dateOfMustBeUsedTo = DateTime.Parse(
                    purchaseSubscriptionNewByAdminDTO.date_must_be_used_to_year + "-"
                    + purchaseSubscriptionNewByAdminDTO.date_must_be_used_to_month + "-"
                    + purchaseSubscriptionNewByAdminDTO.date_must_be_used_to_day);
            }
            else
            {
                purchaseSubscription.dateOfMustBeUsedTo = null;
            }

            await _dbc.PurchaseSubscriptions.AddAsync(purchaseSubscription);
            await _dbc.SaveChangesAsync();
            purchaseSubscription.orderInList = purchaseSubscription.id;
            await _dbc.SaveChangesAsync();
            return true;
        }


        public async Task<bool> delete(PurchaseSubscription purchaseSubscription)
        {
            _dbc.PurchaseSubscriptions.Remove(purchaseSubscription);
            await _dbc.SaveChangesAsync();
            return true;
        }
    }
}
