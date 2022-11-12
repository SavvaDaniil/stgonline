using Microsoft.AspNetCore.Http;
using STG.Attribute;
using STG.Component;
using STG.Data;
using STG.DTO;
using STG.DTO.Teacher;
using STG.Entities;
using STG.Service;
using STG.ViewModels;
using STG.ViewModels.Lesson;
using STG.ViewModels.TeacherViewModel;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace STG.Facade
{
    public class TeacherFacade
    {
        private ApplicationDbContext _dbc;
        public TeacherFacade(ApplicationDbContext dbc)
        {
            this._dbc = dbc;
        }

        private string defaultPosterSrc = "/images/teacher-default.jpg";

        public TeacherLiteViewModel get(int id)
        {
            TeacherService teacherService = new TeacherService(this._dbc);
            Teacher teacher = teacherService.findById(id);
            if (teacher == null) return null;

            return new TeacherLiteViewModel(
                teacher.id,
                teacher.name,
                teacher.email,
                teacher.instagram,
                teacher.shortDescription,
                teacher.description,
                teacher.active,
                teacher.isCurator,
                teacher.mentor_bio,
                teacher.mentor_awards,
                teacher.priceCurator,
                teacher.priceTariff1,
                teacher.priceTariff2,
                teacher.priceTariff3,
                getPosterSrc(teacher.id),
                getPosterRectSrc(teacher.id)
            );
        }

        public JsonAnswerStatus getModal(HttpContext httpContext, TeacherIdDTO teacherIdDTO)
        {
            TeacherService teacherService = new TeacherService(this._dbc);
            Teacher teacher = teacherService.findById(teacherIdDTO.id);
            if (teacher == null) return null;

            LessonFacade lessonFacade = new LessonFacade(_dbc);
            List<LessonLiteViewModel> lessonsOfTeacher = lessonFacade.getAllActiveByFilter(
                httpContext,
                0,
                0,
                teacher.id,
                null,
                0,
                0,
                6
            );

            return new JsonAnswerStatus(
                "success",
                null,
                new TeacherIndexModalViewModel(
                    teacher.id,
                    teacher.name,
                    teacher.instagram,
                    getPosterSrc(teacher.id),
                    teacher.description,
                    lessonsOfTeacher
                )
            );
        }


        public List<TeacherPreviewViewModel> listAllActive()
        {
            TeacherService teacherService = new TeacherService(this._dbc);
            IEnumerable<Teacher> teachers = teacherService.listAllActive();

            List<TeacherPreviewViewModel> teacherLiteViewModels = new List<TeacherPreviewViewModel>();

            string posterSrc;

            foreach(Teacher teacher in teachers)
            {
                posterSrc = getPosterSrc(teacher.id);
                if (posterSrc == null) posterSrc = defaultPosterSrc;

                teacherLiteViewModels.Add(
                    new TeacherPreviewViewModel(
                        teacher.id,
                        teacher.name,
                        teacher.shortDescription,
                        posterSrc,
                        teacher.id.ToString(),
                        getPosterRectSrc(teacher.id)
                    )    
                );
            }
            teacherService = null;
            teachers = null;
            posterSrc = null;

            return teacherLiteViewModels;
        }

        public List<TeacherLiteIndexViewModel> listAllLiteForIndexActive()
        {
            TeacherService teacherService = new TeacherService(this._dbc);
            List<Teacher> teachers = teacherService.listAllActive();

            List<TeacherLiteIndexViewModel> teacherLiteViewModels = new List<TeacherLiteIndexViewModel>();

            string posterSrc;
            string specialSideForClassName;

            foreach (Teacher teacher in teachers)
            {
                posterSrc = getPosterSrc(teacher.id);
                if (posterSrc == null) posterSrc = defaultPosterSrc;

                specialSideForClassName = null;
                if (teachers.Count() / 2 % 2 != 0 && teachers.Count() > 2)
                {
                    if(teachers[teachers.Count() - 2] == teacher)
                    {
                        specialSideForClassName = "left";
                    }
                    if(teacher == teachers.Last())
                    {
                        specialSideForClassName = "right";
                    }
                }

                teacherLiteViewModels.Add(
                    new TeacherLiteIndexViewModel(
                        teacher.id,
                        teacher.name,
                        teacher.description,
                        posterSrc,
                        specialSideForClassName
                    )    
                );
            }
            teacherService = null;
            teachers = null;
            posterSrc = null;

            return teacherLiteViewModels;
        }

        public List<TeacherCuratorChooseViewModel> listAllCurator()
        {
            TeacherService teacherService = new TeacherService(this._dbc);
            IEnumerable<Teacher> teachers = teacherService.listAllActiveCurator();
            List<TeacherCuratorChooseViewModel> teacherLiteViewModels = new List<TeacherCuratorChooseViewModel>();

            string posterSrc;
            string posterRectSrc;
            foreach (Teacher teacher in teachers)
            {
                posterSrc = getPosterSrc(teacher.id);
                posterRectSrc = getPosterRectSrc(teacher.id);
                if (posterSrc == null) posterSrc = defaultPosterSrc;

                teacherLiteViewModels.Add(
                    new TeacherCuratorChooseViewModel(
                        teacher.id,
                        teacher.name,
                        teacher.priceCurator,
                        teacher.shortDescription,
                        posterSrc,
                        posterRectSrc,
                        teacher.priceTariff1,
                        teacher.priceTariff2,
                        teacher.priceTariff3
                    )
                );
            }
            teacherService = null;
            teachers = null;
            posterSrc = null;

            return teacherLiteViewModels;
        }

        public ListTeacherCuratorPreviewsViewModel listAllCuratorForMentoring(bool isOnlyPrivateCourses = true)
        {
            TeacherService teacherService = new TeacherService(this._dbc);
            IEnumerable<Teacher> teachers = (isOnlyPrivateCourses ? teacherService.listAllActiveCurator() : teacherService.listAll());
            List<TeacherCuratorPreviewViewModel> curators = new List<TeacherCuratorPreviewViewModel>();

            HomeworkService homeworkService = new HomeworkService(_dbc);
            PackageChatService packageChatService = new PackageChatService(_dbc);
            int countUnreadHomeworks = 0;
            int countUnreadPackageChats = 0;

            string posterSrc;
            foreach (Teacher teacher in teachers)
            {
                posterSrc = getPosterSrc(teacher.id);
                if (posterSrc == null) posterSrc = defaultPosterSrc;

                //получение количества непрочитанных домашних заданий
                countUnreadHomeworks = homeworkService.getCountNotReadedByTeacher(teacher);
                //получение количества непрочитанных сообщений
                countUnreadPackageChats = packageChatService.getCountNotReadedByTeacher(teacher);

                curators.Add(
                    new TeacherCuratorPreviewViewModel(
                        teacher.id,
                        teacher.name,
                        teacher.email,
                        posterSrc,
                        countUnreadHomeworks,
                        countUnreadPackageChats
                    )
                );
            }
            teacherService = null;
            teachers = null;
            posterSrc = null;

            return new ListTeacherCuratorPreviewsViewModel(curators);
        }


        public JsonAnswerStatus update(TeacherDTO teacherDTO)
        {
            if (teacherDTO.isPosterDelete == 1)
            {
                deletePoster(teacherDTO.id);
            }
            if (teacherDTO.avatarFile != null)
            {
                if (!ValidateImageAttribute.IsValid(teacherDTO.avatarFile))
                {
                    return new JsonAnswerStatus("error", "wrong_image");
                }
                if (!(uploadPoster(teacherDTO.avatarFile, teacherDTO.id.ToString())))
                {
                    return new JsonAnswerStatus("error", "unknown_error");
                }
            }

            TeacherService teacherService = new TeacherService(this._dbc);
            teacherService.save(teacherDTO);

            return new JsonAnswerStatus("success", null);
        }


        private void deletePoster(int id)
        {
            string path = Directory.GetCurrentDirectory();
            string filePath = path + "\\wwwroot\\uploads\\teacher\\" + id.ToString() + ".jpg";
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }

        public string getPosterSrc(int id)
        {
            string path = Directory.GetCurrentDirectory();
            string uploadsForFiles = path + "\\wwwroot\\uploads\\teacher\\" + id.ToString() + ".jpg";
            if (File.Exists(uploadsForFiles))
            {
                return "/uploads/teacher/" + id.ToString() + ".jpg";
            }
            return null;
        }

        public string getPosterRectSrc(int id)
        {
            string path = Directory.GetCurrentDirectory();
            string uploadsForFiles = path + "\\wwwroot\\uploads\\teacher\\" + id.ToString() + "_rect.png";
            if (File.Exists(uploadsForFiles))
            {
                return "/uploads/teacher/" + id.ToString() + "_rect.png";
            }
            return null;
        }


        private bool uploadPoster(IFormFile file, string nameOfFile)
        {
            string path = Directory.GetCurrentDirectory();
            string uploadsForFiles = path + "\\wwwroot\\uploads\\teacher";
            if (!Directory.Exists(uploadsForFiles))
            {
                Directory.CreateDirectory(uploadsForFiles);
            }


            string pathwithfileName = uploadsForFiles + "\\" + nameOfFile + ".jpg";
            string pathwithTmpName = uploadsForFiles + "\\tmp.png";
            using (var fileStream = new FileStream(pathwithTmpName, FileMode.Create))
            {
                file.CopyTo(fileStream);

                fileStream.Dispose();
            }

            ResizeImageComponent resizeImageComponent = new ResizeImageComponent();
            //resizeImageComponent.ResizeTmpImageAndSaveFinally_9_16(pathwithTmpName, pathwithfileName);
            resizeImageComponent.ResizeTmpImageAndSaveFinally_box(pathwithTmpName, pathwithfileName);
            resizeImageComponent = null;

            if (File.Exists(pathwithTmpName))File.Delete(pathwithTmpName);



            return true;
        }

        public bool changeOrder(TeacherOrderDTO teacherOrderDTO)
        {
            TeacherService teacherService = new TeacherService(_dbc);
            bool answer = teacherService.changeOrder(teacherOrderDTO);
            teacherService = null;
            return answer;
        }

        private Bitmap ResizeImage(Image image, int width, int height)
        {
            
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);
            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                    wrapMode.Dispose();
                }

                graphics.Dispose();
            }

            return destImage;
        }


        private string getFileExtension(IFormFile file)
        {
            return (file != null) ?
                file.FileName.Substring(file.FileName.LastIndexOf('.')).ToLower() :
                string.Empty;
        }
    }
}
