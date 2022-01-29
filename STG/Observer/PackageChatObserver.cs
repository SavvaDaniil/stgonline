using STG.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace STG.Observer
{
    public class PackageChatObserver
    {
        public void sendMessageFromUserToTeacher(Teacher teacher, Package package, User user, string answerText)
        {
            if (teacher.email == null)
            {
                System.Diagnostics.Debug.WriteLine("Нет почты учителя");
                return;
            }

            string title = "STG - Новое сообщение от пользователя: id" + user.Id + " - " + user.Username;
            MainObserver mainObserver = new MainObserver();

            string messageHtml = generateHtmlAnswerFromUserToTeacher(
                teacher,
                package,
                user,
                answerText
            );

            mainObserver.sendMailToUser(
                teacher.email,
                title,
                messageHtml
            );
        }
        

        private string generateHtmlAnswerFromUserToTeacher(Teacher teacher, Package package, User user, string answerText)
        {
            string link = (teacher != null 
                ? "https://stgonline.pro/admin/homeworks/" + teacher.id : "https://stgonline.pro/admin/mentoring");

            return "<h2>Новое сообщение от пользователя</h2>" +
                "<p>Вы можете пройти в админку по <a href='" + link + "' target='_blank'> ссылке</a>, чтобы увидеть весь процесс прохождения программы пользователем</p>" +
                "<hr />" +
                "<p>Пользователь: id" + user.Id + " " + user.Username + " - " + user.secondname + " " + user.firstname + "</p>" +
                "<p>Программа: id" + package.id + " - " + package.name + "</p>" +
                "<hr /><p><b>Сообщение</b>: " + answerText + "</p>";
        }



        public void sendMessageFromTeacherToUser(Teacher teacher, Package package, User user, string answerText, string teacherPosterSrc)
        {
            string title = "STG - Новое соообщение от наставника: " + teacher.name;
            MainObserver mainObserver = new MainObserver();

            string messageHtml = generateHtmlAnswerFromTeacher(
                teacher,
                package,
                answerText,
                teacherPosterSrc
            );

            mainObserver.sendMailToUser(
                user.Username,
                title,
                messageHtml
            );
        }

        private string generateHtmlAnswerFromTeacher(Teacher teacher, Package package, string answerText, string teacherPosterSrc)
        {
            string link = (package != null ? "https://stgonline.pro/package/chat/" + package.id : "https://stgonline.pro/packages");

            string imgSrc = (teacherPosterSrc != null ? "<img src='https://stgonline.pro/" + teacherPosterSrc + "' style='width:120px;height:120px;' /><br />" : String.Empty);

            return imgSrc +
                "<p>Вы можете свой прогресс открыв чат по <a href='" + link + "' target='_blank'> ссылке</a></p>" +
                "<hr />" +
                "<p>Программа: <b>"+ package.name + "</b></p>" +
                "<p><b>" + teacher.name + "</b>: " + answerText + "</p>";
        }
    }
}
