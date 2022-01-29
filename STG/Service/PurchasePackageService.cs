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

        public async Task<PurchasePackage> findById(int id)
        {
            return await _dbc.PurchasePackages
                .Include(p => p.package)
                .Include(p => p.payment)
                .Include(p => p.user)
                .Where(p => p.id == id).FirstOrDefaultAsync();
        }

        public async Task<List<PurchasePackage>> listAllByUser(User user)
        {
            return await _dbc.PurchasePackages
                .Include(p => p.package)
                .Include(p => p.payment)
                .Where(p => p.user == user && p.active == 1).ToListAsync();
        }



        public async Task<List<PurchasePackage>> first2Active(User user)
        {
            return await _dbc.PurchasePackages
                .Where(p => p.user == user && p.active == 1 && (p.dateOfActivation == null || p.dateOfMustBeUsedTo > DateTime.Now.Date))
                .Include(p => p.user)
                .Include(p => p.package)
                .Include(p => p.payment)
                .OrderBy(p => p.id)
                .Take(2)
                .ToListAsync();
        }

        public async Task<bool> isAnyActive(User user, Package package)
        {
            return await _dbc.PurchasePackages
                .Where(p => p.user == user && p.package == package && p.active == 1 && (p.dateOfActivation == null || p.dateOfMustBeUsedTo > DateTime.Now.Date))
                .OrderBy(p => p.id)
                .AnyAsync();
        }

        public async Task<bool> isAnyActivePurchaseForAnyUser(Package package)
        {
            return await _dbc.PurchasePackages
                .Where(p => p.package == package && p.active == 1 && (p.dateOfActivation == null || p.dateOfMustBeUsedTo > DateTime.Now.Date))
                .OrderBy(p => p.id)
                .AnyAsync();
        }

        public async Task<PurchasePackage> getFirstActive(User user)
        {
            return await _dbc.PurchasePackages
                .Where(p => p.user == user && p.active == 1 && (p.dateOfActivation == null || p.dateOfMustBeUsedTo > DateTime.Now.Date))
                .OrderBy(p => p.id)
                .FirstOrDefaultAsync();
        }

        public async Task<PurchasePackage> getFirstActive(User user, Package package)
        {
            return await _dbc.PurchasePackages
                .Where(p => p.user == user && p.package == package && p.active == 1 && (p.dateOfActivation == null || p.dateOfMustBeUsedTo > DateTime.Now.Date))
                .OrderBy(p => p.id)
                .FirstOrDefaultAsync();
        }

        public async Task<List<PurchasePackage>> listAllActive(User user)
        {
            return await _dbc.PurchasePackages
                .Include(p => p.package)
                .Include(p => p.payment)
                .Where(p => p.user == user && p.active == 1 && (p.dateOfActivation == null || p.dateOfMustBeUsedTo > DateTime.Now.Date))
                .OrderBy(p => p.id)
                .ToListAsync();
        }

        public async Task<int> countAllActive(User user)
        {
            return await _dbc.PurchasePackages
                .Include(p => p.package)
                .Include(p => p.payment)
                .Where(p => p.user == user && p.active == 1 && (p.dateOfActivation == null || p.dateOfMustBeUsedTo > DateTime.Now.Date))
                .OrderBy(p => p.id)
                .CountAsync();
        }

        public async Task<PurchasePackage> add(Payment payment)
        {
            PurchasePackage PurchasePackage = new PurchasePackage();
            PurchasePackage.user = payment.user;
            PurchasePackage.package = payment.package;
            PurchasePackage.payment = payment;
            PurchasePackage.dateOfAdd = DateTime.Now;
            PurchasePackage.isPayed = 1;
            PurchasePackage.active = 1;
            PurchasePackage.days = payment.package.days;

            await _dbc.PurchasePackages.AddAsync(PurchasePackage);
            await _dbc.SaveChangesAsync();
            PurchasePackage.orderInList = PurchasePackage.id;
            await _dbc.SaveChangesAsync();

            return PurchasePackage;
        }

        public async Task activate(PurchasePackage purchasePackage)
        {
            purchasePackage.dateOfActivation = DateTime.Now.Date;
            if (purchasePackage.days != 0)
            {
                purchasePackage.dateOfMustBeUsedTo = DateTime.Now.Date.AddDays(purchasePackage.days);
            }
            await _dbc.SaveChangesAsync();
        }

        public async Task<bool> setCanselByPayment(Payment payment)
        {
            List<PurchasePackage> purchasePackages = await _dbc.PurchasePackages
                .Where(p => p.payment == payment)
                .ToListAsync();
            foreach (PurchasePackage purchase in purchasePackages)
            {
                purchase.active = 0;
                purchase.dateOfActivation = null;
                purchase.dateOfMustBeUsedTo = null;
                await _dbc.SaveChangesAsync();
            }
            return true;
        }

    }
}
