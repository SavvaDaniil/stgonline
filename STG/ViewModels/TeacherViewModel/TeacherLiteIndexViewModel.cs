using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace STG.ViewModels.TeacherViewModel
{
    public class TeacherLiteIndexViewModel
    {
        public int id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string posterSrc { get; set; }
        public string specialSideForClassName { get; set; }

        public TeacherLiteIndexViewModel(int id, string name, string description, string posterSrc, string specialSideForClassName)
        {
            this.id = id;
            this.name = name;
            this.description = description;
            this.posterSrc = posterSrc;
            this.specialSideForClassName = specialSideForClassName;
        }
    }
}
