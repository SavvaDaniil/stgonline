using STG.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace STG.ViewModels.PackageChat
{
    public class PackageChatMessageViewModel
    {
        public int id { get; set; }
        public string message { get; set; }
        public STG.Entities.User user { get; set; }
        public Teacher teacher { get; set; }
        public STG.Entities.Admin admin { get; set; }
        public int user0_teacher1 { get; set; }
        public int is_read { get; set; }
        public STG.Entities.Homework homework { get; set; }
        public string homework_video_src { get; set; }
        public string date_of_add { get; set; }

        public PackageChatMessageViewModel(int id, string message, Entities.User user, Teacher teacher, Entities.Admin admin, int user0_teacher1, int is_read, Entities.Homework homework, string homework_video_src, string date_of_add)
        {
            this.id = id;
            this.message = message;
            this.user = user;
            this.teacher = teacher;
            this.admin = admin;
            this.user0_teacher1 = user0_teacher1;
            this.is_read = is_read;
            this.homework = homework;
            this.homework_video_src = homework_video_src;
            this.date_of_add = date_of_add;
        }
    }
}
