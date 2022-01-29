using STG.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace STG.ViewModels.PackageChat
{
    public class PackageChatPreviewViewModel
    {
        public STG.Entities.ConnectionUserToPrivatePackage connectionUserToPrivatePackage { get; set; }
        public STG.Entities.User user { get; set; }
        public STG.Entities.Package package {get;set;}
        public string message_last { get; set; }
        public bool isAnyUnread { get; set; }
        public DateTime? date_of_update { get; set; }

        public PackageChatPreviewViewModel(ConnectionUserToPrivatePackage connectionUserToPrivatePackage, Entities.User user, Entities.Package package, string message_last, bool isAnyUnread, DateTime? date_of_update)
        {
            this.connectionUserToPrivatePackage = connectionUserToPrivatePackage;
            this.user = user;
            this.package = package;
            this.message_last = message_last;
            this.isAnyUnread = isAnyUnread;
            this.date_of_update = date_of_update;
        }
    }
}
