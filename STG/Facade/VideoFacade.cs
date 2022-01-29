using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Linq;
using STG.Attribute;
using STG.Data;
using STG.DTO;
using STG.DTO.Video;
using STG.Entities;
using STG.Service;
using STG.ViewModels;
using STG.ViewModels.Video;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace STG.Facade
{
    public class VideoFacade
    {
        private ApplicationDbContext _dbc;

        public VideoFacade(ApplicationDbContext dbc)
        {
            this._dbc = dbc;
        }

        private string uploadFolder = Directory.GetCurrentDirectory() + "\\wwwroot\\uploads\\video\\";
        //private string uploadUrlApiServer = "http://127.0.0.1:5000";
        private string uploadUrlApiServer = "XXXXXXXXXXXXXXXXXX:81";
        private string folderVideoContentUrlApiServer = "XXXXXXXXXXXXXXXXXX";


    }
}
