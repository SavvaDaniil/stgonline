using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace STG.ViewModels.Lesson
{
    public class EnumLessonMicroViewModel
    {

        public IEnumerable<LessonMicroViewModel> lessons { get; set; }

        public EnumLessonMicroViewModel(IEnumerable<LessonMicroViewModel> lessons)
        {
            this.lessons = lessons;
        }
    }
}
