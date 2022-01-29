using STG.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace STG.ViewModels.PackageChat
{
    public class ListPackageChatPreviewsViewModel
    {
        public List<PackageChatPreviewViewModel> packageChatPreviewsViewModel { get; set; }
        public Teacher teacher { get; set; }

        public ListPackageChatPreviewsViewModel(List<PackageChatPreviewViewModel> packageChatPreviewsViewModel, Teacher teacher)
        {
            this.packageChatPreviewsViewModel = packageChatPreviewsViewModel;
            this.teacher = teacher;
        }
    }
}
