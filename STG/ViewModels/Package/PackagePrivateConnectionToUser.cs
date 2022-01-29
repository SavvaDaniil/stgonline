using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace STG.ViewModels.Package
{
    public class PackagePrivateConnectionToUser
    {
        public int id { get; set; }
        public string name { get; set; }
        public int active { get; set; }

        public PackagePrivateConnectionToUser(int id, string name, int active)
        {
            this.id = id;
            this.name = name;
            this.active = active;
        }
    }
}
