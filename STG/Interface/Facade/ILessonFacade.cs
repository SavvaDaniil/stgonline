using STG.Data;
using STG.Models;
using STG.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using STG.Entities;

namespace STG.Interface.Facade
{
    interface ILessonFacade
    {
        //Task<List<LessonLiteViewModel>> getAllActiveByFilter(ApplicationDbContext _dbc, User user, Style style, Level level, string name);
        Lesson getIfAvailable(User user, Lesson lesson);
    }
}
