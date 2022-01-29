using Microsoft.AspNetCore.Http;
using STG.Data;
using STG.Entities;
using STG.Service;
using STG.Factory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace STG.Facade
{
    public class PurchasePackageFacade
    {
        private ApplicationDbContext _dbc;
        public PurchasePackageFacade(ApplicationDbContext dbc)
        {
            this._dbc = dbc;
        }

        /*
        public async Task<bool> isAnyActive(User user)
        {
            PurchasePackageService purchasePackageService = new PurchasePackageService(_dbc);
            return await purchasePackageService.isAnyActive(user);
        }
        */

        public async Task<bool> isAnyActive(User user, bool activateIfNeed = false)
        {
            PurchasePackageService purchasePackageService = new PurchasePackageService(_dbc);
            PurchasePackage purchasePackage = await purchasePackageService.getFirstActive(user);
            if (purchasePackage == null) return false;
            if (activateIfNeed && purchasePackage.dateOfActivation == null)
            {
                await purchasePackageService.activate(purchasePackage);
            }

            return true;
        }


        public async Task<PurchasePackage> getFirstActive(User user, bool activateIfNeed = false)
        {
            PurchasePackageService purchasePackageService = new PurchasePackageService(_dbc);
            PurchasePackage purchasePackage = await purchasePackageService.getFirstActive(user);
            if (purchasePackage == null) return null;
            if (activateIfNeed && purchasePackage.dateOfActivation == null)
            {
                await purchasePackageService.activate(purchasePackage);
            }

            return purchasePackage;
        }

        public async Task<PurchasePackage> getFirstActive(User user, Package package, bool activateIfNeed = false)
        {
            PurchasePackageService purchasePackageService = new PurchasePackageService(_dbc);
            PurchasePackage purchasePackage = await purchasePackageService.getFirstActive(user, package);
            if (purchasePackage == null) return null;
            if (activateIfNeed && purchasePackage.dateOfActivation == null)
            {
                await purchasePackageService.activate(purchasePackage);
            }

            return purchasePackage;
        }

        public async Task<PurchasePackage> getFirstActiveForLesson(User user, Lesson lesson, bool activateIfNeed = false)
        {
            PackageLessonService packageLessonService = new PackageLessonService(_dbc);
            List<PackageLesson> packageLessonsOfLesson = await packageLessonService.listAllByLesson(lesson);
            if (packageLessonsOfLesson == null) return null;

            PurchasePackageService purchasePackageService = new PurchasePackageService(_dbc);
            List<PurchasePackage> purchasePackagesOfUser = await purchasePackageService.listAllActive(user);
            if (purchasePackagesOfUser == null) return null;

            PurchasePackage purchasePackageAvailableForUser = null;
            bool isAnyActive = false;
            foreach(PackageLesson packageLesson in packageLessonsOfLesson)
            {
                if (isAnyActive) break;
                foreach (PurchasePackage purchasePackage in purchasePackagesOfUser)
                {
                    if(purchasePackage.package == packageLesson.package)
                    {
                        isAnyActive = true;
                        purchasePackageAvailableForUser = purchasePackage;
                        break;
                    }
                }
            }

            //PurchasePackage purchasePackage = await purchasePackageService.getFirstActive(user, package);
            if (purchasePackageAvailableForUser == null) return null;
            if (activateIfNeed && purchasePackageAvailableForUser.dateOfActivation == null)
            {
                await purchasePackageService.activate(purchasePackageAvailableForUser);
            }

            return purchasePackageAvailableForUser;
        }





        public async Task<PurchasePackage> buyAfterSuccessfullPayment(Payment payment)
        {
            PurchasePackageFactory PurchasePackageFactory = new PurchasePackageFactory(_dbc);
            return await PurchasePackageFactory.create(payment);
        }


    }
}
