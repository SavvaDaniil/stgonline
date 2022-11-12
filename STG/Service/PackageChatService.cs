using Microsoft.EntityFrameworkCore;
using STG.Data;
using STG.Entities;
using STG.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace STG.Service
{
    public class PackageChatService
    {
        public ApplicationDbContext _dbc;
        public PackageChatService(ApplicationDbContext dbc)
        {
            this._dbc = dbc;
        }

        public PackageChat findById(int id)
        {
            return _dbc.PackageChats
                .Include(p => p.user)
                .Include(p => p.admin)
                .Include(p => p.teacher)
                .OrderByDescending(p => p.id)
                .FirstOrDefault(p => p.id == id);
        }

        public List<PackageChat> listAllByUserAndConnectionUserToPrivatePackage(int userId, int packageId)
        {
            return _dbc.PackageChats
                .Include(p => p.admin)
                .Include(p => p.teacher)
                .Include(p => p.user)
                .Where(p => p.user.Id == userId && p.package.id == packageId)
                .OrderBy(p => p.date_of_add)
                .ToList();
        }

        public List<PackageChat> listAllByUserAndConnectionUserToPrivatePackage(User user, Package package)
        {
            return _dbc.PackageChats
                .Include(p => p.admin )
                .Include(p => p.teacher)
                .Where(p => p.user == user && p.package == package)
                .OrderBy(p => p.date_of_add)
                .ToList();
        }


        /*
        public List<PackageChat>> listAllByConnectionUserToPrivatePackage(Package package)
        {
            return _dbc.PackageChats
                .Include(p => p.user)
                .Include(p => p.admin)
                .Include(p => p.teacher)
                .Where(p => p.package == package)
                .OrderBy(p => p.date_of_add)
                .ToList();
        }
        */

        public bool updateListRead(List<PackageChat> packageChats, int watching_by_user0_teacher1)
        {
            foreach (PackageChat packageChat in packageChats)
            {
                if (((packageChat.user0_teacher1 == 0 && watching_by_user0_teacher1 == 1) || (packageChat.user0_teacher1 == 1 && watching_by_user0_teacher1 == 0))
                    && (packageChat.is_read == 0 || packageChat.date_of_read == null))
                {
                    packageChat.is_read = 1;
                    packageChat.date_of_read = DateTime.Now;
                    _dbc.SaveChanges();
                } 
            }
            return true;
        }

        public bool isAnyUnreadByUserAndPackage(User user, Package package, int user0_admin1)
        {
            return _dbc.PackageChats
                .Where(p => p.user == user && p.package == package && p.user0_teacher1 == user0_admin1 && p.is_read == 0)
                .Any();
        }

        public int getCountNotReaded(User user)
        {
            return _dbc.PackageChats
                .Where(p => p.user == user && p.user0_teacher1 == 1 && p.is_read == 0)
                .OrderByDescending(p => p.id)
                .Count();
        }

        public int getCountNotReadedByTeacher(Teacher teacher)
        {
            return _dbc.PackageChats
                .Where(p => p.teacher == teacher && p.user0_teacher1 == 0 && p.is_read == 0)
                .OrderByDescending(p => p.id)
                .Count();
        }
        public int getCountNotReadedByAnyTeacher()
        {
            return _dbc.PackageChats
                .Where(p => p.user0_teacher1 == 0 && p.is_read == 0)
                .OrderByDescending(p => p.id)
                .Count();
        }


        public PackageChat add(User user, Admin admin, Teacher teacher, Package package, string message, int fromUser0FromAdmin1)
        {
            PackageChat packageChat = new PackageChat();
            packageChat.user = user;
            packageChat.admin = admin;
            packageChat.teacher = teacher;

            if (fromUser0FromAdmin1 == 1)
            {
                packageChat.user0_teacher1 = 1;
            } else
            {
                packageChat.user0_teacher1 = 0;
            }

            packageChat.message = message;

            packageChat.package = package;
            packageChat.date_of_add = DateTime.Now;

            _dbc.PackageChats.Add(packageChat);
            _dbc.SaveChanges();

            return packageChat;
        }

        public bool setRead(int id)
        {
            PackageChat packageChat = findById(id);
            if (packageChat == null) return false;

            packageChat.date_of_read = DateTime.Now;
            _dbc.SaveChanges();

            return true;
        }
    }
}
