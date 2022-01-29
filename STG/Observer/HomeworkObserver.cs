using Microsoft.AspNetCore.Http;
using STG.Entities;
using STG.Factory;
using STG.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace STG.Observer
{
    public class HomeworkObserver
    {
        public async Task sendHomeworkToTeacher(Teacher teacher, Lesson lesson, Homework homework, PackageLesson packageLesson, User user, string filepath, string filename)
        {
            string title = "STG - Новая домашняя работа от пользователя: id"+user.Id + " - " + user.Username;
            UserMailHomeworkFactory userMailHomeworkFactory = new UserMailHomeworkFactory();
            MainObserver mainObserver = new MainObserver();

            string messageHtml = userMailHomeworkFactory.create(
                teacher,
                packageLesson,
                lesson,
                homework,
                user
            );

            //отправить админам сообщение
            /*
            mainObserver.sendMailToDirectorWithAttechment(
                title,
                messageHtml,
                filepath,
                filename
            );
            */


            if (teacher.email == null || teacher.email == "")
            {
                System.Diagnostics.Debug.WriteLine("teacher.email = null");
                return;
            }
            await mainObserver.threadSendWithVideoFile(
                teacher.email,
                title,
                messageHtml,
                filepath,
                filename
            );


        }



        public void sendAnswerFromTeacher(Teacher teacher, User user, PackageLesson packageLesson, string answerText, string teacherPosterSrc)
        {
            string title = "STG - Отзыв о домашнем задании от наставника: " + teacher.name;
            UserMailHomeworkFactory userMailHomeworkFactory = new UserMailHomeworkFactory();
            MainObserver mainObserver = new MainObserver();

            string messageHtml = generateHtmlAnswerFromTeacher(
                teacher,
                packageLesson.package.id,
                answerText,
                teacherPosterSrc
            );

            mainObserver.sendMailToUser(
                user.Username,
                title,
                messageHtml
            );
        }


        private string generateHtmlAnswerFromTeacher(Teacher teacher, int id_of_package, string answerText, string teacherPosterSrc)
        {
            string link = (id_of_package != 0 ? "https://stgonline.pro/package/chat/" + id_of_package : "https://stgonline.pro/packages");

            string imgSrc = (teacherPosterSrc != null ? "<img src='https://stgonline.pro/" + teacherPosterSrc+"' style='width:120px;height:120px;' /><br />" : String.Empty);

            return imgSrc +
                "<p>Вы можете свой прогресс открыв чат по <a href='" + link + "' target='_blank'> ссылке</a></p>" +
                "<hr />" +
                "<p><b>"+ teacher.name + "</b>: " + answerText + "</p>";
        }
    }
}
