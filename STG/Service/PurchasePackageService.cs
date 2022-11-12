using Microsoft.EntityFrameworkCore;
using STG.Data;
using STG.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace STG.Service
{
    public class PurchasePackageService
    {
        private ApplicationDbContext _dbc;
        public PurchasePackageService(ApplicationDbContext dbc)
        {
            this._dbc = dbc;
        }

        public PurchasePackage findById(int id)
        {
            return _dbc.PurchasePackages
                .Include(p => p.package)
                .Include(p => p.payment)
                .Include(p => p.user)
                .Where(p => p.id == id).FirstOrDefault();
        }

        public List<PurchasePackage> listAllByUser(User user)
        {
            return _dbc.PurchasePackages
                .Include(p => p.package)
                .Include(p => p.payment)
                .Where(p => p.user == user && p.active == 1).ToList();
        }

        public List<PurchasePackage> listAllActiveByUser(User user)
        {
            return _dbc.PurchasePackages
                .Include(p => p.package)
                .Include(p => p.payment)
                .Where(p => p.user == user && p.active == 1 && (p.dateOfActivation == null || p.dateOfMustBeUsedTo > DateTime.Now.Date))
                .ToList();
        }



        public List<PurchasePackage> first2Active(User user)
        {
            return _dbc.PurchasePackages
                .Where(p => p.user == user && p.active == 1 && (p.dateOfActivation == null || p.dateOfMustBeUsedTo > DateTime.Now.Date))
                .Include(p => p.user)
                .Include(p => p.package)
                .Include(p => p.payment)
                .OrderBy(p => p.id)
                .Take(2)
                .ToList();
        }

        public bool isAnyActive(User user, Package package)
        {
            return _dbc.PurchasePackages
                .Where(p => p.user == user && p.package == package && p.active == 1 && (p.dateOfActivation == null || p.dateOfMustBeUsedTo > DateTime.Now.Date))
                .OrderBy(p => p.id)
                .Any();
        }

        public bool isAnyActivePurchaseForAnyUser(Package package)
        {
            return _dbc.PurchasePackages
                .Where(p => p.package == package && p.active == 1 && (p.dateOfActivation == null || p.dateOfMustBeUsedTo > DateTime.Now.Date))
                .OrderBy(p => p.id)
                .Any();
        }

        public PurchasePackage getFirstActive(User user)
        {
            return _dbc.PurchasePackages
                .Where(p => p.user == user && p.active == 1 && (p.dateOfActivation == null || p.dateOfMustBeUsedTo > DateTime.Now.Date))
                .OrderBy(p => p.id)
                .FirstOrDefault();
        }

        public PurchasePackage getFirstActive(User user, Package package)
        {
            return _dbc.PurchasePackages
                .Where(p => p.user == user && p.package == package && p.active == 1 && (p.dateOfActivation == null || p.dateOfMustBeUsedTo > DateTime.Now.Date))
                .OrderBy(p => p.id)
                .FirstOrDefault();
        }

        public List<PurchasePackage> listAllActive(User user)
        {
            return _dbc.PurchasePackages
                .Include(p => p.package)
                .Include(p => p.payment)
                .Where(p => p.user == user && p.active == 1 && (p.dateOfActivation == null || p.dateOfMustBeUsedTo > DateTime.Now.Date))
                .OrderBy(p => p.id)
                .ToList();
        }

        public List<PurchasePackage> listAllActiveWithChat(int teacherId)
        {
            return _dbc.PurchasePackages
                .Include(p => p.package)
                .Include(p => p.payment)
                .Include(p => p.user)
                .Where(p => p.package.teacher.id == teacherId && p.isWithChat == 1  && p.active == 1 && (p.dateOfActivation == null || p.dateOfMustBeUsedTo > DateTime.Now.Date))
                .OrderBy(p => p.id)
                .ToList();
        }

        public int countAllActive(User user)
        {
            return _dbc.PurchasePackages
                .Include(p => p.package)
                .Include(p => p.payment)
                .Where(p => p.user == user && p.active == 1 && (p.dateOfActivation == null || p.dateOfMustBeUsedTo > DateTime.Now.Date))
                .OrderBy(p => p.id)
                .Count();
        }

        public PurchasePackage add(Payment payment)
        {
            PurchasePackage purchasePackage = new PurchasePackage();
            purchasePackage.user = payment.user;
            purchasePackage.package = payment.package;
            purchasePackage.payment = payment;
            purchasePackage.dateOfAdd = DateTime.Now;
            purchasePackage.isPayed = 1;
            purchasePackage.active = 1;
            purchasePackage.days = payment.package.days;
            if (payment.isWithChat == 1) purchasePackage.isWithChat = 1;

            _dbc.PurchasePackages.Add(purchasePackage);
            _dbc.SaveChanges();
            purchasePackage.orderInList = purchasePackage.id;
            _dbc.SaveChanges();

            return purchasePackage;
        }

        public void activate(PurchasePackage purchasePackage)
        {
            purchasePackage.dateOfActivation = DateTime.Now.Date;
            if (purchasePackage.days != 0)
            {
                purchasePackage.dateOfMustBeUsedTo = DateTime.Now.Date.AddDays(purchasePackage.days);
            }
            _dbc.SaveChanges();
        }

        public bool setCanselByPayment(Payment payment)
        {
            List<PurchasePackage> purchasePackages = _dbc.PurchasePackages
                .Where(p => p.payment == payment)
                .ToList();
            foreach (PurchasePackage purchase in purchasePackages)
            {
                purchase.active = 0;
                purchase.dateOfActivation = null;
                purchase.dateOfMustBeUsedTo = null;
                _dbc.SaveChanges();
            }
            return true;
        }


        public List<PurchasePackage> listAllPayedByDates(DateTime dateFrom, DateTime dateTo)
        {
            return _dbc.PurchasePackages
                .Where(p => p.isPayed == 1 && p.active == 1 && p.dateOfAdd >= dateFrom && p.dateOfAdd <= dateTo)
                .OrderByDescending(p => p.id)
                .ToList();
        }

    }
}
