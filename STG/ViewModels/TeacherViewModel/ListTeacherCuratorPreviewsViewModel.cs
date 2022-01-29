using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace STG.ViewModels.TeacherViewModel
{
    public class ListTeacherCuratorPreviewsViewModel
    {
        public List<TeacherCuratorPreviewViewModel> curators {get;set;}

        public ListTeacherCuratorPreviewsViewModel(List<TeacherCuratorPreviewViewModel> curators)
        {
            this.curators = curators;
        }
    }
}
