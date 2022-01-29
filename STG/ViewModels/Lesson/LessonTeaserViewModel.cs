using STG.ViewModels.Video;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace STG.ViewModels.Lesson
{
    public class LessonTeaserViewModel
    {
        public int id { get; set; }
        public string teaserSrc { get; set; }

        public LessonTeaserViewModel(int id)
        {
            this.id = id;
        }

        public LessonTeaserViewModel(int id, string teaserSrc) : this(id)
        {
            this.teaserSrc = teaserSrc;
        }
    }
}
