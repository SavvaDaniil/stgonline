using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace STG.ViewModels
{
    public class UserProfileViewModel
    {
        public string firstname { get; set; }

        public string secondname { get; set; }

        public string username { get; set; }

        public int sex { get; set; }

        public string instagram { get; set; }

        public string date_of_birthday { get; set; }

        public int prolongation { get; set; }

        public UserProfileViewModel(string firstname, string secondname, string username, int sex, string instagram, string date_of_birthday, int prolongation)
        {
            this.firstname = firstname;
            this.secondname = secondname;
            this.username = username;
            this.sex = sex;
            this.instagram = instagram;
            this.date_of_birthday = date_of_birthday;
            this.prolongation = prolongation;
        }
    }
}
