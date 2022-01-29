using STG.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace STG.ViewModels
{
    public class EnumLessonTypesViewModel
    {
        public IEnumerable<LessonType> lessonTypes { get; set; }

        public EnumLessonTypesViewModel(IEnumerable<LessonType> lessonTypes)
        {
            this.lessonTypes = lessonTypes;
        }
    }
}
