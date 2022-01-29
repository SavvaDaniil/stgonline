using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace STG.ViewModels.Admin
{
    public class AdminSearchViewModel
    {
        public List<AdminSearchPreviewViewModel> admins { get; set; }
        public int count { get; set; }

        public AdminSearchViewModel(List<AdminSearchPreviewViewModel> admins, int count)
        {
            this.admins = admins;
            this.count = count;
        }
    }
}
