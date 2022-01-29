using Microsoft.AspNetCore.Http;
using STG.Attribute;
using STG.Component;
using STG.Data;
using STG.DTO.ConnectionUserToPrivatePackage;
using STG.DTO.Package;
using STG.Entities;
using STG.Service;
using STG.ViewModels;
using STG.ViewModels.Package;
using STG.ViewModels.PackageChat;
using STG.ViewModels.TeacherViewModel;
using STG.ViewModels.User;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace STG.Facade
{
    public class PackageFacade
    {
        private ApplicationDbContext _dbc;
        public PackageFacade(ApplicationDbContext dbc)
        {
            this._dbc = dbc;
        }

        private string uploadFolder = Directory.GetCurrentDirectory() + "\\wwwroot\\uploads\\package\\";
        private string defaultPosterSrc = "/images/preview_box_default_1.jpg";


        public async Task<bool> add(PackageNewDTO packageNewDTO)
        {
            PackageService packageService = new PackageService(_dbc);
            return await packageService.add(packageNewDTO);
        }

        public async Task<List<PackagePreviewViewModel>> listAllPrivatePreview(HttpContext httpContext)
        {
            List<int> idsOfAllPackagesActiveForUser = null;
            List<int> listAllIdOfPrivatePackagesAvailableForUser = null;
            UserFacade userFacade = new UserFacade(_dbc);
            User user = await userFacade.getCurrentOrNull(httpContext);
            if(user != null)
            {
                idsOfAllPackagesActiveForUser = await listIdOfAllActiveByUser(user);

                //получаем массив id пакетов допустимых к просмотру пользователем
                ConnectionUserToPrivatePackageFacade connectionUserToPrivatePackageFacade = new ConnectionUserToPrivatePackageFacade(_dbc);
                listAllIdOfPrivatePackagesAvailableForUser = await connectionUserToPrivatePackageFacade.listAllIdOfPrivatePackagesConnectedToUser(user);
            }

            PackageService packageService = new PackageService(_dbc);
            IEnumerable<Package> packagesPrivateEnum = await packageService.listAllPrivateActive();
            List<PackagePreviewViewModel> packagePreviewViewModels = new List<PackagePreviewViewModel>();

            PackageLessonService packageLessonService = new PackageLessonService(_dbc);

            int lessonsCount = 0;
            string posterSrc;

            if (listAllIdOfPrivatePackagesAvailableForUser != null)
            {
                foreach (Package package in packagesPrivateEnum)
                {
                    if (!listAllIdOfPrivatePackagesAvailableForUser.Contains(package.id)) continue;
                    lessonsCount = await packageLessonService.countAllByPackage(package);
                    posterSrc = getPosterSrc(package.id);
                    if (posterSrc == null) posterSrc = defaultPosterSrc;

                    packagePreviewViewModels.Add(
                        new PackagePreviewViewModel(
                            package.id,
                            package.name,
                            (idsOfAllPackagesActiveForUser.Contains(package.id) ? 1 : 0),
                            (package.level != null ? package.level.Name.ToLower() : null),
                            (package.style != null ? package.style.name : null),
                            (package.teacher != null ? package.teacher.name : null),
                            lessonsCount,
                            posterSrc,
                            null
                        )
                    );
                }
            }

            return packagePreviewViewModels;
        }

        public async Task<List<PackageLiteViewModel>> listAll()
        {
            PackageService programService = new PackageService(_dbc);

            IEnumerable<Package> packagesEnum = await programService.listAll();
            List<PackageLiteViewModel> packageLiteViewModels = new List<PackageLiteViewModel>();

            foreach(Package package in packagesEnum)
            {
                packageLiteViewModels.Add(
                    new PackageLiteViewModel(
                        package.id,
                        package.name,
                        package.active
                    )
                );
            }
            programService = null;
            packagesEnum = null;

            return packageLiteViewModels;
        }

        public async Task<PackageEditViewModel> getForEdit(int id)
        {
            PackageService packageService = new PackageService(_dbc);
            Package package = await packageService.findById(id);
            if (package == null) return null;

            LevelService levelService = new LevelService(this._dbc);
            IEnumerable<Level> levels = await levelService.listAll();

            StyleService styleService = new StyleService(this._dbc);
            IEnumerable<Style> styles = await styleService.listAllActive();

            TeacherService teacherService = new TeacherService(this._dbc);
            IEnumerable<Teacher> teachers = await teacherService.listAllActiveCurator();

            TariffService tariffService = new TariffService(_dbc);
            IEnumerable<Tariff> tariffs = await tariffService.enumAll();

            PackageEditViewModel packageEditViewModel = new PackageEditViewModel(
                package.id,
                package.name,
                package.active,
                package.price,
                package.days,
                package.description,
                package.orderInList,
                getPosterSrc(package.id),
                package.level,
                package.style,
                package.teacher,
                package.tariff,
                package.statusOfChatNone0Homework1AndChat2
            );

            packageEditViewModel.levelList = levels;
            packageEditViewModel.styleList = styles;
            packageEditViewModel.teacherList = teachers;
            packageEditViewModel.tariffs = tariffs;


            return packageEditViewModel;
        }

        public async Task<PackageInfoViewModel> getFullInfoForUser(HttpContext httpContext, int id_of_package)
        {
            UserFacade userFacade = new UserFacade(_dbc);
            User user = await userFacade.getCurrentOrNull(httpContext);
            return await getFullInfo(user, id_of_package);
        }

        public async Task<UserPassingPackageViewModel> getUserPassingPackageForUser(HttpContext httpContext, int id_of_package)
        {
            UserFacade userFacade = new UserFacade(_dbc);
            User user = await userFacade.getCurrentOrNull(httpContext);
            if (user == null) return null;

            ConnectionUserToPrivatePackageService connectionUserToPrivatePackageService = new ConnectionUserToPrivatePackageService(_dbc);
            ConnectionUserToPrivatePackage connectionUserToPrivatePackage = await connectionUserToPrivatePackageService.find(user, id_of_package);
            if (connectionUserToPrivatePackage == null || connectionUserToPrivatePackage.user == null || connectionUserToPrivatePackage.package == null) return null;


            PackageInfoViewModel packageInfoViewModel = await getFullInfo(user, id_of_package);

            //считываем чат
            PackageChatFacade packageChatFacade = new PackageChatFacade(_dbc);
            List<PackageChatMessageViewModel> packageChatMessageViewModels = await packageChatFacade.get(connectionUserToPrivatePackage.id, 0, true);

            return new UserPassingPackageViewModel(
                packageInfoViewModel,
                packageChatMessageViewModels
            );
        }

        public async Task<UserPassingPackageViewModel> getUserPassingPackageForAdmin(ConnectionUserToPackagePrivateIdDTO connectionUserToPackagePrivateIdDTO)
        {
            ConnectionUserToPrivatePackageService connectionUserToPrivatePackageService = new ConnectionUserToPrivatePackageService(_dbc);
            ConnectionUserToPrivatePackage connectionUserToPrivatePackage = await connectionUserToPrivatePackageService.findById(connectionUserToPackagePrivateIdDTO.id);
            if (connectionUserToPrivatePackage == null || connectionUserToPrivatePackage.user == null || connectionUserToPrivatePackage.package == null) return null;

            User user = connectionUserToPrivatePackage.user;
            if (user == null) return null;
            PackageInfoViewModel packageInfoViewModel = await getFullInfo(user, connectionUserToPrivatePackage.package.id);

            //ставим прочитано у домашнего задания
            //HomeworkService homeworkService = new HomeworkService(_dbc);
            //await homeworkService.setSeenListByAdmin(connectionUserToPrivatePackage.user, connectionUserToPrivatePackage.package);

            //считываем чат
            PackageChatFacade packageChatFacade = new PackageChatFacade(_dbc);
            List<PackageChatMessageViewModel> packageChatMessageViewModels = await packageChatFacade.get(connectionUserToPackagePrivateIdDTO.id, 1, true);

            return new UserPassingPackageViewModel(
                packageInfoViewModel,
                packageChatMessageViewModels
            );
        }

        private async Task<PackageInfoViewModel> getFullInfo(User user, int id_of_package)
        {
            PackageService packageService = new PackageService(_dbc);
            Package package = await packageService.findById(id_of_package);
            if (package == null) return null;

            TeacherFacade teacherFacade = new TeacherFacade(_dbc);
            TeacherLiteViewModel teacherLiteViewModel = null;
            if (package.teacher != null)
            {
                teacherLiteViewModel = await teacherFacade.get(package.teacher.id);
            }

            int active = 0;
            bool isAnyUnreadByUser = false;
            if (user != null)
            {
                PurchasePackageFacade purchasePackageFacade = new PurchasePackageFacade(_dbc);
                if (await purchasePackageFacade.getFirstActive(user, package) != null) active = 1;

                PackageChatService packageChatService = new PackageChatService(_dbc);
                isAnyUnreadByUser = await packageChatService.isAnyUnreadByUserAndPackage(user, package, 1);
            }

            PackageDayFacade packageDayFacade = new PackageDayFacade(_dbc);
            PackageIdDTO packageIdDTO = new PackageIdDTO(); packageIdDTO.id_of_package = package.id;
            List<PackageDayViewModel> packageDays = await packageDayFacade.listAllByIdOfPackage(user, packageIdDTO, true);

            int countOfAllFinishedPackageDays = countOfAllFinishedPackageDay(packageDays);
            int summaOfAllLessons = countOfAllLessons(packageDays);
            int summaOfAllFinishedLessons = countOfAllLessons(packageDays, true);
            int procentOfFinishedPackage = (summaOfAllLessons > 0 ? summaOfAllFinishedLessons * 100 / summaOfAllLessons : 0);

            PackageLessonLiteViewModel packageLessonLiteViewModelLastViewed = getLastViewedPackageDay(packageDays);

            string posterSrc = getPosterSrc(package.id);
            if (posterSrc == null) posterSrc = this.defaultPosterSrc;


            PackageInfoViewModel packageInfoViewModel = new PackageInfoViewModel(
                package.id,
                (package.name != null ? package.name : "Программа"),
                active,
                package.active,
                package.price,
                package.days,
                package.description,
                package.statusOfChatNone0Homework1AndChat2,
                package.orderInList,
                posterSrc,
                null,
                package.level,
                package.style,
                package.tariff,
                teacherLiteViewModel,
                packageDays,
                countOfAllFinishedPackageDays,
                summaOfAllLessons,
                summaOfAllFinishedLessons,
                procentOfFinishedPackage,
                packageLessonLiteViewModelLastViewed,
                isAnyUnreadByUser
            );

            return packageInfoViewModel;
        }


        private async Task<List<int>> listIdOfAllActiveByUser(User user)
        {
            PurchasePackageService purchasePackageService = new PurchasePackageService(_dbc);
            List<PurchasePackage> purchasePackages = await purchasePackageService.listAllActive(user);

            List<int> idsOfAllDoneByUser = new List<int>();

            foreach (PurchasePackage purchasePackage in purchasePackages)
            {
                //System.Diagnostics.Debug.WriteLine("purchasePackage: " + purchasePackage.package.id);
                if (purchasePackage.package == null) continue;
                if (!idsOfAllDoneByUser.Contains(purchasePackage.package.id)) idsOfAllDoneByUser.Add(purchasePackage.package.id);
            }

            return idsOfAllDoneByUser;
        }

        private PackageLessonLiteViewModel getLastViewedPackageDay(List<PackageDayViewModel> packageDays)
        {
            PackageLessonLiteViewModel packageLessonLiteViewModelLastViewed = null;
            foreach (PackageDayViewModel packageDayViewModel in packageDays)
            {
                if (packageLessonLiteViewModelLastViewed != null) break;
                foreach (PackageLessonLiteViewModel packageLessonLiteViewModel in packageDayViewModel.package_lesson_list)
                {
                    if (!packageLessonLiteViewModel.isFinished)
                    {
                        packageLessonLiteViewModelLastViewed = packageLessonLiteViewModel;
                        break;
                    }
                }
            }
            return packageLessonLiteViewModelLastViewed;
        }

        private int countOfAllFinishedPackageDay(List<PackageDayViewModel> packageDays)
        {
            int summa = 0;
            foreach(PackageDayViewModel packageDayViewModel in packageDays)
            {
                if (packageDayViewModel.isFinished) summa++;
            }
            return summa;
        }

        private int countOfAllLessons(List<PackageDayViewModel> packageDays, bool isFinished = false)
        {
            int summa = 0;
            foreach(PackageDayViewModel packageDayViewModel in packageDays)
            {
                foreach(PackageLessonLiteViewModel packageLessonLiteViewModel in packageDayViewModel.package_lesson_list)
                {
                    if (isFinished)
                    {
                        if(packageLessonLiteViewModel.isFinished) summa++;
                    } else
                    {
                        summa++;
                    }
                }
            }
            return summa;
        }

        public async Task<PackageBuyViewModel> getInfoForBuying(int id)
        {
            PackageService packageService = new PackageService(_dbc);
            Package package = await packageService.findById(id);
            if (package == null) return null;

            PackageBuyViewModel packageBuyViewModel = new PackageBuyViewModel(package);

            return packageBuyViewModel;
        }

        private void deletePoster(int id)
        {
            string filePath = this.uploadFolder + "\\" + id.ToString() + "\\poster.jpg";
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }

        private string getPosterSrc(int id)
        {
            string filePath = this.uploadFolder + "\\" + id.ToString() + "\\poster.jpg";
            if (File.Exists(filePath))
            {
                return "/uploads/package/" + id.ToString() + "/poster.jpg";
            }
            return null;
        }



        public async Task<JsonAnswerStatus> update(PackageDTO packageDTO)
        {
            PackageService packageService = new PackageService(_dbc);
            Package package = await packageService.findById(packageDTO.id);
            if (package == null) return new JsonAnswerStatus("error", "not_found");

            if (packageDTO.isPosterDelete == 1)
            {
                this.deletePoster(packageDTO.id);
            }
            if (packageDTO.posterFile != null)
            {
                if (!ValidateImageAttribute.IsValid(packageDTO.posterFile))
                {
                    return new JsonAnswerStatus("error", "wrong_image");
                }
                if (!(uploadPoster(packageDTO.id, packageDTO.posterFile)))
                {
                    return new JsonAnswerStatus("error", "unknown_error");
                }
            }

            LevelService levelService = new LevelService(this._dbc);
            Level level = await levelService.findById(packageDTO.id_of_level);

            StyleService styleService = new StyleService(this._dbc);
            Style style = await styleService.findById(packageDTO.id_of_style);

            TeacherService teacherService = new TeacherService(this._dbc);
            Teacher teacher = await teacherService.findById(packageDTO.id_of_teacher);

            TariffService tariffService = new TariffService(this._dbc);
            Tariff tariff = await tariffService.findById(packageDTO.id_of_tariff);


            if (!(await packageService.update(packageDTO, level, style, teacher, tariff)))
            {
                return new JsonAnswerStatus("error", "unknown");
            }

            return new JsonAnswerStatus("success", null);
        }




        private bool uploadPoster(int id, IFormFile posterFile)
        {
            string uploadFolder = this.uploadFolder + "\\" + id.ToString();
            if (!Directory.Exists(uploadFolder))
            {
                Directory.CreateDirectory(uploadFolder);
            }
            if (!ValidateImageAttribute.IsValid(posterFile)) return false;

            Image posterFilerImage = ValidateImageSize.checkAndResizeIfNeed_box(posterFile);

            string pathwithfileName = uploadFolder + "\\poster.jpg";
            string pathwithTmpName = uploadFolder + "\\tmp.jpg";
            posterFilerImage.Save(pathwithTmpName);

            ResizeImageComponent resizeImageComponent = new ResizeImageComponent();
            resizeImageComponent.ResizeTmpImageAndSaveFinally_box(pathwithTmpName, pathwithfileName);
            resizeImageComponent = null;

            return true;
        }








        public async Task<JsonAnswerStatus> delete(PackageIdDTO packageIdDTO)
        {
            PackageService programService = new PackageService(_dbc);
            Package package = await programService.findById(packageIdDTO.id_of_package);
            if (package == null) return new JsonAnswerStatus("error", "no_package");

            PurchasePackageService purchasePackageService = new PurchasePackageService(_dbc);
            if(await purchasePackageService.isAnyActivePurchaseForAnyUser(package))return new JsonAnswerStatus("error", "purchase_package_active_exist");

            PackageLessonService packageLessonService = new PackageLessonService(_dbc);
            if (!await packageLessonService.deleteAllByPackage(package)) return new JsonAnswerStatus("error", "package_lesson");

            PackageDayService packageDayService = new PackageDayService(_dbc);
            if (!await packageDayService.deleteAllByPackage(package)) return new JsonAnswerStatus("error", "package_day");

            ConnectionUserToPrivatePackageService connectionUserToPrivatePackageService = new ConnectionUserToPrivatePackageService(_dbc);
            if (!await connectionUserToPrivatePackageService.deleteAllByPackage(package)) return new JsonAnswerStatus("error", "package_connection");

            deletePoster(packageIdDTO.id_of_package);
            return (await programService.delete(package)
                ? new JsonAnswerStatus("success", null)
                : new JsonAnswerStatus("error", "unknown")
            );
        }


        public async Task<List<Package>> listAllAvailableForUser(User user)
        {
            ConnectionUserToPrivatePackageService connectionUserToPrivatePackageService = new ConnectionUserToPrivatePackageService(_dbc);
            List<Package> packages = new List<Package>();
            List<ConnectionUserToPrivatePackage> connectionsUserToPrivatePackage = await connectionUserToPrivatePackageService.listAllByUser(user);

            foreach (ConnectionUserToPrivatePackage connectionUserToPrivatePackage in connectionsUserToPrivatePackage)
            {
                if (connectionUserToPrivatePackage.package == null) continue;
                if (!packages.Contains(connectionUserToPrivatePackage.package))
                {
                    packages.Add(connectionUserToPrivatePackage.package);
                }
            }

            return packages;
        }


        /*
        
            public function listAllActive(): ?array{
                $programService = new ProgramService();
                $programList = $programService -> listAllActive();
                $programLiteViewModelList = [];
                foreach($programList as $program){

                    $posterSrc = $this -> getPosterSrc($program -> id);
                    if($posterSrc == null)$posterSrc = self::$defaultPosterSrc;

                    array_push($programLiteViewModelList,
                        new ProgramLiteViewModel(
                            $program -> id,
                            $program -> name,
                            $program -> short_description,
                            $posterSrc
                        )
                    );
                }
                $programService = null;
                $programList = null;
                $posterSrc = null;

                return $programLiteViewModelList;
            }

            public function listAllCuratorActive(): ?array{
                $programService = new ProgramService();
                $programCuratorList = $programService -> listAllCuratorActive();

                $programService = null;
                $programCuratorLiteViewModelList = [];

                foreach($programCuratorList as $programCurator){
                    array_push(
                        $programCuratorLiteViewModelList,
                        new ProgramCuratorLiteViewModel(
                            $programCurator -> id,
                            $programCurator -> name,
                            $programCurator -> price_curator
                        )
                    );
                }

                return $programCuratorLiteViewModelList;
            }



            public function getActive(int $id): ProgramViewModel {
                $programService = new ProgramService();

                $program = $programService -> findActiveById($id);
                if($program == null)return null;

                $posterSrc = $this -> getPosterSrc($program -> id);
                if($posterSrc == null)$posterSrc = self::$defaultPosterSrc;

                $lessonFacade = new LessonFacade();
                $lessons = $lessonFacade -> searchByFilter(new LessonFilterDTO(null, 0, $program -> id));

                return new ProgramViewModel(
                    $program -> id,
                    $program -> name,
                    $program -> is_curator,
                    $program -> price_curator,
                    $program -> instagram,
                    $program -> active,
                    $program -> description,
                    $program -> order_in_list,
                    $posterSrc,
                    $lessons
                );
            }


            public function update(ProgramDTO $programDTO): bool {
                $programService = new ProgramService();
        
                $program = $programService -> findById($programDTO -> id);
                if($program == null)return null;

                if($programDTO -> isPosterDelete == 1){
                    if(!$this -> deletePoster($program -> id))return false;
                }

                if($programDTO -> programPosterFileDTO -> check()){
                    if(!$this -> uploadPoster($program -> id, $programDTO -> programPosterFileDTO)) return false;
                }

                return $programService -> update($programDTO);
            }


            private function uploadPoster(int $id, ProgramPosterFileDTO $programPosterFileDTO): bool {
                $filepath = self::$folderUpload . $id . "/poster.jpg";

                if(!\file_exists(self::$folderUpload . $id)){
                    mkdir(self::$folderUpload . $id, 0775, true);
                }

                //$programPosterFileDTO = new ProgramPosterFileDTO();
                //$programPosterFileDTO -> posterFile = UploadedFile::getInstance($programPosterFileDTO, 'posterFile');

                if(!$programPosterFileDTO -> upload($id)) return false;
        
                if(!ResizeImageByFilePath::resize_9_16_AfterSave($filepath, false)) return false;

                return true;
            }
        */
    }
}
