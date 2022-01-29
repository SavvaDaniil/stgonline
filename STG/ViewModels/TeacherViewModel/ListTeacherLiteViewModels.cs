using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace STG.ViewModels.TeacherViewModel
{
    public class ListTeacherLiteViewModels
    {
        public List<TeacherPreviewViewModel> teachers { get; set; }

        public ListTeacherLiteViewModels(List<TeacherPreviewViewModel> teachers)
        {
            this.teachers = teachers;
        }
    }
}
