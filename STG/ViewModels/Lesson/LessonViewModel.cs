using STG.Entities;
using STG.ViewModels.Video;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace STG.ViewModels.Lesson
{
    public class LessonViewModel
    {
        public int id { get; set; }
        public string name { get; set; }
        public string shortName { get; set; }
        public string musicName { get; set; }
        public string description { get; set; }
        public int price { get; set; }
        public int days { get; set; }
        public int active { get; set; }
        public int isVisible { get; set; }
        public int isFree { get; set; }

        public string posterSrc { get; set; }
        public string teaserSrc { get; set; }
        
        public Level level { get; set; }
        public LessonType lessonType { get; set; }
        public Teacher teacher { get; set; }
        public Style style { get; set; }
        public STG.Entities.Video video { get; set; }

        public List<Level> levels { get; set; }


        public VideoApiStatusViewModel videoApiStatusViewModel { get; set; }


        public LessonViewModel(int id, string name, string shortName, string musicName, string description, int price, int days, int active, int isVisible, int isFree, string posterSrc, string teaserSrc, Level level, LessonType lessonType, Teacher teacher, Style style, Entities.Video video, List<Level> levels)
        {
            this.id = id;
            this.name = name;
            this.shortName = shortName;
            this.musicName = musicName;
            this.description = description;
            this.price = price;
            this.days = days;
            this.active = active;
            this.isVisible = isVisible;
            this.isFree = isFree;
            this.posterSrc = posterSrc;
            this.teaserSrc = teaserSrc;
            this.level = level;
            this.lessonType = lessonType;
            this.teacher = teacher;
            this.style = style;
            this.video = video;
            this.levels = levels;
        }

        public override string ToString()
        {
            return "LessonViewModel.id: " + this.id;
        }
    }
}
