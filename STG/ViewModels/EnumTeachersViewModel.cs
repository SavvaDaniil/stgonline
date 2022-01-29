using STG.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace STG.ViewModels.TeacherViewModel
{
    public class EnumTeachersViewModel
    {

        public EnumTeachersViewModel(IEnumerable<Teacher> enumTeachers)
        {
            this.enumTeachers = enumTeachers;
        }
        public IEnumerable<Teacher> enumTeachers { get; set; }


    }
}
