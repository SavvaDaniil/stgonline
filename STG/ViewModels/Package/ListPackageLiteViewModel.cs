using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace STG.ViewModels.Package
{
    public class ListPackageLiteViewModel
    {
        public List<PackageLiteViewModel> packages { get; set; }

        public ListPackageLiteViewModel(List<PackageLiteViewModel> packages)
        {
            this.packages = packages;
        }
    }
}
