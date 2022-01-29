using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace STG.Models
{
    public class ObserverLessonUserOnlyTime
    {
        public int currentTime { get; set; }
        public int maxViewedTime { get; set; }
        public int length { get; set; }

        public ObserverLessonUserOnlyTime(int currentTime, int maxViewedTime, int length)
        {
            this.currentTime = currentTime;
            this.maxViewedTime = maxViewedTime;
            this.length = length;
        }
    }
}
