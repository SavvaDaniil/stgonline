using STG.ViewModels.Subscription;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace STG.ViewModels.Lesson
{
    public class LessonBuyViewModel
    {
        public STG.Entities.Lesson lesson { get; set; }
        public List<SubscriptionLiteViewModel> subscriptions { get; set; }

        public LessonBuyViewModel(STG.Entities.Lesson lesson, List<SubscriptionLiteViewModel> subscriptions)
        {
            this.lesson = lesson;
            this.subscriptions = subscriptions;
        }
    }
}
