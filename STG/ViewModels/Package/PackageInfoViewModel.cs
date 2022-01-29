using STG.Entities;
using STG.ViewModels.TeacherViewModel;
using STG.ViewModels.Video;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace STG.ViewModels.Package
{
    public class PackageInfoViewModel
    {
        public int id { get; set; }
        public string name { get; set; }
        public int active { get; set; }
        public int active_of_package { get; set; }
        public int price { get; set; }
        public int days { get; set; }
        public string description { get; set; }
        public int statusOfChatNone0Homework1AndChat2 { get; set; }

        public int order_in_list { get; set; }
        public string posterSrc { get; set; }
        public string teaserSrc { get; set; }

        public Level level { get; set; }

        public Style style { get; set; }

        public Tariff tariff { get; set; }

        public TeacherLiteViewModel teacherLiteViewModel { get; set; }
       
        public List<PackageDayViewModel> packageDays { get; set; }

        public int countOfAllPackageDaysFinished { get; set; }
        public int countOfAllLessons { get; set; }
        public int countOfAllLessonsFinished { get; set; }
        public int procentFinished { get; set; }
        public PackageLessonLiteViewModel packageLessonLiteViewModelLastViewed { get; set; }
        public bool isAnyUnreadByUser { get; set; }

        public PackageInfoViewModel(int id, string name, int active, int active_of_package, int price, int days, string description, int statusOfChatNone0Homework1AndChat2, int order_in_list, string posterSrc, string teaserSrc, Level level, Style style, Tariff tariff, TeacherLiteViewModel teacherLiteViewModel, List<PackageDayViewModel> packageDays, int countOfAllPackageDaysFinished, int countOfAllLessons, int countOfAllLessonsFinished, int procentFinished, PackageLessonLiteViewModel packageLessonLiteViewModelLastViewed, bool isAnyUnreadByUser)
        {
            this.id = id;
            this.name = name;
            this.active = active;
            this.active_of_package = active_of_package;
            this.price = price;
            this.days = days;
            this.description = description;
            this.statusOfChatNone0Homework1AndChat2 = statusOfChatNone0Homework1AndChat2;
            this.order_in_list = order_in_list;
            this.posterSrc = posterSrc;
            this.teaserSrc = teaserSrc;
            this.level = level;
            this.style = style;
            this.tariff = tariff;
            this.teacherLiteViewModel = teacherLiteViewModel;
            this.packageDays = packageDays;
            this.countOfAllPackageDaysFinished = countOfAllPackageDaysFinished;
            this.countOfAllLessons = countOfAllLessons;
            this.countOfAllLessonsFinished = countOfAllLessonsFinished;
            this.procentFinished = procentFinished;
            this.packageLessonLiteViewModelLastViewed = packageLessonLiteViewModelLastViewed;
            this.isAnyUnreadByUser = isAnyUnreadByUser;
        }
    }
}
