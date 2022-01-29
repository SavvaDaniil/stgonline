using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace STG.ViewModels.Video
{
    public class VideoSubsectionLiteViewModel
    {

        public int id { get; set; }
        public string name { get; set; }
        public int timing_minutes { get; set; }
        public int timing_seconds { get; set; }

        public VideoSubsectionLiteViewModel(int id, string name, int timing_minutes, int timing_seconds)
        {
            this.id = id;
            this.name = name;
            this.timing_minutes = timing_minutes;
            this.timing_seconds = timing_seconds;
        }
    }
}
