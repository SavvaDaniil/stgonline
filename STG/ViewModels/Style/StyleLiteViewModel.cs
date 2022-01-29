using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace STG.ViewModels
{
    public class StyleLiteViewModel
    {
        public int id { get; set; }
        public string name { get; set; }
        public string shortDescription { get; set; }
        public int active { get; set; }

        public StyleLiteViewModel(int id, string name, string shortDescription, int active)
        {
            this.id = id;
            this.name = name;
            this.shortDescription = shortDescription;
            this.active = active;
        }
    }
}
