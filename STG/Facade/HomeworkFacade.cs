using Microsoft.AspNetCore.Http;
using STG.Attribute;
using STG.Component;
using STG.Data;
using STG.DTO.Homework;
using STG.Entities;
using STG.Models;
using STG.Observer;
using STG.Service;
using STG.ViewModels;
using STG.ViewModels.Homework;
using STG.ViewModels.Lesson;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace STG.Facade
{
    public class HomeworkFacade
    {
        private ApplicationDbContext _dbc;
        public HomeworkFacade(ApplicationDbContext dbc)
        {
            this._dbc = dbc;
        }
        private string uploadFolder = Directory.GetCurrentDirectory() + "\\wwwroot\\uploads\\homework\\";


        public async Task<Homework> get(User user, PackageLesson packageLesson)
        {
            HomeworkService homeworkService = new HomeworkService(_dbc);
            return await homeworkService.find(user, packageLesson);
        }

        public async Task<JsonAnswerStatus> get(HttpContext httpContext, int id_of_package_lesson)
        {
            UserFacade userFacade = new UserFacade(_dbc);
            User user = await userFacade.getCurrentOrNull(httpContext);
            if (user == null) return new JsonAnswerStatus("error", "no_user");

            PackageLessonService packageLessonService = new PackageLessonService(_dbc);
            PackageLesson packageLesson = await packageLessonService.findById(id_of_package_lesson);
            if (packageLesson == null) return new JsonAnswerStatus("error", "no package_lesson");

            Homework homework = await get(user, packageLesson);
            if(homework != null) homework.user = null;

            HomeworkFacade homeworkFacade = new HomeworkFacade(_dbc);

            return new JsonAnswerStatus(
                "success", 
                null, 
                new LessonHomeworkViewModel(
                    id_of_package_lesson,
                    packageLesson.homeworkStatus,
                    packageLesson.homeworkText,
                    packageLesson.lesson.name,
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
                )
            );
        }

        public async Task<JsonAnswerStatus> newHomework(HttpContext httpContext, HomeworkNewFromUserDTO homeworkNewDTO)
        {
            try
            {
                UserFacade userFacade = new UserFacade(_dbc);
                User user = await userFacade.getCurrentOrNull(httpContext);
                if (user == null) return new JsonAnswerStatus("error", "no user");

                PackageLessonService packageLessonService = new PackageLessonService(_dbc);
                PackageLesson packageLesson = await packageLessonService.findById(homeworkNewDTO.id_of_package_lesson);
                if (packageLesson == null) return new JsonAnswerStatus("error", "no package");

                LessonService lessonService = new LessonService(_dbc);
                Lesson lesson = await lessonService.findById(packageLesson.lesson.id);

                if (packageLesson.homeworkStatus == 1 && homeworkNewDTO.videofile == null) return new JsonAnswerStatus("error", "no file");

                Homework homework;
                HomeworkService homeworkService = new HomeworkService(_dbc);
                homework = await homeworkService.find(user, packageLesson);
                if(homework == null)
                {
                    homework = await homeworkService.add(user, packageLesson);
                }


                //загрузка видео
                string folderPath = uploadFolder + "\\" + packageLesson.id + "\\";
                string fileName = homework.id + "_" + homework.hash + "."
                    + homeworkNewDTO.videofile.FileName.Substring(homeworkNewDTO.videofile.FileName.LastIndexOf(".")).ToLower();
                string filePath = folderPath + fileName;

                homework.filename = fileName;
                await homeworkService.update(homework, homeworkNewDTO.comment);

                if (!ValidateHomeworkVideoAttribute.IsValid(homeworkNewDTO.videofile)) return new JsonAnswerStatus("error", "file_error"); ;
                if (!await uploadHomeworkVideo(homeworkNewDTO.videofile, folderPath, filePath)) return new JsonAnswerStatus("error", "can't save file");


                PackageService packageService = new PackageService(_dbc);
                Package package = await packageService.findById(packageLesson.package.id);

                if (package.teacher != null)
                {
                    HomeworkObserver homeworkObserver = new HomeworkObserver();
                    await homeworkObserver.sendHomeworkToTeacher(
                        package.teacher,
                        packageLesson.lesson,
                        homework,
                        packageLesson,
                        user,
                        filePath,
                        fileName
                    );
                }

                return new JsonAnswerStatus("success", null);
            } catch(Exception ex)
            {
                LoggerComponent.writeToLogError("Ошибка newHomework: " + ex.Message.ToString());
                return new JsonAnswerStatus("error", "unknown");
            } finally
            {

            }
            return new JsonAnswerStatus("success", null);
        }

        public async Task<JsonAnswerStatus> newHomeworkByChunk(HttpContext httpContext, HomeworkNewFromUserByChunkDTO homeworkNewFromUserByChunkDTO)
        {
            try
            {
                if (homeworkNewFromUserByChunkDTO.videofile == null) return new JsonAnswerStatus("error", "no file");

                UserFacade userFacade = new UserFacade(_dbc);
                User user = await userFacade.getCurrentOrNull(httpContext);
                if (user == null) return new JsonAnswerStatus("error", "no user");

                PackageLessonService packageLessonService = new PackageLessonService(_dbc);
                PackageLesson packageLesson = await packageLessonService.findById(homeworkNewFromUserByChunkDTO.id_of_package_lesson);
                if (packageLesson == null) return new JsonAnswerStatus("error", "no package");

                //LessonService lessonService = new LessonService(_dbc);
                //Lesson lesson = await lessonService.findById(packageLesson.lesson.id);

                Homework homework;
                HomeworkService homeworkService = new HomeworkService(_dbc);
                homework = await homeworkService.find(user, packageLesson);
                if (homework == null)
                {
                    homework = await homeworkService.add(user, packageLesson);
                }

                //обновить статус, что домашнее задание не загружено
                if (homeworkNewFromUserByChunkDTO.index_of_chunk == 1)
                {
                    homework = await homeworkService.updateRenew(homework);
                }

                if (homework == null) return new JsonAnswerStatus("error", "no homework");

                //загрузка видео
                string folderPath = uploadFolder + "\\" + packageLesson.id + "\\";
                //string fileName = homework.id + "_" + homework.hash + ".mp4";
                string fileName = homework.id + "_" + homework.hash;
                string filePath = folderPath + fileName;

                if (!uploadHomeworkVideoByChunk(homeworkNewFromUserByChunkDTO.videofile, homeworkNewFromUserByChunkDTO.index_of_chunk, folderPath, filePath)) return new JsonAnswerStatus("error", "can't save file");

            } catch(Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Ошибка newHomeworkByChunk: " + ex.Message);
                return new JsonAnswerStatus("error", null);
            }
            finally
            {

            }
            return new JsonAnswerStatus("success", null);
        }

        private bool uploadHomeworkVideoByChunk(IFormFile videoFile, int index_of_chunk, string folderPath, string filePath)
        {
            if (!ValidateHomeworkVideoAttribute.IsValidChunk(videoFile))
            {
                System.Diagnostics.Debug.WriteLine("Ошибка ValidateHomeworkVideoAttribute.IsValidChunk");
                return false;
            }
            if (!Directory.Exists(folderPath))Directory.CreateDirectory(folderPath);

            filePath = filePath + ".mp4";
            if (File.Exists(filePath) && index_of_chunk == 1) File.Delete(filePath);

            if (!File.Exists(filePath))
            {
                try
                {
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        videoFile.CopyTo(fileStream);
                        fileStream.Dispose();
                    }
                    return true;
                }
                catch
                {
                    System.Diagnostics.Debug.WriteLine("Ошибка VuploadHomeworkVideoByChunk при попытке создать файл");
                    return false;
                }
            } else
            {
                //FileStream fs1 = null;
                //FileStream fs2 = null;
                string fileNameTemp = folderPath + "tmp";

                if (File.Exists(fileNameTemp)) File.Delete(fileNameTemp);

                try
                {
                    using (FileStream fs1 = System.IO.File.Open(filePath, FileMode.Append, FileAccess.Write))
                    {
                        using (FileStream fs2 = new FileStream(fileNameTemp, FileMode.Create))
                        {
                            //fs2 = System.IO.File.Open(chunk2, FileMode.Open);
                            videoFile.CopyTo(fs2);
                            System.Diagnostics.Debug.WriteLine("fs2.Length = " + fs2.Length);
                            if (fs2.Length == 0)
                            {
                                System.Diagnostics.Debug.WriteLine("Ошибка fs2.Length = " + fs2.Length);
                                return false;
                            }
                            //byte[] fs2Content = new byte[fs2.Length];
                            //fs2.Read(fs2Content, 0, (int)fs2.Length);
                            //fs1.Write(fs2Content, 0, (int)fs2.Length);

                            fs2.Dispose();
                            if (fs2 != null) fs2.Close();

                            using (FileStream fs3 = System.IO.File.Open(fileNameTemp, FileMode.Open))
                            {
                                System.Diagnostics.Debug.WriteLine("fs3.Length = " + fs3.Length);
                                if (fs3.Length == 0)
                                {
                                    System.Diagnostics.Debug.WriteLine("Ошибка fs3.Length = " + fs3.Length);
                                    return false;
                                }
                                byte[] fs2Content = new byte[fs3.Length];
                                fs3.Read(fs2Content, 0, (int)fs3.Length);
                                fs1.Write(fs2Content, 0, (int)fs3.Length);

                                fs3.Dispose();
                                if (fs3 != null) fs3.Close();
                                File.Delete(fileNameTemp);
                            }
                        }
                        fs1.Dispose();
                        if (fs1 != null) fs1.Close();
                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine("Ошибка при попытке сохранить файл: " + ex.Message + " : " + ex.StackTrace);
                    return false;
                }
                finally
                {
                    //fs1.Dispose();
                    //fs2.Dispose();

                    //if (fs1 != null) fs1.Close();
                    //if (fs2 != null) fs2.Close();
                    // System.IO.File.Delete(chunk2);

                }
                return true;
            }


        }


        public async Task<JsonAnswerStatus> newHomeworkOnlyComment(HttpContext httpContext, HomeworkNewFromUserDTO homeworkNewDTO)
        {
            UserFacade userFacade = new UserFacade(_dbc);
            User user = await userFacade.getCurrentOrNull(httpContext);
            if (user == null) return new JsonAnswerStatus("error", "no user");

            PackageLessonService packageLessonService = new PackageLessonService(_dbc);
            PackageLesson packageLesson = await packageLessonService.findById(homeworkNewDTO.id_of_package_lesson);
            if (packageLesson == null) return new JsonAnswerStatus("error", "no package");

            LessonService lessonService = new LessonService(_dbc);
            Lesson lesson = await lessonService.findById(packageLesson.lesson.id);

            Homework homework;
            HomeworkService homeworkService = new HomeworkService(_dbc);
            homework = await homeworkService.find(user, packageLesson);
            if (homework == null)
            {
                homework = await homeworkService.add(user, packageLesson);
            }

            string folderPath = uploadFolder + "\\" + packageLesson.id + "\\";
            string fileName = homework.id + "_" + homework.hash + ".mp4";
            string filePath = folderPath + fileName;

            homework.filename = fileName;
            await homeworkService.update(homework, homeworkNewDTO.comment);

            return new JsonAnswerStatus("success", null);
        }



        public async Task<JsonAnswerStatus> newAnswerFromAdmin(HttpContext httpContext, HomeworkAnswerFromAdminDTO homeworkAnswerFromAdminDTO)
        {

            HomeworkService homeworkService = new HomeworkService(_dbc);
            Homework homework = await homeworkService.findById(homeworkAnswerFromAdminDTO.id_of_homework);
            if (homework == null) return new JsonAnswerStatus("error", "no homework");

            await homeworkService.newAnswerFromAdmin(homework, homeworkAnswerFromAdminDTO.answer);

            //отправить пользователю сообщение на почту об ответе
            PackageLessonService packageLessonService = new PackageLessonService(_dbc);
            PackageLesson packageLesson = await packageLessonService.findById(homework.packageLesson.id);
            PackageService packageService = new PackageService(_dbc);
            Package package = await packageService.findById(packageLesson.package.id);
            if (package.teacher != null)
            {
                TeacherFacade teacherFacade = new TeacherFacade(_dbc);
                string posterSrc = teacherFacade.getPosterSrc(package.teacher.id);

                HomeworkObserver homeworkObserver = new HomeworkObserver();
                homeworkObserver.sendAnswerFromTeacher(
                    package.teacher,
                    homework.user,
                    packageLesson,
                    homeworkAnswerFromAdminDTO.answer,
                    posterSrc
                );
            }

            return new JsonAnswerStatus("success", null);
        }

        public async Task<bool> setSeenByAdmin(HttpContext httpContext, HomeworkIdDTO homeworkIdDTO)
        {
            HomeworkService homeworkService = new HomeworkService(_dbc);
            Homework homework = await homeworkService.findById(homeworkIdDTO.id);
            if (homework == null) return false;
            if (homework.statusOfSeen == 1) return true;
            return await homeworkService.setSeenByAdmin(homework);
        }

        public async Task<bool> setSeenByUser(HttpContext httpContext, HomeworkIdDTO homeworkIdDTO)
        {
            HomeworkService homeworkService = new HomeworkService(_dbc);
            Homework homework = await homeworkService.findById(homeworkIdDTO.id);
            if (homework == null) return false;
            if (homework.status_of_seen_of_message_from_teacher == 1) return true;
            return await homeworkService.setSeenByUser(homework);
        }


        //загрузка файла
        private async Task<bool> uploadHomeworkVideo(IFormFile videoFile, string folderPath, string filePath)
        {
            if (!ValidateHomeworkVideoAttribute.IsValid(videoFile)) return false;

            //ValidateHomeworkVideoAttribute
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            try
            {
                string pathwithfileName = filePath;
                using (var fileStream = new FileStream(pathwithfileName, FileMode.Create))
                {
                    await videoFile.CopyToAsync(fileStream);
                    fileStream.Dispose();
                }
                return true;
            } catch
            {
                return false;
            }

        }

        public string getHomeworkVideoSrc(Homework homework)
        {
            if (homework.packageLesson == null || homework.filename == null) return null;
            return "/uploads/homework/" + homework.packageLesson.id + "/" + homework.filename;
        }
    }
}
