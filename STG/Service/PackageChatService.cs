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

        public async Task<PackageChat> findById(int id)
        {
            return await _dbc.PackageChats
                .Include(p => p.user)
                .Include(p => p.admin)
                .Include(p => p.teacher)
                .OrderByDescending(p => p.id)
                .FirstOrDefaultAsync(p => p.id == id);
        }

        public async Task<List<PackageChat>> listAllByUserAndConnectionUserToPrivatePackage(User user, Package package)
        {
            return await _dbc.PackageChats
                .Include(p => p.admin )
                .Include(p => p.teacher)
                .Where(p => p.user == user && p.package == package)
                .OrderBy(p => p.date_of_add)
                .ToListAsync();
        }


        /*
        public async Task<List<PackageChat>> listAllByConnectionUserToPrivatePackage(Package package)
        {
            return await _dbc.PackageChats
                .Include(p => p.user)
                .Include(p => p.admin)
                .Include(p => p.teacher)
                .Where(p => p.package == package)
                .OrderBy(p => p.date_of_add)
                .ToListAsync();
        }
        */

        public async Task<bool> updateListRead(List<PackageChat> packageChats, int watching_by_user0_teacher1)
        {
            foreach (PackageChat packageChat in packageChats)
            {
                if (((packageChat.user0_teacher1 == 0 && watching_by_user0_teacher1 == 1) || (packageChat.user0_teacher1 == 1 && watching_by_user0_teacher1 == 0))
                    && (packageChat.is_read == 0 || packageChat.date_of_read == null))
                {
                    packageChat.is_read = 1;
                    packageChat.date_of_read = DateTime.Now;
                    await _dbc.SaveChangesAsync();
                } 
            }
            return true;
        }

        public async Task<bool> isAnyUnreadByUserAndPackage(User user, Package package, int user0_admin1)
        {
            return await _dbc.PackageChats
                .Where(p => p.user == user && p.package == package && p.user0_teacher1 == user0_admin1 && p.is_read == 0)
                .AnyAsync();
        }

        public async Task<int> getCountNotReaded(User user)
        {
            return await _dbc.PackageChats
                .Where(p => p.user == user && p.user0_teacher1 == 1 && p.is_read == 0)
                .OrderByDescending(p => p.id)
                .CountAsync();
        }

        public async Task<int> getCountNotReadedByTeacher(Teacher teacher)
        {
            return await _dbc.PackageChats
                .Where(p => p.teacher == teacher && p.user0_teacher1 == 0 && p.is_read == 0)
                .OrderByDescending(p => p.id)
                .CountAsync();
        }
        public async Task<int> getCountNotReadedByAnyTeacher()
        {
            return await _dbc.PackageChats
                .Where(p => p.user0_teacher1 == 0 && p.is_read == 0)
                .OrderByDescending(p => p.id)
                .CountAsync();
        }


        public async Task<PackageChat> add(User user, Admin admin, Teacher teacher, Package package, string message, int fromUser0FromAdmin1)
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

            await _dbc.PackageChats.AddAsync(packageChat);
            await _dbc.SaveChangesAsync();

            return packageChat;
        }

        public async Task<bool> setRead(int id)
        {
            PackageChat packageChat = await findById(id);
            if (packageChat == null) return false;

            packageChat.date_of_read = DateTime.Now;
            await _dbc.SaveChangesAsync();

            return true;
        }
    }
}
