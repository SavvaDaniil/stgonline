using STG.Models;
using STG.ViewModels.ObserverLessonUser;
using STG.ViewModels.Video;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace STG.ViewModels.Lesson
{
    public class LessonVideoViewModel
    {
        public int id { get; set; }
        public string name { get; set; }
        public string shortName { get; set; }
        public int id_of_teacher { get; set; }
        public string name_of_teacher { get; set; }
        public string posterTeacherSrc { get; set; }

        public int lessonTypeId { get; set; }
        public string lessonTypeName { get; set; }
        public string[] levelNames { get; set; }
        public string duration { get; set; }
        public string musicName { get; set; }

        public int id_of_video { get; set; }
        public string posterSrc { get; set; }
        public string videoSrc { get; set; }
        public string videoMobileSrc { get; set; }
        public string teaserSrc { get; set; }

        public LessonHomeworkViewModel lessonHomeworkViewModel { get; set; }
        public ObserverLessonUserLiteViewModel observer { get; set; }
        public List<VideoSectionViewModel> videosectionList { get; set; }

        public LessonVideoViewModel(int id, string name, string shortName, int id_of_teacher, string name_of_teacher, string posterTeacherSrc, int lessonTypeId, string lessonTypeName, string[] levelNames, string duration, string musicName, int id_of_video, string posterSrc, string videoSrc, string videoMobileSrc, string teaserSrc, LessonHomeworkViewModel lessonHomeworkViewModel, ObserverLessonUserLiteViewModel observer, List<VideoSectionViewModel> videosectionList)
        {
            this.id = id;
            this.name = name;
            this.shortName = shortName;
            this.id_of_teacher = id_of_teacher;
            this.name_of_teacher = name_of_teacher;
            this.posterTeacherSrc = posterTeacherSrc;
            this.lessonTypeId = lessonTypeId;
            this.lessonTypeName = lessonTypeName;
            this.levelNames = levelNames;
            this.duration = duration;
            this.musicName = musicName;
            this.id_of_video = id_of_video;
            this.posterSrc = posterSrc;
            this.videoSrc = videoSrc;
            this.videoMobileSrc = videoMobileSrc;
            this.teaserSrc = teaserSrc;
            this.lessonHomeworkViewModel = lessonHomeworkViewModel;
            this.observer = observer;
            this.videosectionList = videosectionList;
        }
    }
}
