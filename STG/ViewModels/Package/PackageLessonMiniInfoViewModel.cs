using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace STG.ViewModels.Package
{
    public class PackageLessonMiniInfoViewModel
    {
        public int id { get; set; }
        public int homeworkStatus { get; set; }
        public string homeworkText { get; set; }

        public PackageLessonMiniInfoViewModel(int id, int homeworkStatus, string homeworkText)
        {
            this.id = id;
            this.homeworkStatus = homeworkStatus;
            this.homeworkText = homeworkText;
        }
    }
}
