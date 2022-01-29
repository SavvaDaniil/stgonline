using STG.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace STG.ViewModels.Package
{
    public class PackagePreviewViewModel
    {
        public int id { get; set; }
        public string name { get; set; }
        public int active { get; set; }
        public string levelName { get; set; }
        public string styleName { get; set; }
        public string teacherName { get; set; }
        public int lessonsCount { get; set; }
        public string posterSrc { get; set; }
        public string teaserSrc { get; set; }

        public PackagePreviewViewModel(int id, string name, int active, string levelName, string styleName, string teacherName, int lessonsCount, string posterSrc, string teaserSrc)
        {
            this.id = id;
            this.name = name;
            this.active = active;
            this.levelName = levelName;
            this.styleName = styleName;
            this.teacherName = teacherName;
            this.lessonsCount = lessonsCount;
            this.posterSrc = posterSrc;
            this.teaserSrc = teaserSrc;
        }
    }
}
