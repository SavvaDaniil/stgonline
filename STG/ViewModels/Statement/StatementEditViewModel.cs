using STG.Entities;
using STG.ViewModels.TeacherViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace STG.ViewModels.Statement
{
    public class StatementEditViewModel
    {
        public int id { get; set; }
        public int id_of_user { get; set; }
        public int active { get; set; }
        public string username { get; set; }
        public string secondname { get; set; }
        public string firstname { get; set; }
        public string region_name { get; set; }
        public string teachers_wanted { get; set; }
        public string date_of_add { get; set; }
        public string date_of_active { get; set; }
        public int is_need_curator { get; set; }

        public int experience { get; set; }
        public int expectations { get; set; }
        public int expected_time_for_lessons { get; set; }
        public string idols { get; set; }
        public string link1 { get; set; }
        public string link2 { get; set; }
        public string link3 { get; set; }

        public Teacher teacher { get; set; }
        public ListTeacherCuratorPreviewsViewModel curators { get; set; }
        public bool is_super_user { get; set; }

        public StatementEditViewModel(int id, int id_of_user, int active, string username, string secondname, string firstname, string region_name, string teachers_wanted, string date_of_add, string date_of_active, int is_need_curator, int experience, int expectations, int expected_time_for_lessons, string idols, string link1, string link2, string link3, Teacher teacher, ListTeacherCuratorPreviewsViewModel curators, bool is_super_user)
        {
            this.id = id;
            this.id_of_user = id_of_user;
            this.active = active;
            this.username = username;
            this.secondname = secondname;
            this.firstname = firstname;
            this.region_name = region_name;
            this.teachers_wanted = teachers_wanted;
            this.date_of_add = date_of_add;
            this.date_of_active = date_of_active;
            this.is_need_curator = is_need_curator;
            this.experience = experience;
            this.expectations = expectations;
            this.expected_time_for_lessons = expected_time_for_lessons;
            this.idols = idols;
            this.link1 = link1;
            this.link2 = link2;
            this.link3 = link3;
            this.teacher = teacher;
            this.curators = curators;
            this.is_super_user = is_super_user;
        }
    }
}
