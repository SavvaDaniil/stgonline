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

        public PurchaseSubscription findById(int id)
        {
            return _dbc.PurchaseSubscriptions
                .Include(p => p.subscription)
                .Include(p => p.payment)
                .Include(p => p.user)
                .Where(p => p.id == id)
                .FirstOrDefault();
        }

        public bool isAnyBuyedBeforeByUser(User user)
        {
            return _dbc.PurchaseSubscriptions
                .Where(p => p.user == user && p.active == 1)
                .OrderBy(p => p.id)
                .Any();
        }

        public List<PurchaseSubscription> listAllByUser(User user)
        {
            return _dbc.PurchaseSubscriptions
                .Include(p => p.subscription)
                .Include(p => p.payment)
                .Where(p => p.user == user && p.active == 1).ToList();
        }

        public List<PurchaseSubscription> listAllAnyByUser(User user)
        {
            return _dbc.PurchaseSubscriptions
                .Include(p => p.subscription)
                .Include(p => p.payment)
                .Where(p => p.user == user)
                .OrderByDescending(p => p.id)
                .ToList();
        }

        public List<PurchaseSubscription> first2Active(User user)
        {
            return _dbc.PurchaseSubscriptions
                .Where(p => p.user == user && p.active == 1 && (p.dateOfActivation == null || p.dateOfMustBeUsedTo > DateTime.Now.Date))
                .Include(p => p.user)
                .Include(p => p.subscription)
                .Include(p => p.payment)
                .OrderBy(p => p.id)
                .Take(2)
                .ToList();
        }

        public bool isAnyActive(User user)
        {
            return _dbc.PurchaseSubscriptions
                .Where(p => p.user == user && p.active == 1 && (p.dateOfActivation == null || p.dateOfMustBeUsedTo > DateTime.Now.Date))
                .OrderBy(p => p.id)
                .Any();
        }

        public PurchaseSubscription add(Payment payment)
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

            _dbc.PurchaseSubscriptions.AddAsync(purchaseSubscription);
            _dbc.SaveChanges();
            purchaseSubscription.orderInList = purchaseSubscription.id;
            _dbc.SaveChanges();

            return purchaseSubscription;
        }

        public void activate(PurchaseSubscription purchaseSubscription)
        {
            purchaseSubscription.dateOfActivation = DateTime.Now.Date;
            if (purchaseSubscription.days != 0)
            {
                purchaseSubscription.dateOfMustBeUsedTo = DateTime.Now.Date.AddDays(purchaseSubscription.days);
            }
            _dbc.SaveChanges();
        }


        public bool canselProlongation(PurchaseSubscription purchaseSubscription)
        {
            purchaseSubscription.isProlongation = 0;
            _dbc.SaveChanges();
            return true;
        }

        public bool extendForDaysSelf(PurchaseSubscription purchaseSubscription)
        {
            if (purchaseSubscription.days == 0 || purchaseSubscription.dateOfActivation == null) return false;

            DateTime date_must_be_used = (DateTime)purchaseSubscription.dateOfMustBeUsedTo;

            purchaseSubscription.dateOfMustBeUsedTo = date_must_be_used.AddDays(purchaseSubscription.days);

            _dbc.SaveChanges();
            return true;
        }

        public bool setCanselByPayment(Payment payment)
        {
            List<PurchaseSubscription> purchaseSubscriptions = _dbc.PurchaseSubscriptions
                .Where(p => p.payment == payment)
                .ToList();
            foreach (PurchaseSubscription purchase in purchaseSubscriptions)
            {
                purchase.active = 0;
                purchase.dateOfActivation = null;
                purchase.dateOfMustBeUsedTo = null;
                _dbc.SaveChanges();
            }
            return true;
        }


        public bool edit(PurchaseSubscription purchaseSubscription, PurchaseSubscriptionDTO purchaseSubscriptionDTO, Subscription subscription)
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

            _dbc.SaveChanges();

            return true;
        }


        public bool addByAdmin(PurchaseSubscriptionNewByAdminDTO purchaseSubscriptionNewByAdminDTO, User user, Subscription subscription)
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

            _dbc.PurchaseSubscriptions.AddAsync(purchaseSubscription);
            _dbc.SaveChanges();
            purchaseSubscription.orderInList = purchaseSubscription.id;
            _dbc.SaveChanges();
            return true;
        }


        public bool delete(PurchaseSubscription purchaseSubscription)
        {
            _dbc.PurchaseSubscriptions.Remove(purchaseSubscription);
            _dbc.SaveChanges();
            return true;
        }

        public List<PurchaseSubscription> listAllPayedByDates(DateTime dateFrom, DateTime dateTo)
        {
            return _dbc.PurchaseSubscriptions
                .Where(p => p.isPayed == 1 && p.active == 1 && p.dateOfAdd >= dateFrom && p.dateOfAdd <= dateTo)
                .OrderByDescending(p => p.id)
                .ToList();
        }
    }
}
