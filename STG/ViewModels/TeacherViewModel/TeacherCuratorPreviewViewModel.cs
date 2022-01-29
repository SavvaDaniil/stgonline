using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace STG.ViewModels.TeacherViewModel
{
    public class TeacherCuratorPreviewViewModel
    {
        public int id { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public string posterSrc { get; set; }
        public int countOfStudents { get; set; }
        public int countUnreadHomeworks { get; set; }
        public int countUnreadPackageChats { get; set; }

        public string posterRectSrc { get; set; }

        public TeacherCuratorPreviewViewModel(int id, string name, string email, string posterSrc)
        {
            this.id = id;
            this.name = name;
            this.email = email;
            this.posterSrc = posterSrc;
        }

        public TeacherCuratorPreviewViewModel(int id, string name, string email, string posterSrc, int countUnreadHomeworks, int countUnreadPackageChats) : this(id, name, email, posterSrc)
        {
            this.countUnreadHomeworks = countUnreadHomeworks;
            this.countUnreadPackageChats = countUnreadPackageChats;
        }


    }
}
