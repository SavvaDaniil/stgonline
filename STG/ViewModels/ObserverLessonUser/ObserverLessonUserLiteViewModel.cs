using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace STG.ViewModels.ObserverLessonUser
{
    public class ObserverLessonUserLiteViewModel
    {
        public int currentTime { get; set; }

        public ObserverLessonUserLiteViewModel(int currentTime)
        {
            this.currentTime = currentTime;
        }
    }
}
