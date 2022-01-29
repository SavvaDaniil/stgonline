using STG.ViewModels.Lesson;
using STG.ViewModels.Package;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace STG.ViewModels.Package
{
    public class PackageDayViewModel
    {
        public int id { get; set; }
        public int number { get; set; }
        public string name { get; set; }
        public List<PackageLessonLiteViewModel> package_lesson_list { get; set; }
        public bool isFinished { get; set; }

        public PackageDayViewModel(int id, int number, string name, List<PackageLessonLiteViewModel> package_lesson_list, bool isFinished)
        {
            this.id = id;
            this.number = number;
            this.name = name;
            this.package_lesson_list = package_lesson_list;
            this.isFinished = isFinished;
        }
    }
}
