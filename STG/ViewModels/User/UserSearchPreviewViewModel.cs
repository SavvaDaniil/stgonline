using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace STG.ViewModels.User
{
    public class UserSearchPreviewViewModel
    {
        public int id { get; set; }
        public string username { get; set; }
        public string firstname { get; set; }
        public string secondname { get; set; }
        public string instagram { get; set; }
        public string region_name { get; set; }

        public UserSearchPreviewViewModel(int id, string username, string firstname, string secondname, string instagram, string region_name)
        {
            this.id = id;
            this.username = username;
            this.firstname = firstname;
            this.secondname = secondname;
            this.instagram = instagram;
            this.region_name = region_name;
        }
    }
}
