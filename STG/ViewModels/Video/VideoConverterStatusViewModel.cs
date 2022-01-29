using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace STG.ViewModels.Video
{
    public class VideoConverterStatusViewModel
    {
        public int id { get; set; }
        public string status { get; set; }
        public bool isVideoConveterFree { get; set; }
        public DateTime? datetime_current_start { get; set; }
        public DateTime? datetime_last_start { get; set; }
        public DateTime? datetime_last_finish { get; set; }

        public VideoConverterStatusViewModel(string status)
        {
            this.status = status;
        }

        public VideoConverterStatusViewModel(string status, bool isVideoConveterFree)
        {
            this.status = status;
            this.isVideoConveterFree = isVideoConveterFree;
        }
        

        public VideoConverterStatusViewModel(int id, string status, bool isVideoConveterFree, DateTime? datetime_current_start, DateTime? datetime_last_start, DateTime? datetime_last_finish)
        {
            this.id = id;
            this.status = status;
            this.isVideoConveterFree = isVideoConveterFree;
            this.datetime_current_start = datetime_current_start;
            this.datetime_last_start = datetime_last_start;
            this.datetime_last_finish = datetime_last_finish;
        }
    }
}
