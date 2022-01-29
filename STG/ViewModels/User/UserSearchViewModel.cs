using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace STG.ViewModels.User
{
    public class UserSearchViewModel
    {
        public List<UserSearchPreviewViewModel> users { get; set; }
        public int count { get; set; }

        public UserSearchViewModel(List<UserSearchPreviewViewModel> users, int count)
        {
            this.users = users;
            this.count = count;
        }
    }
}
