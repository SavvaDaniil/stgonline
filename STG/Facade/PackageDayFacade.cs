using STG.Data;
using STG.DTO.Package;
using STG.Entities;
using STG.Models;
using STG.Service;
using STG.ViewModels.Homework;
using STG.ViewModels.Package;
using STG.ViewModels.Video;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace STG.Facade
{
    public class PackageDayFacade
    {
        private ApplicationDbContext _dbc;
        public PackageDayFacade(ApplicationDbContext dbc)
        {
            this._dbc = dbc;
        }

        public async Task<bool> add(PackageDayNewDTO packageDayNewDTO)
        {
            PackageService packageService = new PackageService(_dbc);
            Package package = await packageService.findById(packageDayNewDTO.id_of_package);
            if (package == null) return false;
            packageService = null;

            PackageDayService PackageDayService = new PackageDayService(_dbc);

            return await PackageDayService.add(packageDayNewDTO, package);
        }


        public async Task<List<PackageDayViewModel>> listAllByIdOfPackage(User user, PackageIdDTO packageIdDTO, bool isLessonPreviewInfo = false, bool withVideoSrcFromApi = false)
        {
            PackageService packageService = new PackageService(_dbc);
            Package package = await packageService.findById(packageIdDTO.id_of_package);
            if (package == null) return null;
            return await listAllByPackage(user, package, isLessonPreviewInfo, withVideoSrcFromApi);
        }

        public async Task<List<PackageDayViewModel>> listAllByPackage(User user, Package package, bool isLessonPreviewInfo = false, bool withVideoSrcFromApi = false)
        {

            PackageDayService packageDayService = new PackageDayService(_dbc);
            List<PackageDay> packageDayList = await packageDayService.listAllByPackage(package);

            List<PackageDayViewModel> packageDayViewModelList = new List<PackageDayViewModel>();

            PackageLessonService packageLessonService = new PackageLessonService(_dbc);
            //все возможные уроки в пакете
            List<PackageLesson> packageLessonListAll = await packageLessonService.listAllByPackage(package);

            //все истории просмотров
            ObserverLessonUserFacade observerLessonUserFacade = new ObserverLessonUserFacade(_dbc);
            Dictionary<int, ObserverLessonUserOnlyTime> dictObserverLessonUser = null;
            List<int> idsOfAllPackageLessonsDoneByUser = null;
            List<Homework> homeworksOfUser = null;
            HomeworkService homeworkService = new HomeworkService(_dbc);
            if (user != null)
            {
                dictObserverLessonUser = await observerLessonUserFacade.dictAllByUser(user);
                //все выполненные домашние задания
                PackageLessonFacade packageLessonFacade = new PackageLessonFacade(_dbc);
                idsOfAllPackageLessonsDoneByUser = await packageLessonFacade.listIdOfAllDoneByUser(user);
                packageLessonFacade = null;

                homeworksOfUser = await homeworkService.listAllByUser(user);
            }



            int schetchik = 0;
            int procentsViewed = 0;
            int isFinished_2True_1False_0Start = 0;
            bool isAvailable = true;
            bool isPackageDayFinished = false;
            int countOfAllLesons = 0;
            int countOfAllFinishedLessons = 0;
            int lessonCurrentTime = 0;
            bool isHomeworkSend = false;
            LessonFacade lessonFacade = new LessonFacade(_dbc);
            HomeworkFacade homeworkFacade = new HomeworkFacade(_dbc);
            HomeworkLiteViewModel homeworkLiteViewModel = null;
            foreach (PackageDay packageDay in packageDayList)
            {
                isPackageDayFinished = false;
                List<PackageLessonLiteViewModel> packageLessonList = new List<PackageLessonLiteViewModel>();
                foreach (PackageLesson packageLesson in packageLessonListAll)
                {
                    if (isLessonPreviewInfo)
                    {
                        if (packageLesson.packageDay == packageDay)
                        {
                            countOfAllLesons++;
                            isHomeworkSend = false;

                            procentsViewed = 0;
                            lessonCurrentTime = 0;

                            //проверяем, есть ли пользоватеть в истории просмотров, если есть, вычисляем процент просмотра, и вычисляем, есть ли доступ исходя их этого
                            if (packageLesson.lesson != null)
                            {
                                if (dictObserverLessonUser.ContainsKey(packageLesson.lesson.id))
                                {
                                    procentsViewed = (int)(dictObserverLessonUser[packageLesson.lesson.id].maxViewedTime * 100 / dictObserverLessonUser[packageLesson.lesson.id].length);
                                    lessonCurrentTime = dictObserverLessonUser[packageLesson.lesson.id].currentTime;
                                }
                            }

                            if (isFinished_2True_1False_0Start == 1 && isAvailable) isAvailable = false;

                            if (isFinished_2True_1False_0Start == 0)
                            {
                                isFinished_2True_1False_0Start = isLessonFinished_2True_1False(packageLesson, procentsViewed, await homeworkService.isAnySend(user, packageLesson));
                            }
                            else
                            {
                                if (isFinished_2True_1False_0Start == 2)
                                {
                                    isFinished_2True_1False_0Start = isLessonFinished_2True_1False(packageLesson, procentsViewed, await homeworkService.isAnySend(user, packageLesson));
                                }
                                else
                                {//значит предыдущий не сдан
                                    isFinished_2True_1False_0Start = 1;
                                }
                            }
                            if (isFinished_2True_1False_0Start == 2) countOfAllFinishedLessons++;

                            //проверяем на домашнее задание
                            if(packageLesson.homeworkStatus == 1)if (idsOfAllPackageLessonsDoneByUser.Contains(packageLesson.id)) isHomeworkSend = true;


                            homeworkLiteViewModel = null;
                            foreach (Homework homework in homeworksOfUser)
                            {
                                if (homework.packageLesson == packageLesson) homeworkLiteViewModel = new HomeworkLiteViewModel(
                                    homework.id,
                                    homework.comment,
                                    homeworkFacade.getHomeworkVideoSrc(homework),
                                    homework.status,
                                    homework.statusOfSeen,
                                    homework.statusOfUpload,
                                    (homework.date_of_add != null ? homework.date_of_add.Value.ToString("HH:mm:ss dd.MM.yyyy") : null),
                                    (homework.date_of_update != null ? homework.date_of_update.Value.ToString("HH:mm:ss dd.MM.yyyy") : null),
                                    homework.answer_from_teacher,
                                    (homework.date_of_update_of_teacher != null ? homework.date_of_update_of_teacher.Value.ToString("HH:mm:ss dd.MM.yyyy") : null),
                                    (homework.date_of_seen_by_admin != null ? homework.date_of_seen_by_admin.Value.ToString("HH:mm:ss dd.MM.yyyy") : null),
                                    homework.status_of_seen_of_message_from_teacher
                                );
                            }

                            packageLessonList.Add(
                                new PackageLessonLiteViewModel(
                                packageLesson.id,
                                (packageLesson.lesson != null ? await lessonFacade.getFullInfo(packageLesson.lesson.id, withVideoSrcFromApi) : null),
                                packageLesson.homeworkStatus,
                                packageLesson.homeworkText,
                                procentsViewed,
                                lessonCurrentTime,
                                (isFinished_2True_1False_0Start == 2 ? true : false),
                                isAvailable,
                                isHomeworkSend,
                                homeworkLiteViewModel
                                )
                            );
                        }
                    } else
                    {
                        if (packageLesson.packageDay == packageDay) packageLessonList.Add(
                            new PackageLessonLiteViewModel(
                                packageLesson.id,
                                (packageLesson.lesson != null ? packageLesson.lesson.id : 0),
                                packageLesson.homeworkStatus,
                                packageLesson.homeworkText
                            )
                        );
                    }
                }

                schetchik++;

                if (countOfAllLesons == countOfAllFinishedLessons) isPackageDayFinished = true;

                packageDayViewModelList.Add(
                    new PackageDayViewModel(
                        packageDay.id,
                        schetchik,
                        packageDay.name,
                        packageLessonList,
                        isPackageDayFinished
                    )
                );
            }

            package = null;
            packageDayService = null;
            packageLessonService = null;
            packageDayList = null;
            packageLessonListAll = null;
            lessonFacade = null;

            return packageDayViewModelList;
        }

        public async Task<bool> update(PackageDayDTO PackageDayDTO)
        {
            PackageDayService PackageDayService = new PackageDayService(_dbc);
            return await PackageDayService.update(PackageDayDTO);
        }

        public async Task<bool> delete(PackageDayIdDTO packageDayIdDTO)
        {
            PackageDayService packageDayService = new PackageDayService(_dbc);
            PackageDay packageDay = await packageDayService.findById(packageDayIdDTO.id);

            PackageLessonService packageLessonService = new PackageLessonService(_dbc);
            await packageLessonService.deleteAllByPackageDay(packageDay);

            return await packageDayService.delete(packageDayIdDTO.id);
        }




        private int isLessonFinished_2True_1False(PackageLesson packageLesson, int procentsViewed, bool isAnyHomeworkSent)
        {
            if (packageLesson.homeworkStatus == 0 && procentsViewed > 90)
            {
                return 2;
            }
            //нужна еще проверка на выполнение домашнего задания
            if (packageLesson.homeworkStatus == 1 && procentsViewed > 90 && isAnyHomeworkSent)
            {
                return 2;
            }

            return 1;
        }

    }
}
