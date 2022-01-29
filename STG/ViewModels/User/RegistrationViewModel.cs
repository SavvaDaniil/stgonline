using STG.Entities;
using STG.ViewModels.TeacherViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace STG.ViewModels.User
{
    public class RegistrationViewModel
    {
        public IEnumerable<Region> regionEnum { get; set; }
        public List<TeacherCuratorChooseViewModel> teacherCurators { get; set; }

        public RegistrationViewModel(IEnumerable<Region> regionEnum, List<TeacherCuratorChooseViewModel> teacherCurators)
        {
            this.regionEnum = regionEnum;
            this.teacherCurators = teacherCurators;
        }
    }
}
