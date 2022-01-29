using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Linq;
using STG.Attribute;
using STG.Component;
using STG.Data;
using STG.DTO.Lesson;
using STG.Entities;
using STG.Interface.Facade;
using STG.Service;
using STG.ViewModels;
using STG.ViewModels.Homework;
using STG.ViewModels.Lesson;
using STG.ViewModels.ObserverLessonUser;
using STG.ViewModels.Package;
using STG.ViewModels.Subscription;
using STG.ViewModels.Video;

namespace STG.Facade
{
    public class LessonFacade : ILessonFacade
    {
        private ApplicationDbContext _dbc;
        public LessonFacade(ApplicationDbContext dbc)
        {
            _dbc = dbc;
        }
        private readonly IServiceScopeFactory _serviceScopeFactory;
        public LessonFacade(ApplicationDbContext dbc, IServiceScopeFactory serviceScopeFactory)
        {
            _dbc = dbc;
            _serviceScopeFactory = serviceScopeFactory;
        }

        private string uploadFolder = Directory.GetCurrentDirectory() + "\\wwwroot\\uploads\\lesson\\";
        //private string uploadUrlApiServer = "http://127.0.0.1:5000";
        private string uploadUrlApiServer = "XXXXXXXXXXXXXXXXXX:81";
        private string folderContentUrlApiServer = "XXXXXXXXXXXXXXXXXX";


    }
}
