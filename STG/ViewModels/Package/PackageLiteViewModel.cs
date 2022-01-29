using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace STG.ViewModels.Package
{
    public class PackageLiteViewModel
    {
        public int id { get; set; }
        public string name { get; set; }
        public int active { get; set; }

        public PackageLiteViewModel(int id, string name, int active)
        {
            this.id = id;
            this.name = name;
            this.active = active;
        }
    }
}
