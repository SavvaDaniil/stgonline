using STG.ViewModels.Package;
using STG.ViewModels.PackageChat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace STG.ViewModels.User
{
    public class UserPassingPackageViewModel
    {
        public PackageInfoViewModel packageInfoViewModel { get; set; }
        public List<PackageChatMessageViewModel> packageChatMessages { get; set; }

        public UserPassingPackageViewModel(PackageInfoViewModel packageInfoViewModel, List<PackageChatMessageViewModel> packageChatMessages)
        {
            this.packageInfoViewModel = packageInfoViewModel;
            this.packageChatMessages = packageChatMessages;
        }
    }
}
