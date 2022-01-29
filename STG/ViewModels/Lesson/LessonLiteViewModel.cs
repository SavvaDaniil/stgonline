using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace STG.ViewModels.Lesson
{
    public class LessonLiteViewModel
    {
        public int id { get; set; }
        public string name { get; set; }
        public string shortName { get; set; }
        public bool active { get; set; }
        public bool isHaveVideo { get; set; }

        public string levelName { get; set; }
        public string lessonTypeName { get; set; }
        public string styleName { get; set; }
        public string teacherName { get; set; }

        public string posterSrc { get; set; }
        public string teaserSrc { get; set; }


        public LessonLiteViewModel(int id, string name, bool active)
        {
            this.id = id;
            this.name = name;
            this.active = active;
        }

        public LessonLiteViewModel(int id, string name, string shortName, bool active, string levelName, string lessonTypeName, string styleName, string teacherName)
        {
            this.id = id;
            this.name = name;
            this.shortName = shortName;
            this.active = active;
            this.levelName = levelName;
            this.lessonTypeName = lessonTypeName;
            this.styleName = styleName;
            this.teacherName = teacherName;
        }

        public LessonLiteViewModel(int id, string name, string shortName, bool active, string levelName, string lessonTypeName, string styleName, string teacherName, string posterSrc, string teaserSrc)
        {
            this.id = id;
            this.name = name;
            this.shortName = shortName;
            this.active = active;
            this.levelName = levelName;
            this.lessonTypeName = lessonTypeName;
            this.styleName = styleName;
            this.teacherName = teacherName;
            this.posterSrc = posterSrc;
            this.teaserSrc = teaserSrc;
        }

        public LessonLiteViewModel(int id, string name, string shortName, bool active, bool isHaveVideo)
        {
            this.id = id;
            this.name = name;
            this.shortName = shortName;
            this.active = active;
            this.isHaveVideo = isHaveVideo;
        }
    }
}
