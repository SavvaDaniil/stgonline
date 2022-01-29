using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace STG.ViewModels.Statement
{
    public class StatementPreviewViewModel
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

        public StatementPreviewViewModel(int id, int id_of_user, int active, string username, string secondname, string firstname, string region_name, string teachers_wanted, string date_of_add)
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
        }
    }
}
