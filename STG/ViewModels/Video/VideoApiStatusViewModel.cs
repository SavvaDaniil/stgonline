using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace STG.ViewModels.Video
{
    public class VideoApiStatusViewModel
    {
        public string status { get; set; }
        public int isContentExist { get; set; }
        public string src { get; set; }
        public string mobileSrc { get; set; }

        public VideoApiStatusViewModel(string status)
        {
            this.status = status;
        }

        /*
        public VideoApiStatusViewModel(string status, int isContentExist, string src)
        {
            this.status = status;
            this.isContentExist = isContentExist;
            this.src = src;
        }
        */

        public VideoApiStatusViewModel(string status, int isContentExist, string src, string mobileSrc)
        {
            this.status = status;
            this.isContentExist = isContentExist;
            this.src = src;
            this.mobileSrc = mobileSrc;
        }
    }
}
