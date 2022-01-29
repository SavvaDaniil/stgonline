using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace STG.ViewModels.Package
{
    public class PackageBuyViewModel
    {
        public STG.Entities.Package package { get; set; }

        public PackageBuyViewModel(Entities.Package package)
        {
            this.package = package;
        }
    }
}
