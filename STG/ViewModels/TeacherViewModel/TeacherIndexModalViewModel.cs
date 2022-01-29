using STG.ViewModels.Lesson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace STG.ViewModels.TeacherViewModel
{
    public class TeacherIndexModalViewModel
    {
        public int id { get; set; }
        public string name { get; set; }
        public string instagram { get; set; }
        public string posterSrc { get; set; }
        public string description { get; set; }
        public List<LessonLiteViewModel> lessons { get; set; }

        public TeacherIndexModalViewModel(int id, string name, string instagram, string posterSrc, string description, List<LessonLiteViewModel> lessons)
        {
            this.id = id;
            this.name = name;
            this.instagram = instagram;
            this.posterSrc = posterSrc;
            this.description = description;
            this.lessons = lessons;
        }
    }
}
