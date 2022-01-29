using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using STG.Models;
using STG.ViewModels.Homework;
using STG.ViewModels.Lesson;

namespace STG.ViewModels.Package
{
    public class PackageLessonLiteViewModel
    {
        public int id { get; set; }
        public int id_of_lesson { get; set; }
        public LessonViewModel lessonViewModel { get; set; }
        public int homework_status { get; set; }
        public string homework_text { get; set; }

        public int procentsViewed { get; set; }
        public int currentTime { get; set; }
        public bool isFinished { get; set; }
        public bool isAvailable { get; set; }
        public bool isHomeworkSend { get; set; }

        public HomeworkLiteViewModel homework_lite { get; set; }

        public PackageLessonLiteViewModel(int id, int id_of_lesson, int homework_status, string homework_text)
        {
            this.id = id;
            this.id_of_lesson = id_of_lesson;
            this.homework_status = homework_status;
            this.homework_text = homework_text;
        }

        public PackageLessonLiteViewModel(int id, LessonViewModel lessonViewModel, int homework_status, string homework_text, int procentsViewed, int currentTime, bool isFinished, bool isAvailable, bool isHomeworkSend, HomeworkLiteViewModel homework_lite)
        {
            this.id = id;
            this.id_of_lesson = id_of_lesson;
            this.lessonViewModel = lessonViewModel;
            this.homework_status = homework_status;
            this.homework_text = homework_text;
            this.procentsViewed = procentsViewed;
            this.currentTime = currentTime;
            this.isFinished = isFinished;
            this.isAvailable = isAvailable;
            this.isHomeworkSend = isHomeworkSend;
            this.homework_lite = homework_lite;
        }
    }
}
