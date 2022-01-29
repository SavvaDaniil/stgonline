using STG.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace STG.ViewModels
{
    public class JsonAnswerViewModel
    {
        private JsonAnswerViewModel(){}

        public JsonAnswerViewModel(string status, string errors)
        {
            this.status = status;
            this.errors = errors;
        }

        public JsonAnswerViewModel(string status, string errors, STG.Entities.Admin admin, bool isNeedRelogin)
        {
            this.status = status;
            this.errors = errors;
            this.admin = admin;
            this.isNeedRelogin = isNeedRelogin;
        }


        public JsonAnswerViewModel(string status, string errors, STG.Entities.User user)
        {
            this.status = status;
            this.errors = errors;
            this.user = user;
        }

        public string status { get; }
        public string errors { get; }
        public string access_token { get; set; }
        public STG.Entities.User user { get; set; }

        public STG.Entities.Admin admin { get; set; }
        public bool isNeedRelogin { get; set; }
    }
}
