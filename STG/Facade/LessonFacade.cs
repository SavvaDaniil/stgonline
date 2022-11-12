using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Linq;
using STG.Attribute;
using STG.Component;
using STG.Data;
using STG.DTO.Lesson;
using STG.Entities;
using STG.Interface.Facade;
using STG.Service;
using STG.ViewModels;
using STG.ViewModels.Homework;
using STG.ViewModels.Lesson;
using STG.ViewModels.ObserverLessonUser;
using STG.ViewModels.Package;
using STG.ViewModels.Subscription;
using STG.ViewModels.Video;

namespace STG.Facade
{
    public class LessonFacade : ILessonFacade
    {
        private ApplicationDbContext _dbc;
        public LessonFacade(ApplicationDbContext dbc)
        {
            _dbc = dbc;
        }
        private readonly IServiceScopeFactory _serviceScopeFactory;
        public LessonFacade(ApplicationDbContext dbc, IServiceScopeFactory serviceScopeFactory)
        {
            _dbc = dbc;
            _serviceScopeFactory = serviceScopeFactory;
        }

        private string uploadFolder = Directory.GetCurrentDirectory() + "\\wwwroot\\uploads\\lesson\\";
        private const string folderVideoContentUrlApiServer = "XXXXXXXXXXXXXXXXXXXXXXXXXX";


        public List<LessonMicroViewModel> listAllByMicro()
        {
            LessonService lessonService = new LessonService(this._dbc);
            IEnumerable<Lesson> lessonsEnum = lessonService.listAll();

            List<LessonMicroViewModel> lessonMicros = new List<LessonMicroViewModel>();
            foreach (Lesson lesson in lessonsEnum)
            {
                lessonMicros.Add(
                    new LessonMicroViewModel(
                        lesson.id, 
                        lesson.name, 
                        lesson.active, 
                        lesson.isVisible, 
                        lesson.orderInList,
                        getPosterPath(lesson.id)
                    )
                );
            }

            return lessonMicros;
        }

        public bool changeOrder(LessonOrderDTO lessonOrderDTO)
        {
            LessonService lessonService = new LessonService(_dbc);
            bool answer = lessonService.changeOrder(lessonOrderDTO);
            lessonService = null;
            return answer;
        }
        


        public List<LessonLiteViewModel> getAllActiveByFilter(
            HttpContext httpContext, int id_of_style, int id_of_level, int id_of_teacher, string name, int skip, int isFree, int take)
        {
            UserFacade userFacade = new UserFacade(_dbc);
            User user = userFacade.getCurrentOrNull(httpContext);
            if (user != null)
            {
                System.Diagnostics.Debug.WriteLine("user распознан: " + user.Id);
            }

            LessonService lessonService = new LessonService(_dbc);
            List<Lesson> lessonList = lessonService.listAllActiveByFilter(name, id_of_style, id_of_level, id_of_teacher, skip, isFree, take);

            PurchaseLessonService purchaseLessonService = new PurchaseLessonService(_dbc);
            //List<PurchaseLesson> purchaseLessonListOfUser = null;

            PurchaseLessonFacade purchaseLessonFacade = new PurchaseLessonFacade(_dbc);
            PurchaseSubscriptionFacade purchaseSubscriptionFacade = new PurchaseSubscriptionFacade(_dbc);
            PurchasePackageFacade purchasePackageFacade = new PurchasePackageFacade(_dbc);
            HashSet<int> setIdOfAvailableLessons = new HashSet<int>();
            HashSet<int> setIdOfAvailableLessonsByPackages = new HashSet<int>();
            bool isAnyActiveSubscriptionOfUser = false;
            bool isAnyActivePackageOfUser = false;
            bool userLessonFullAccess = false;

            if (user != null)
            {
                if (user.isLessonFullAccess == 1) userLessonFullAccess = true;
                 setIdOfAvailableLessons = purchaseLessonFacade.getAllAvaiableHashSetOfIdLessonByUser(user);
                isAnyActiveSubscriptionOfUser = purchaseSubscriptionFacade.isAnyActive(user);
                //setIdOfAvailableLessonsByPackages = getAllIdOfAvailablePrivateLessonsOfUser(user);
                setIdOfAvailableLessonsByPackages = getAllIdOfAvailablePrivateLessonsForAllConnectedPackagesOfUser(user);

                isAnyActivePackageOfUser = purchasePackageFacade.isAnyActive(user);
                //purchaseLessonListOfUser = purchaseLessonService.listAllActiveByUser(user);
            }

            List<LessonLiteViewModel> lessonLiteViewModels = new List<LessonLiteViewModel>();

            //bool isAlreadyAdded = false;
            foreach (Lesson lesson in lessonList)
            {
                //isAlreadyAdded = false;
                bool isLessonActiveForUser = false;

                if (user != null)
                {
                    if (userLessonFullAccess)
                    {
                        isLessonActiveForUser = true;
                        //} else if(lesson.active == 2 && setIdOfAvailableLessonsByPackages.Contains(lesson.id))
                    }
                    else if (lesson.active == 2 && setIdOfAvailableLessonsByPackages != null)
                    {
                        if (setIdOfAvailableLessonsByPackages.Contains(lesson.id)) isLessonActiveForUser = true;
                    }

                    if (!isLessonActiveForUser)
                    {
                        if (lesson.active == 1 && (isAnyActiveSubscriptionOfUser || isAnyActivePackageOfUser))
                        {
                            isLessonActiveForUser = true;
                        }
                        else if (lesson.active == 1 && setIdOfAvailableLessons.Contains(lesson.id))
                        {
                            isLessonActiveForUser = true;
                        }
                    }
                }
              


                lessonLiteViewModels.Add(new LessonLiteViewModel(
                    lesson.id,
                    lesson.name,
                    lesson.shortName,
                    isLessonActiveForUser,
                    (lesson.level != null ? lesson.level.Name : null),
                    (lesson.lessonType != null ? lesson.lessonType.name : null),
                    (lesson.style != null ? lesson.style.name : null),
                    (lesson.teacher != null ? lesson.teacher.name : null),
                    getPosterPath(lesson.id),
                    getTeaserPath(lesson.id)
                    )
                 );

            }
            lessonService = null;
            lessonList = null;
            purchaseLessonService = null;
            //purchaseLessonListOfUser = null;
            GC.Collect();

            return lessonLiteViewModels;
        }

        public LessonViewModel getFullInfo(int id, bool withVideoSrcFromApi = false)
        {
            LessonService lessonService = new LessonService(this._dbc);
            Lesson lesson = lessonService.findById(id);
            if (lesson == null) return null;

            string teaserSrc = getTeaserPath(lesson.id);

            ConnectionLessonToLevelFacade connectionLessonToLevelFacade = new ConnectionLessonToLevelFacade(_dbc);
            List<Level> levelsOfLesson = connectionLessonToLevelFacade.getListOfLevel(lesson);

            LessonViewModel lessonViewModel = new LessonViewModel(
                lesson.id,
                lesson.name,
                lesson.shortName,
                lesson.musicName,
                lesson.description,
                lesson.price,
                lesson.days,
                lesson.active,
                lesson.isVisible,
                lesson.isFree,
                this.getPosterPath(lesson.id),
                teaserSrc,
                lesson.level,
                lesson.lessonType,
                lesson.teacher,
                lesson.style,
                lesson.video,
                levelsOfLesson
            );

            if (lesson.video != null && withVideoSrcFromApi)
            {
                VideoFacade videoFacade = new VideoFacade(_dbc);
                lessonViewModel.videoApiStatusViewModel = videoFacade.getStatusOfVideoFromApi(lesson.video.id, lesson.video.hashPath);
            }

            return lessonViewModel;
        }



        public LessonVideoViewModel getWithVideo(int id)
        {
            return this.get(id);
        }


        public LessonVideoViewModel getWithVideoWithAddInfo(HttpContext httpContext, int id, string id_of_package_as_string)
        {
            UserFacade userFacade = new UserFacade(_dbc);
            User user = userFacade.getCurrentOrNull(httpContext);
            if (user == null) return null;

            LessonService lessonService = new LessonService(this._dbc);
            Lesson lesson = lessonService.findById(id);
            if (lesson == null) return null;

            LessonVideoViewModel lessonVideoViewModel = this.get(id);
            if (lessonVideoViewModel == null) return null;

            ObserverLessonUserFacade observerLessonUserFacade = new ObserverLessonUserFacade(_dbc);
            ObserverLessonUserLiteViewModel observerLessonUserLiteViewModel = observerLessonUserFacade.getLast(user, lesson);

            //вытаскиваем возможные домашние задания для пакета, сначала проверяем, у пакета вообще есть этот урок
            LessonHomeworkViewModel lessonHomeworkViewModel = null;
            if (id_of_package_as_string != null)
            {
                try
                {
                    int id_of_package = int.Parse(id_of_package_as_string);
                    PackageLessonFacade packageLessonFacade = new PackageLessonFacade(_dbc);
                    PackageLesson packageLesson = packageLessonFacade.get(id_of_package, lesson);
                    Homework homework;
                    if (packageLesson != null)
                    {
                        //ищем домашние задания пользователя
                        HomeworkFacade homeworkFacade = new HomeworkFacade(_dbc);
                        homework = homeworkFacade.get(user, packageLesson);
                        lessonHomeworkViewModel = new LessonHomeworkViewModel(
                            packageLesson.id,
                            packageLesson.homeworkStatus,
                            packageLesson.homeworkText,
                            lesson.name,
                            (homework != null ? new HomeworkLiteViewModel(
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
                            ) : null)
                        );
                    }
                }
                catch
                {

                }
            }

            lessonVideoViewModel.observer = observerLessonUserLiteViewModel;
            lessonVideoViewModel.lessonHomeworkViewModel = lessonHomeworkViewModel;

            return lessonVideoViewModel;
        }


        private LessonVideoViewModel get(int id)
        {
            LessonService lessonService = new LessonService(this._dbc);
            Lesson lesson = lessonService.findById(id);
            if (lesson == null) return null;

            VideoFacade videoFacade = new VideoFacade(_dbc);
            VideoViewModel videoViewModel = videoFacade.get(lesson.video.id, false);

            string videoSrc = null;
            string videoMobileSrc = null;
            if (videoViewModel.videoApiStatusViewModel != null) {
                if (videoViewModel.videoApiStatusViewModel.isContentExist == 1 && videoViewModel.videoApiStatusViewModel.src != null)
                {
                    videoSrc = videoViewModel.videoApiStatusViewModel.src;
                    if (videoViewModel.videoApiStatusViewModel.mobileSrc != null)
                    {
                        videoMobileSrc = videoViewModel.videoApiStatusViewModel.mobileSrc;
                    }
                }
            }
            VideoSectionFacade videoSectionFacade = new VideoSectionFacade(_dbc);

            TeacherFacade teacherFacade = new TeacherFacade(_dbc);

            ConnectionLessonToLevelService connectionLessonToLevelService = new ConnectionLessonToLevelService(_dbc);
            string[] arrayOfLevelNames = connectionLessonToLevelService.arrayOfLevelName(lesson);

            //string teaserSrc = getTeaserPath(lesson.id);
            string teaserSrc = getTeaserSrcFromApi(lesson.id);

            LessonVideoViewModel lessonVideoViewModel = new LessonVideoViewModel(
                lesson.id,
                lesson.name,
                lesson.shortName,
                (lesson.teacher != null ? lesson.teacher.id : 0),
                (lesson.teacher != null ? lesson.teacher.name : null),
                (lesson.teacher != null ? teacherFacade.getPosterSrc(lesson.teacher.id) : null),

                (lesson.lessonType != null ? lesson.lessonType.id : 0),
                (lesson.lessonType != null ? lesson.lessonType.name : null),
                arrayOfLevelNames,

                (videoViewModel != null ? 
                    (videoViewModel.durationHours < 10 ? 0 + videoViewModel.durationHours.ToString() : videoViewModel.durationHours.ToString())
                    + ":" + (videoViewModel.durationMinutes < 10 ? 0 + videoViewModel.durationMinutes.ToString() : videoViewModel.durationMinutes.ToString())
                    + ":" + (videoViewModel.durationSeconds < 10 ? 0 + videoViewModel.durationSeconds.ToString() : videoViewModel.durationSeconds.ToString())
                    : null),

                lesson.musicName,

                (lesson.video != null ? lesson.video.id : 0),
                getPosterPath(lesson.id),
                videoSrc,
                videoMobileSrc,
                teaserSrc,
                null,
                null,
                (lesson.video != null ? videoSectionFacade.listAllByIdOfVideo(lesson.video.id) : null)
            );

            return lessonVideoViewModel;


        }



        public JsonAnswerStatus update(LessonDTO lessonDTO)
        {
            if (lessonDTO.isDeletePoster == 1)
            {
                this.deletePoster(lessonDTO.id);
            }
            if (lessonDTO.posterFile != null)
            {
                if (!ValidateImageAttribute.IsValid(lessonDTO.posterFile))
                {
                    return new JsonAnswerStatus("error", "wrong_image");
                }
                if (!(uploadPoster(lessonDTO.id, lessonDTO.posterFile)))
                {
                    return new JsonAnswerStatus("error", "unknown_error");
                }
            }

            if(lessonDTO.teaserFile != null)
            {
                uploadTeaser(lessonDTO.id, lessonDTO.teaserFile);
            }
            if (lessonDTO.isDeleteTeaser == 1)
            {
                deleteTeaserFromApi(lessonDTO.id);
            }

            LessonService lessonService = new LessonService(_dbc);
            Lesson lesson = lessonService.save(lessonDTO);

            LevelFacade levelFacade = new LevelFacade(_dbc);
            List<Level> levels = levelFacade.getListOfLevelFromStringListOfIds(lessonDTO.levels);
            ConnectionLessonToLevelFacade connectionLessonToLevelFacade = new ConnectionLessonToLevelFacade(_dbc);
            connectionLessonToLevelFacade.updateConnectionsOfLesson(lesson, levels);

            return new JsonAnswerStatus("success", null);
        }





        public bool delete(int id)
        {
            LessonService lessonService = new LessonService(this._dbc);
            Lesson lesson = lessonService.findById(id);
            if (lesson == null) return false;

            //здесь thread на удаление может


            return lessonService.delete(id);
        }




        private bool uploadPoster(int id, IFormFile posterFile)
        {
            string uploadFolder = this.uploadFolder + "\\" + id.ToString();
            if (!Directory.Exists(uploadFolder)) {
                Directory.CreateDirectory(uploadFolder);
            }
            if (!ValidateImageAttribute.IsValid(posterFile)) return false;

            Image posterFilerImage = ValidateImageSize.checkAndResizeIfNeed_16_9(posterFile);

            string pathwithfileName = uploadFolder + "\\poster.jpg";
            string pathwithTmpName = uploadFolder + "\\tmp.jpg";
            posterFilerImage.Save(pathwithTmpName);

            ResizeImageComponent resizeImageComponent = new ResizeImageComponent();
            resizeImageComponent.ResizeTmpImageAndSaveFinally_16_9(pathwithTmpName, pathwithfileName);
            resizeImageComponent = null;


            return true;
        }

        private void deletePoster(int id)
        {
            string filePath = this.uploadFolder + "\\" + id.ToString() + "\\poster.jpg";
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }

        private string getPosterPath(int id)
        {
            string filePath = this.uploadFolder + "\\" + id.ToString() + "\\poster.jpg";
            if (File.Exists(filePath))
            {
                return "/uploads/lesson/" + id.ToString() + "/poster.jpg";
            }
            return null;
        }


        private bool uploadTeaser(int id_of_lesson, IFormFile teaserFile)
        {
            if (!Directory.Exists(this.uploadFolder))
            {
                Directory.CreateDirectory(this.uploadFolder);
            }
            if (ValidateVideoAttribute.IsValid(teaserFile))
            {
                string filePath = this.uploadFolder + id_of_lesson.ToString() + "_teaser.mp4";
                using(var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    teaserFile.CopyToAsync(fileStream);
                }
                return true;
            }
            return false;
        }

        private string getTeaserPath(int id)
        {
            if(File.Exists(this.uploadFolder + id.ToString() + "\\teaser.mp4"))
            {
                return "/uploads/lesson/" + id.ToString() + "/teaser.mp4";
            } else if (File.Exists(this.uploadFolder + id.ToString() + "\\teaser.mov"))
            {
                return "/uploads/lesson/" + id.ToString() + "/teaser.mov";
            }
            return null;
        }
        private bool deleteTeaser(int id)
        {
            if (File.Exists(this.uploadFolder + id.ToString() + "\\teaser.mp4"))
            {
                File.Delete(this.uploadFolder + id.ToString() + "\\teaser.mp4");
                return true;
            } else if (File.Exists(this.uploadFolder + id.ToString() + "\\teaser.mov"))
            {
                File.Delete(this.uploadFolder + id.ToString() + "\\teaser.mov");
                return true;
            }
            return false;
        }


        public Lesson getIfAvailable(User user, Lesson lesson)
        {
            throw new NotImplementedException();
        }

        private string getTeaserSrcFromApi(int id_of_lesson)
        {
            return folderVideoContentUrlApiServer + "/uploads/lesson/" + id_of_lesson + "/teaser.mp4";

        }

        private bool uploadTeaserToApi(int id_of_lesson, IFormFile teaserFile)
        {
            return false;
        }

        private bool deleteTeaserFromApi(int id)
        {
            return false;
        }

        public JsonAnswerStatus checkAccessForUser(HttpContext httpContext, string domainHost, int id_of_lesson)
        {
            LessonService lessonService = new LessonService(_dbc);
            Lesson lesson = lessonService.findById(id_of_lesson);
            if (lesson == null) return new JsonAnswerStatus("error", "not_found");
            if (lesson.active == 0) return new JsonAnswerStatus("error", null);

            UserFacade userFacade = new UserFacade(_dbc);
            User user = userFacade.getCurrentOrNull(httpContext);
            if(user == null)
            {
                //if(lesson.active == 2) return new JsonAnswerStatus("error", "excluzive");
                //return new JsonAnswerStatus("error", "login");
            }
            if(user != null)
            {
                if(user.isLessonFullAccess == 1)return new JsonAnswerStatus("success", null);
            }

            PurchasePackageFacade purchasePackageFacade = new PurchasePackageFacade(_dbc);

            if (lesson.active == 1 && user != null)
            {
                PurchaseLessonFacade purchaseLessonFacade = new PurchaseLessonFacade(_dbc);
                if (purchaseLessonFacade.getFirstActiveAndActivateIfNull(httpContext, id_of_lesson) != null) return new JsonAnswerStatus("success", null);

                if (purchasePackageFacade.isAnyActive(user)) return new JsonAnswerStatus("success", null);
                //if ((getAllIdOfAvailablePrivateLessonsForAllConnectedPackagesOfUser(user)).Contains(lesson.id)) return new JsonAnswerStatus("success", null);

                PurchaseSubscriptionFacade purchaseSubscriptionFacade = new PurchaseSubscriptionFacade(_dbc, _serviceScopeFactory);
                if (purchaseSubscriptionFacade.getFirstActiveAndActivateIfNull(httpContext, domainHost) != null) return new JsonAnswerStatus("success", null);
            }

            string teaserSrc = getTeaserSrcFromApi(lesson.id);
            LessonTeaserViewModel lessonTeaserViewModel = new LessonTeaserViewModel(lesson.id, teaserSrc);

            if (lesson.active == 2 && user != null)
            {
                //if (purchasePackageFacade.getFirstActiveForLesson(user, lesson, true) != null) return new JsonAnswerStatus("success", null);
                HashSet<int> setIdOfAvailableLessonsByPackages = getAllIdOfAvailablePrivateLessonsForAllConnectedPackagesOfUser(user);
                if(setIdOfAvailableLessonsByPackages != null)
                {
                    if (setIdOfAvailableLessonsByPackages.Contains(lesson.id)) return new JsonAnswerStatus("success", null);
                }

                return new JsonAnswerStatus("error", "excluzive", lessonTeaserViewModel);
            } else if(lesson.active == 2)
            {
                return new JsonAnswerStatus("error", "excluzive", lessonTeaserViewModel);
            }


            return new JsonAnswerStatus("error", "not_available", lessonTeaserViewModel);
        }

        public int checkAccessForUserAndUpdateAccessIfNull_none0_buy1_granted2(HttpContext httpContext, string domainHost, int id_of_lesson)
        {
            LessonService lessonService = new LessonService(_dbc);
            Lesson lesson = lessonService.findById(id_of_lesson);
            if (lesson == null) return 0;
            if (lesson.active == 0) return 0;

            UserFacade userFacade = new UserFacade(_dbc);
            User user = userFacade.getCurrentOrNull(httpContext);
            if (user == null) return 0;
            if (user.isLessonFullAccess == 1) return 2;

            PurchasePackageFacade purchasePackageFacade = new PurchasePackageFacade(_dbc); 

            if (lesson.active == 1) {
                PurchaseLessonFacade purchaseLessonFacade = new PurchaseLessonFacade(_dbc);
                if (purchaseLessonFacade.getFirstActiveAndActivateIfNull(httpContext, id_of_lesson) != null) return 2;

                if (purchasePackageFacade.isAnyActive(user)) return 2;

                PurchaseSubscriptionFacade purchaseSubscriptionFacade = new PurchaseSubscriptionFacade(_dbc, _serviceScopeFactory);
                if (purchaseSubscriptionFacade.getFirstActiveAndActivateIfNull(httpContext, domainHost) != null) return 2;
            }

            if (lesson.active == 2)
            {
                //if ((getAllIdOfAvailablePrivateLessonsOfUser(user)).Contains(lesson.id)) return 2;
                if (user.isPublicPackageFullAccess == 1) return 2;
                if (purchasePackageFacade.getFirstActiveForLesson(user, lesson, true) != null) return 2;
                return 0;
            }

            return 1;
        }

        public LessonBuyViewModel getInfoForBuying(HttpContext httpContext, int id)
        {
            UserFacade userFacade = new UserFacade(_dbc);
            User user = userFacade.getCurrentOrNull(httpContext);

            LessonService lessonService = new LessonService(_dbc);
            Lesson lesson = lessonService.findById(id);
            if (lesson == null) return null;

            SubscriptionFacade subscriptionFacade = new SubscriptionFacade(_dbc);

            return new LessonBuyViewModel(
                lesson,
                subscriptionFacade.listAllActiveForAnyLesson(user)
            );
        }


        private HashSet<int> getAllIdOfAvailablePrivateLessonsOfUser(User user)
        {
            PackageLessonFacade packageLessonFacade = new PackageLessonFacade(_dbc);
            List<PackageLesson> packageLessons = packageLessonFacade.listAllAvailableByActivePackagesOfUser(user);
            HashSet<int> idOfAvailableLessons = new HashSet<int>();
            foreach (PackageLesson packageLesson in packageLessons)
            {
                if (packageLesson.lesson == null) continue;
                if (!idOfAvailableLessons.Contains(packageLesson.lesson.id))
                {
                    idOfAvailableLessons.Add(packageLesson.lesson.id);
                }
            }
            return idOfAvailableLessons;
        }


        private HashSet<int> getAllIdOfAvailablePrivateLessonsForAllConnectedPackagesOfUser(User user)
        {
            PackageFacade packageFacade = new PackageFacade(_dbc);
            List<Package> packagesConnectedToUser = packageFacade.listAllAvailableForUser(user);
            if (packagesConnectedToUser.Count == 0) return null;

            PackageDayFacade packageDayFacade = new PackageDayFacade(_dbc);
            HashSet<int> idOfAvailableLessons = new HashSet<int>();
            List<PackageDayViewModel> packageDaysOfPackage = new List<PackageDayViewModel>();

            foreach (Package packageConnectedToUser in packagesConnectedToUser)
            {
                packageDaysOfPackage = packageDayFacade.listAllByPackage(user, packageConnectedToUser, true, false);

                foreach(PackageDayViewModel packageDayViewModel in packageDaysOfPackage)
                {
                    if (packageDayViewModel.package_lesson_list == null) continue;

                    foreach (PackageLessonLiteViewModel packageLessonLiteViewModel in packageDayViewModel.package_lesson_list)
                    {
                        if (packageLessonLiteViewModel.lessonViewModel == null) continue;


                        if (packageLessonLiteViewModel.isAvailable)
                        {
                            if (!idOfAvailableLessons.Contains(packageLessonLiteViewModel.lessonViewModel.id)) idOfAvailableLessons.Add(packageLessonLiteViewModel.lessonViewModel.id);
                        }
                    }
                }
            }

            return idOfAvailableLessons;
        }
    }
}
