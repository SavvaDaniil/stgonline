using STG.Models;
using STG.ViewModels.Package;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using STG.Entities;

namespace STG.DTO.UserDTO
{
    public class UserEditViewModel
    {
        public int id { get; set; }
        public string username { get; set; }
        public int active { get; set; }
        public string firstname { get; set; }
        public string secondname { get; set; }
        public string instagram { get; set; }
        public string date_of_birthday { get; set; }
        public int is_test { get; set; }
        public int is_lesson_full_access { get; set; }
        public Region region { get; set; }

        public List<Region> regions { get; set; }
        public int prolongation { get; set; }

        public List<PackagePrivateConnectionToUser> private_packages { get; set; }

        public UserEditViewModel(int id, string username, int active, string firstname, string secondname, string instagram, string date_of_birthday, int is_test, int is_lesson_full_access, Region region, List<Region> regions, int prolongation, List<PackagePrivateConnectionToUser> private_packages)
        {
            this.id = id;
            this.username = username;
            this.active = active;
            this.firstname = firstname;
            this.secondname = secondname;
            this.instagram = instagram;
            this.date_of_birthday = date_of_birthday;
            this.is_test = is_test;
            this.is_lesson_full_access = is_lesson_full_access;
            this.region = region;
            this.regions = regions;
            this.prolongation = prolongation;
            this.private_packages = private_packages;
        }
    }
}
