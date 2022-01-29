using STG.Data;
using STG.DTO.Package;
using STG.Entities;
using STG.Models;
using STG.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace STG.Facade
{
    public class PackageLessonFacade
    {
        private ApplicationDbContext _dbc;
        public PackageLessonFacade(ApplicationDbContext dbc)
        {
            this._dbc = dbc;
        }

        public async Task<bool> add(PackageLessonNewDTO packageLessonNewDTO)
        {
            PackageLessonService packageLessonService = new PackageLessonService(_dbc);

            PackageService packageService = new PackageService(_dbc);
            Package package = await packageService.findById(packageLessonNewDTO.id_of_package);
            if (package == null) return false;

            PackageDayService packageDayService = new PackageDayService(_dbc);
            PackageDay packageDay = await packageDayService.findById(packageLessonNewDTO.id_of_packageday);
            if (packageDay == null) return false;

            return await packageLessonService.add(package, packageDay);
        }

        public async Task<PackageLesson> get(int id_of_package, Lesson lesson)
        {
            PackageService packageService = new PackageService(_dbc);
            Package package = await packageService.findById(id_of_package);
            if (package == null) return null;

            PackageLessonService packageLessonService = new PackageLessonService(_dbc);
            return await packageLessonService.find(package, lesson);
        }

        public async Task<bool> update(PackageLessonDTO packageLessonDTO)
        {
            PackageLessonService packageLessonService = new PackageLessonService(_dbc);

            LessonService lessonService = new LessonService(this._dbc);
            Lesson lesson = await lessonService.findById(packageLessonDTO.id_of_lesson);

            return await packageLessonService.update(packageLessonDTO, lesson);
        }

        public async Task<bool> delete(PackageLessonIdDTO packageLessonIdDTO)
        {
            PackageLessonService packageLessonService = new PackageLessonService(_dbc);
            return await packageLessonService.delete(packageLessonIdDTO.id);
        }


        public async Task<List<int>> listIdOfAllDoneByUser(User user)
        {
            HomeworkService homeworkService = new HomeworkService(_dbc);
            List<Homework> homeworks = await homeworkService.listAllByUser(user);

            List<int> idsOfAllDoneByUser = new List<int>();

            foreach (Homework homework in homeworks)
            {
                if (homework.packageLesson == null) continue;
                if (!idsOfAllDoneByUser.Contains(homework.packageLesson.id)) idsOfAllDoneByUser.Add(homework.packageLesson.id);
            }

            return idsOfAllDoneByUser;
        }

        public async Task<List<PackageLesson>> listAllAvailableByActivePackagesOfUser(User user)
        {
            List<PackageLesson> packageLessons = new List<PackageLesson>();
            PackageLessonService packageLessonService = new PackageLessonService(_dbc);

            //получаем список всех доступных купленных пакетов
            PurchasePackageService purchasePackageService = new PurchasePackageService(_dbc);
            List<PurchasePackage> purchasePackagesofUser = await purchasePackageService.listAllActive(user);

            List<PackageLesson> packageLessonsOfPurchasePackage = new List<PackageLesson>();
            foreach (PurchasePackage purchasePackage in purchasePackagesofUser)
            {
                if (purchasePackage.package == null) continue;
                //для каждого пакета получаем список всех подключенных уроков
                packageLessonsOfPurchasePackage = await packageLessonService.listAllByPackage(purchasePackage.package);

                //вносим в основной список, если нет
                foreach (PackageLesson packageLesson in packageLessonsOfPurchasePackage)
                {
                    if (!packageLessons.Contains(packageLesson))
                    {
                        packageLessons.Add(packageLesson);
                    }
                }
                
            }

            return packageLessons;
        }

    }
}
