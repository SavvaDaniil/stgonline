using STG.Entities;
using STG.Models;
using STG.ViewModels.Homework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace STG.ViewModels.Lesson
{
    public class LessonHomeworkViewModel
    {
        public int id_of_package_lesson { get; set; }
        public int homeworkStatus { get; set; }
        public string homeworkText { get; set; }
        public string name_of_lesson { get; set; }

        public HomeworkLiteViewModel homework { get; set; }

        public LessonHomeworkViewModel(int id_of_package_lesson, int homeworkStatus, string homeworkText, string name_of_lesson, HomeworkLiteViewModel homework)
        {
            this.id_of_package_lesson = id_of_package_lesson;
            this.homeworkStatus = homeworkStatus;
            this.homeworkText = homeworkText;
            this.name_of_lesson = name_of_lesson;
            this.homework = homework;
        }
    }
}
