using STG.Entities;
using STG.ViewModels.Video;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace STG.ViewModels.Lesson
{
    public class LessonEditViewModel
    {
        public LessonViewModel lesson { get; set; }
        public IEnumerable<Level> levels { get; set; }
        public IEnumerable<LessonType> lessonTypes { get; set; }
        public IEnumerable<Style> styles { get; set; }
        public IEnumerable<STG.Entities.Teacher> teachers { get; set; }
        public List<VideoOptionNameDurationViewModel> videos { get; set; }


        public LessonEditViewModel(LessonViewModel lesson, IEnumerable<Level> levels, IEnumerable<LessonType> lessonTypes, IEnumerable<Style> styles, IEnumerable<STG.Entities.Teacher> teachers, List<VideoOptionNameDurationViewModel> videos)
        {
            this.lesson = lesson;
            this.levels = levels;
            this.lessonTypes = lessonTypes;
            this.styles = styles;
            this.teachers = teachers;
            this.videos = videos;
        }
    }
}
