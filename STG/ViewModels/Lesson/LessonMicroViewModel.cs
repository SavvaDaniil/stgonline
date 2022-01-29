using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace STG.ViewModels.Lesson
{
    public class LessonMicroViewModel
    {
        public int id { get; set; }
        public string name { get; set; }
        public int active { get; set; }
        public int isVisible { get; set; }
        public int orderInList { get; set; }
        public string posterSrc { get; set; }

        public LessonMicroViewModel(int id, string name, int active, int isVisible, int orderInList, string posterSrc)
        {
            this.id = id;
            this.name = name;
            this.active = active;
            this.isVisible = isVisible;
            this.orderInList = orderInList;
            this.posterSrc = posterSrc;
        }
    }




}
