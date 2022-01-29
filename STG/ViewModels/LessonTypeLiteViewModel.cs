using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace STG.ViewModels
{
    public class LessonTypeLiteViewModel
    {
        public int id { get; set; }
        public string name { get; set; }
        public int active { get; set; }

        public LessonTypeLiteViewModel(int id, string name, int active)
        {
            this.id = id;
            this.name = name;
            this.active = active;
        }
    }
}
