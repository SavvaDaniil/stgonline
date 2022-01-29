using Microsoft.AspNetCore.Http;
using STG.Data;
using STG.DTO.PackageChat;
using STG.Entities;
using STG.Models;
using STG.Observer;
using STG.Service;
using STG.ViewModels;
using STG.ViewModels.Admin;
using STG.ViewModels.PackageChat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace STG.Facade
{
    public class PackageChatFacade
    {
        private ApplicationDbContext _dbc;
        public PackageChatFacade(ApplicationDbContext dbc)
        {
            this._dbc = dbc;
        }

        public async Task<List<PackageChatMessageViewModel>> get(HttpContext httpContext, int id_of_package)
        {
            UserFacade userFacade = new UserFacade(_dbc);
            User user = await userFacade.getCurrentOrNull(httpContext);
            if (user == null) return null;

            PackageService packageService = new PackageService(_dbc);
            Package package = await packageService.findById(id_of_package);
            if (package == null) return null;

            PackageChatService packageChatService = new PackageChatService(_dbc);
            List<PackageChat> packageChats = await packageChatService.listAllByUserAndConnectionUserToPrivatePackage(
                user,
                package
            );

            await packageChatService.updateListRead(packageChats, 0);

            List<PackageChatMessageViewModel> packageChatMessageViewModels = new List<PackageChatMessageViewModel>();
            foreach (PackageChat packageChat in packageChats)
            {
                packageChatMessageViewModels.Add(
                    new PackageChatMessageViewModel(
                        packageChat.id,
                        packageChat.message,
                        packageChat.user,
                        packageChat.teacher,
                        null,
                        packageChat.user0_teacher1,
                        packageChat.is_read,
                        null,
                        null,
                        (packageChat.date_of_add != null ? packageChat.date_of_add.Value.ToString("HH:mm:ss dd.MM.yyyy") : null)
                    )
                );
            }
            return packageChatMessageViewModels;
        }

        public async Task<List<PackageChatMessageViewModel>> get(int id_of_connectionUserToPrivatePackage, int for_user0_for_admin1 = 0, bool isUpdateRead = false)
        {
            ConnectionUserToPrivatePackageService connectionUserToPrivatePackageService = new ConnectionUserToPrivatePackageService(_dbc);
            ConnectionUserToPrivatePackage connectionUserToPrivatePackage = await connectionUserToPrivatePackageService.findById(id_of_connectionUserToPrivatePackage);
            if (connectionUserToPrivatePackage == null) return null;

            PackageChatService packageChatService = new PackageChatService(_dbc);
            List<PackageChat> packageChats = await packageChatService.listAllByUserAndConnectionUserToPrivatePackage(
                connectionUserToPrivatePackage.user,
                connectionUserToPrivatePackage.package
            );

            if (isUpdateRead)
            {
                await packageChatService.updateListRead(packageChats, (for_user0_for_admin1 == 1 ? 1 : 0));
            }

            /*
            HomeworkFacade homeworkFacade = new HomeworkFacade(_dbc);

                //нужны все домашние работы по PackageLesson каждый
                PackageLessonService packageLessonService = new PackageLessonService(_dbc);
                List<PackageLesson> packageLessonsOfPackageWithHomeworks = await packageLessonService.listAllByPackageWithHomeworks(connectionUserToPrivatePackage.package);
                HomeworkService homeworkService = new HomeworkService(_dbc);
                List<Homework> homeworksOfUser = await homeworkService.listAllByUser(connectionUserToPrivatePackage.user);

                List<Homework> homeworksForChat = new List<Homework>();
                foreach(PackageLesson packageLesson in packageLessonsOfPackageWithHomeworks)
                {
                //System.Diagnostics.Debug.WriteLine("packageLesson: " + packageLesson.id);
                foreach (Homework homework in homeworksOfUser)
                    {
                    //System.Diagnostics.Debug.WriteLine("homework: " + homework.id);
                        if(homework.packageLesson == packageLesson)
                    {
                        //System.Diagnostics.Debug.WriteLine("Помещен ");
                        homeworksForChat.Add(homework);
                        }
                    }
                }
                */

            List<PackageChatMessageViewModel> packageChatMessageViewModels = new List<PackageChatMessageViewModel>();
            //сканируем все сообщения и проверяем, если у следующего сообщения дата больше, чем у домашнего задания, то устанавливаем перед ним сообщение как дз

            List<int> idsOfHomeworksAlreadyAdded = new List<int>();
            foreach (PackageChat packageChat in packageChats)
            {
                /*
                foreach (Homework homework in homeworksForChat)
                {
                    if(homework.date_of_update < packageChat.date_of_add && !idsOfHomeworksAlreadyAdded.Contains(homework.id))
                    {
                        packageChatMessageViewModels.Add(
                            new PackageChatMessageViewModel(
                                0,
                                homework.comment,
                                homework.user,
                                null,
                                null,
                                0,
                                homework.statusOfSeen,
                                homework,
                                homeworkFacade.getHomeworkVideoSrc(homework),
                                packageChat.date_of_add
                            )
                        );
                        //homeworksForChat.Remove(homework);
                        idsOfHomeworksAlreadyAdded.Add(homework.id);
                        continue;
                    }
                }
                */
                packageChatMessageViewModels.Add(
                    new PackageChatMessageViewModel(
                        packageChat.id,
                        packageChat.message,
                        packageChat.user,
                        packageChat.teacher,
                        null,
                        packageChat.user0_teacher1,
                        packageChat.is_read,
                        null,
                        null,
                        (packageChat.date_of_add != null ? packageChat.date_of_add.Value.ToString("HH:mm:ss dd.MM.yyyy") : null)
                    )
                );
            }

            /*
            if (packageChats.Count() == 0)
            {
                foreach (Homework homework in homeworksForChat)
                {
                    if (!idsOfHomeworksAlreadyAdded.Contains(homework.id))
                    {
                        packageChatMessageViewModels.Add(
                            new PackageChatMessageViewModel(
                                0,
                                homework.comment,
                                homework.user,
                                null,
                                null,
                                0,
                                homework.statusOfSeen,
                                homework,
                                homeworkFacade.getHomeworkVideoSrc(homework),
                                homework.date_of_add
                            )
                        );
                        idsOfHomeworksAlreadyAdded.Add(homework.id);
                    }
                }
            }
            */


            return packageChatMessageViewModels;
        }

        /*
        public async Task<ListPackageChatPreviewsViewModel> getListPreview()
        {
            ConnectionUserToPrivatePackageService connectionUserToPrivatePackageService = new ConnectionUserToPrivatePackageService(_dbc);
            List<ConnectionUserToPrivatePackage> connectionsUserToPrivatePackage = await connectionUserToPrivatePackageService.listAll();

            List<PackageChatPreviewViewModel> packageChatPreviewViewModels = new List<PackageChatPreviewViewModel>();
            foreach(ConnectionUserToPrivatePackage connectionUserToPrivatePackage in connectionsUserToPrivatePackage)
            {
                packageChatPreviewViewModels.Add(
                    new PackageChatPreviewViewModel(
                        connectionUserToPrivatePackage,
                        connectionUserToPrivatePackage.user,
                        connectionUserToPrivatePackage.package,
                        null,
                        null
                    )
                );
            }

            return new ListPackageChatPreviewsViewModel(packageChatPreviewViewModels, null);
        }
        */

        public async Task<ListPackageChatPreviewsViewModel> getListPreviewByIdOfMentor(int id)
        {
            TeacherService teacherService = new TeacherService(_dbc);
            Teacher teacher = await teacherService.findById(id);
            if (teacher == null) return null;
            if (teacher.isCurator != 1) return null;

            ConnectionUserToPrivatePackageService connectionUserToPrivatePackageService = new ConnectionUserToPrivatePackageService(_dbc);
            List<ConnectionUserToPrivatePackage> connectionsUserToPrivatePackage = await connectionUserToPrivatePackageService.listAll();

            PackageChatService packageChatService = new PackageChatService(_dbc);
            HomeworkService homeworkService = new HomeworkService(_dbc);
            bool isAnyUnread = false;

            List<PackageChatPreviewViewModel> packageChatPreviewViewModels = new List<PackageChatPreviewViewModel>();
            foreach (ConnectionUserToPrivatePackage connectionUserToPrivatePackage in connectionsUserToPrivatePackage)
            {
                if(connectionUserToPrivatePackage.package.teacher != null)
                {
                    if(connectionUserToPrivatePackage.package.teacher.id == id)
                    {
                        //есть ли какие-либо не прочитанные сообщения
                        if(await packageChatService.isAnyUnreadByUserAndPackage(connectionUserToPrivatePackage.user, connectionUserToPrivatePackage.package, 0) ||
                            await homeworkService.isAnyUnreadForAdmin(connectionUserToPrivatePackage.user, connectionUserToPrivatePackage.package))
                        {
                            isAnyUnread = true;
                        } else {
                            isAnyUnread = false;
                        }

                        packageChatPreviewViewModels.Add(
                            new PackageChatPreviewViewModel(
                                connectionUserToPrivatePackage,
                                connectionUserToPrivatePackage.user,
                                connectionUserToPrivatePackage.package,
                                null,
                                isAnyUnread,
                                null
                            )
                        );
                    }
                }
            }

            return new ListPackageChatPreviewsViewModel(packageChatPreviewViewModels, teacher);
        }


        /*
        public async Task<PackageChat> sendMessageFromUser(HttpContext httpContext, int id_of_package, string message)
        {
            UserFacade userFacade = new UserFacade(_dbc);
            User user = await userFacade.getCurrentOrNull(httpContext);
            if (user == null) return null;

            PackageService packageService = new PackageService(_dbc);
            Package package = await packageService.findById(id_of_package);
            if (package == null) return null;


            PackageChatService packageChatService = new PackageChatService(_dbc);
            PackageChat packageChat = await packageChatService.add(user, null, package.teacher, package, message, 0);

            return packageChat;
        }
        */


        public async Task<JsonAnswerStatus> sendMessageFromAdminToUser(HttpContext httpContext, PackageChatNewMessageFromAdminDTO packageChatNewMessageFromAdminDTO)
        {
            AdminFacade adminFacade = new AdminFacade(_dbc);
            Admin admin = await adminFacade.getCurrentOrNull(httpContext);
            if (admin == null) return new JsonAnswerStatus("error", "no_admin");

            UserService userService = new UserService(_dbc);
            User user = await userService.findById(packageChatNewMessageFromAdminDTO.id_of_user);
            if (user == null) return new JsonAnswerStatus("error", "no_user");

            PackageService packageService = new PackageService(_dbc);
            Package package = await packageService.findById(packageChatNewMessageFromAdminDTO.id_of_package);
            if (package == null) return new JsonAnswerStatus("error", "no_package");

            PackageChatService packageChatService = new PackageChatService(_dbc);
            PackageChat packageChat = await packageChatService.add(user, admin, package.teacher, package, packageChatNewMessageFromAdminDTO.message, 1);
            if (packageChat == null) return new JsonAnswerStatus("error", "unknown");

            string teacherPosterSrc = String.Empty;
            if (package.teacher != null)
            {
                TeacherFacade teacherFacade = new TeacherFacade(_dbc);
                teacherPosterSrc = teacherFacade.getPosterSrc(package.teacher.id);
            }
            //отправка сообщения на почту пользователю
            PackageChatObserver packageChatObserver = new PackageChatObserver();
            packageChatObserver.sendMessageFromTeacherToUser(
                package.teacher,
                package,
                user,
                packageChatNewMessageFromAdminDTO.message,
                teacherPosterSrc
            );


            return new JsonAnswerStatus("success", null);
        }


        public async Task<JsonAnswerStatus> sendMessageFromUser(HttpContext httpContext, PackageChatNewMessageFromUserDTO packageChatNewMessageFromUserDTO)
        {
            UserFacade userFacade = new UserFacade(_dbc);
            User user = await userFacade.getCurrentOrNull(httpContext);
            if (user == null) return new JsonAnswerStatus("error", "no_user");

            PackageService packageService = new PackageService(_dbc);
            Package package = await packageService.findById(packageChatNewMessageFromUserDTO.id_of_package);
            if (package == null) return new JsonAnswerStatus("error", "no_package");

            PackageChatService packageChatService = new PackageChatService(_dbc);
            PackageChat packageChat = await packageChatService.add(user, null, package.teacher, package, packageChatNewMessageFromUserDTO.message, 0);
            if (packageChat == null) return new JsonAnswerStatus("error", "unknown");

            //отправка сообщения на почту наставнику
            PackageChatObserver packageChatObserver = new PackageChatObserver();
            packageChatObserver.sendMessageFromUserToTeacher(
                package.teacher,
                package,
                user,
                packageChatNewMessageFromUserDTO.message
            );

            return new JsonAnswerStatus("success", null);
        }
    }
}
