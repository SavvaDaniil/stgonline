using STG.Entities;
using STG.ViewModels.Lesson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace STG.ViewModels
{
    public class LessonsViewModel
    {
        public LessonsViewModel(List<LessonLiteViewModel> lessons)
        {
            this.lessons = lessons;
        }

        public List<LessonLiteViewModel> lessons { get; set; }
        public List<Style> styles { get; set; }
        public IEnumerable<Level> levels { get; set; }
    }
}
