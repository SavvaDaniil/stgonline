using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace STG.ViewModels.TeacherViewModel
{
    public class TeacherPreviewViewModel
    {
        public int id { get; set; }
        public string name { get; set; }
        public string short_description { get; set; }
        public string posterSrc { get; set; }
        public string teacherLink { get; set; }
        public string posterRectSrc { get; set; }

        public TeacherPreviewViewModel(int id, string name, string short_description, string posterSrc, string teacherLink)
        {
            this.id = id;
            this.name = name;
            this.short_description = short_description;
            this.posterSrc = posterSrc;
            this.teacherLink = teacherLink;
        }

        public TeacherPreviewViewModel(int id, string name, string short_description, string posterSrc, string teacherLink, string posterRectSrc) : this(id, name, short_description, posterSrc, teacherLink)
        {
            this.posterRectSrc = posterRectSrc;
        }
    }
}
