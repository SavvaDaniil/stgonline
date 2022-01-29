using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace STG.ViewModels.Package
{
    public class ListPackagePreviewViewModel
    {
        public List<PackagePreviewViewModel> packages { get; set; }

        public ListPackagePreviewViewModel(List<PackagePreviewViewModel> packages)
        {
            this.packages = packages;
        }
    }
}
