using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace STG.ViewModels.Admin
{
    public class AdminProfileViewModel
    {
        public int id { get; set; }
        public string username { get; set; }
        public string position { get; set; }

        public AdminProfileViewModel(int id, string username, string position)
        {
            this.id = id;
            this.username = username;
            this.position = position;
        }
    }
}
