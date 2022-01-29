using STG.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace STG.ViewModels.Video
{
    public class VideoSectionViewModel
    {
        public int id { get; set; }
        public string name { get; set; }
        public List<VideoSubsectionLiteViewModel> videosubsectionList { get; set; }


        public VideoSectionViewModel(int id, string name, List<VideoSubsectionLiteViewModel> videosubsectionList)
        {
            this.id = id;
            this.name = name;
            this.videosubsectionList = videosubsectionList;
        }
    }
}
