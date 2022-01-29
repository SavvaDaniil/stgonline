using STG.ViewModels.Video;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace STG.ViewModels
{
    public class EnumVideoLiteViewModel
    {
        public IEnumerable<VideoLiteViewModel> videos { get; set; }

        public EnumVideoLiteViewModel(IEnumerable<VideoLiteViewModel> videos)
        {
            this.videos = videos;
        }
    }
}
