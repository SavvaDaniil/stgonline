using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace STG.ViewModels.Homework
{
    public class HomeworkLiteViewModel
    {
        public int id { get; set; }
        public string comment { get; set; }
        public string filenameSrc { get; set; }
        public int status { get; set; }
        public int statusOfSeen { get; set; }
        public int statusOfUpload { get; set; }
        public string date_of_add { get; set; }
        public string date_of_update { get; set; }

        public string answer_from_teacher { get; set; }
        public string date_of_update_of_teacher { get; set; }
        public string date_of_seen_by_admin { get; set; }
        public int status_of_seen_of_message_from_teacher { get; set; }

        public HomeworkLiteViewModel(int id, string comment, string filenameSrc, int status, int statusOfSeen, int statusOfUpload, string date_of_add, string date_of_update, string answer_from_teacher, string date_of_update_of_teacher, string date_of_seen_by_admin, int status_of_seen_of_message_from_teacher)
        {
            this.id = id;
            this.comment = comment;
            this.filenameSrc = filenameSrc;
            this.status = status;
            this.statusOfSeen = statusOfSeen;
            this.statusOfUpload = statusOfUpload;
            this.date_of_add = date_of_add;
            this.date_of_update = date_of_update;
            this.answer_from_teacher = answer_from_teacher;
            this.date_of_update_of_teacher = date_of_update_of_teacher;
            this.date_of_seen_by_admin = date_of_seen_by_admin;
            this.status_of_seen_of_message_from_teacher = status_of_seen_of_message_from_teacher;
        }
    }
}
