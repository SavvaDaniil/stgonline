using STG.Entities;
using STG.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace STG.Factory
{
    public class UserMailHomeworkFactory
    {

        public string create(Teacher teacher, PackageLesson packageLesson, Lesson lesson, Homework homework, User user)
        {
            string link = (teacher != null ? "https://stgonline.pro/admin/homeworks/" + teacher.id : "https://stgonline.pro/admin/homeworks");

            return "<h2>Новая домашняя работа</h2>" +
                "<p>Вы можете пройти в админку по <a href='"+link+"' target='_blank'> ссылке</a>, чтобы увидеть весь процесс прохождения программы пользователем</p>" +
                "<hr />" +
                "<p>Пользователь: id"+ user.Id + " "+ user.Username + " - " + user.secondname + " " + user.firstname + "</p>" +
                "<p>Урок: id" + lesson.id + " - " + lesson.name + "</p>" +
                "<p>Преподаватель: id" + teacher.id + " - " + teacher.name + " " + teacher.email + "</p>" +
                "<p>Программа: id" + packageLesson.package.id + " - " + packageLesson.package.name + "</p>" +
                "<p>Идентификатор урока пакета: " + packageLesson.id + "</p>" +
                "<p>Идентификатор домашнего задания: " + homework.id + "</p>" +
                "<hr /><p>Комментарий от пользователя: " + homework.comment + "</p>";
        }
    }
}
