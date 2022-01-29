using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using STG.Entities;

namespace STG.Interface.Strategy
{
    interface IPurchaseLessonStrategy
    {
        Lesson buyLesson(User user, Lesson lesson, Payment payment);
    }
}
