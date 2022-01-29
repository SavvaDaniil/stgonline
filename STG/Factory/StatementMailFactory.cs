using STG.Entities;
using STG.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STG.Factory
{
    public class StatementMailFactory
    {
        private Dictionary<int, string> experience = new Dictionary<int, string>
        {
            { 0, ""},
            { 1, "В первый раз (никогда раньше не танцевали)"},
            { 2, "Начинающий (брали несколько классов и понимаете основы)"},
            { 3, "Продолжающие (тацуете регулярно, хочу углубить свои знания)"},
            { 4, "Профи (у вас большой опыт, просто хотите что-то новое)"}
        };
        private Dictionary<int, string> expectations = new Dictionary<int, string>
        {
            { 0, ""},
            { 1, "Хочу изучить/повторить базу"},
            { 2, "Хочу научиться фристайлить"},
            { 3, "Хочу выучить хореографии"},
            { 4, "Хочу стать увереннее на танцевальных площадках"},
            { 5, "Хочу похудеть, улучшить здоровье"}
        };
        private Dictionary<int, string> expected_time_for_lessons = new Dictionary<int, string>
        {
            { 0, ""},
            { 1, "15-45 минут"},
            { 2, "Больше 45 минут"},
            { 3, "Готов тренироваться весь день"}
        };
        private Dictionary<int, string> is_need_curator = new Dictionary<int, string>
        {
            { 0, ""},
            { 1, "Тариф “Базовый”"},
            { 2, "Тариф “Продвинутый”"},
            { 3, "Тариф “Углубленный”"}
        };

        public string create(User user, Statement statement, List<Teacher> curators)
        {
            StringBuilder choosenTeacher = new StringBuilder("");
            foreach(Teacher teacher in curators)
            {
                if (!choosenTeacher.Equals("")) choosenTeacher.Append(", ");
                choosenTeacher.Append(teacher.name + "(" + teacher.id + ")");
            }


            return "" +
                "<h2>Заявка на наставничество</h2>" +
                "<p>Дата заявки: " + statement.date_of_payed + "</p>" +
                "<p>Пользователь: " + statement.user.Id + "  " + statement.user.Username + "</p>" +
                "<p><b>instagram</b>: " + statement.user.instagram + "</p>" +
                "<p><b>Регион</b>: " + (user.region != null ? user.region.name : "") + "</p>" +
                "<hr />" +
                "<p><b>Ваш танцевальный стаж</b>: " + experience[statement.experience] + "</p>" +
                "<p><b>Что вы ожидаете или хотите получить от занятий на данной платформе</b>: " + expectations[statement.expectations] + "</p>" +
                "<p><b>Сколько вы сможете уделять времени на занятия в день</b>: " + expected_time_for_lessons[statement.expected_time_for_lessons] + "</p>" +
                "<p><b>Кто ваши кумиры в танцевальном мире</b>: " + statement.idols + "</p>" +
                "<p><b>Тариф</b>:" + is_need_curator[statement.is_need_curator] + "</p>" +
                "<p><b>Ссылка на видео 1</b>: " + statement.link1 + "</p>" +
                "<p><b>Ссылка на видео 2</b>: " + statement.link2 + "</p>" +
                "<p><b>Ссылка на видео 3</b>: " + statement.link3 + "</p>" +
                "<hr /><p><b>Желаемые наставники: </b>: " + choosenTeacher + "</p>:";

        }
    }
}
