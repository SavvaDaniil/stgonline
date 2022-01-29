using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace STG.ViewModels.Admin
{
    public class AdminEditViewModel
    {
        public int id { get; set; }
        public string username { get; set; }
        public int active { get; set; }
        public string position { get; set; }

        public List<(string, int, string)> accesses_to_panels { get; set; }

        public AdminEditViewModel(int id, string username, int active, string position, List<(string, int, string)> accesses_to_panels)
        {
            this.id = id;
            this.username = username;
            this.active = active;
            this.position = position;
            this.accesses_to_panels = accesses_to_panels;
        }
    }
}
