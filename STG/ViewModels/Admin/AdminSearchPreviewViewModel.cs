using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace STG.ViewModels.Admin
{
    public class AdminSearchPreviewViewModel
    {
        public int id { get; set; }
        public string username { get; set; }
        public string position { get; set; }
        public int active { get; set; }

        public AdminSearchPreviewViewModel(int id, string username, string position, int active)
        {
            this.id = id;
            this.username = username;
            this.position = position;
            this.active = active;
        }
    }
}
