using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace STG.ViewModels.Lesson
{
    public class EnumLessonLiteViewModel
    {
        public IEnumerable<LessonLiteViewModel> lessonLites { get; set; }

        public EnumLessonLiteViewModel(IEnumerable<LessonLiteViewModel> lessonLites)
        {
            this.lessonLites = lessonLites;
        }
    }
}
