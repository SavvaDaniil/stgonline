using STG.ViewModels.Video;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace STG.ViewModels.Video
{
    public class VideoViewModel
    {
        public int id { get; set; }
        public string name { get; set; }
        public string hashPath { get; set; }
        public int duration { get; set; }

        public int durationHours { get; set; }
        public int durationMinutes { get; set; }
        public int durationSeconds { get; set; }

        //чтобы можно было один постер видео поставить, а также менять его в уроках, если нужно
        public string posterSrc { get; set; }
        
        public VideoConverterStatusViewModel videoConverterStatusViewModel { get; set; }
        public VideoApiStatusViewModel videoApiStatusViewModel { get; set; }

        public bool isVideoExist { get; set; }
        public string filePath { get; set; }

        public VideoViewModel(int id, string name, string hashPath, int duration, int durationHours, int durationMinutes, int durationSeconds, string posterSrc)
        {
            this.id = id;
            this.name = name;
            this.hashPath = hashPath;
            this.duration = duration;
            this.durationHours = durationHours;
            this.durationMinutes = durationMinutes;
            this.durationSeconds = durationSeconds;
            this.posterSrc = posterSrc;
        }

        public VideoViewModel(int id, string name, string hashPath, int duration, int durationHours, int durationMinutes, int durationSeconds, string posterSrc, VideoConverterStatusViewModel videoConverterStatusViewModel, VideoApiStatusViewModel videoApiStatusViewModel) : this(id, name, hashPath, duration, durationHours, durationMinutes, durationSeconds, posterSrc)
        {
            this.videoConverterStatusViewModel = videoConverterStatusViewModel;
            this.videoApiStatusViewModel = videoApiStatusViewModel;
        }
    }
}
