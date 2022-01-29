using Microsoft.EntityFrameworkCore;
using STG.Component;
using STG.Data;
using STG.Entities;
using STG.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace STG.Service
{
    public class HomeworkService
    {
        private ApplicationDbContext _dbc;
        public HomeworkService(ApplicationDbContext dbc)
        {
            this._dbc = dbc;
        }

        public async Task<Homework> findById(int id)
        {
            return await _dbc.Homeworks
                .Include(p => p.packageLesson)
                .Include(p => p.user)
                .Where(p => p.id == id)
                .OrderByDescending(p => p.id)
                .FirstOrDefaultAsync();
        }

        public async Task<Homework> find(User user, PackageLesson packageLesson)
        {
            return await _dbc.Homeworks
                .Where(p => p.user == user && p.packageLesson == packageLesson)
                .OrderByDescending(p => p.id)
                .FirstOrDefaultAsync();
        }

        public async Task<bool> isAnySend(User user, PackageLesson packageLesson)
        {
            return await _dbc.Homeworks
                .Where(p => p.user == user && p.packageLesson == packageLesson && p.statusOfUpload == 1 && p.status == 1)
                .OrderByDescending(p => p.id)
                .AnyAsync();
        }

        public async Task<int> getCountNotReaded(User user)
        {
            return await _dbc.Homeworks
                .Where(p => p.user == user && p.answer_from_teacher != null && p.status_of_seen_of_message_from_teacher == 0)
                .OrderByDescending(p => p.id)
                .CountAsync();
        }

        public async Task<int> getCountNotReadedByTeacher(Teacher teacher)
        {
            return await _dbc.Homeworks
                .Where(p => p.statusOfSeen == 0 && p.packageLesson.package.teacher == teacher)
                .OrderByDescending(p => p.id)
                .CountAsync();
        }

        public async Task<int> getCountAllNotReadedByAnyTeacher()
        {
            return await _dbc.Homeworks
                .Where(p => p.statusOfSeen == 0)
                .OrderByDescending(p => p.id)
                .CountAsync();
        }


        public async Task<List<Homework>> listAllByUser(User user)
        {
            return await _dbc.Homeworks
                .Include(p => p.packageLesson)
                .Where(p => p.user == user)
                .OrderBy(p => p.date_of_add)
                .ToListAsync();
        }

        public async Task<List<Homework>> listAllByUserAndPackageLesson(User user, PackageLesson packageLesson)
        {
            return await _dbc.Homeworks
                .Where(p => p.user == user && p.packageLesson == packageLesson)
                .OrderByDescending(p => p.id)
                .ToListAsync();
        }

        public async Task<List<Homework>> listAllByUserAndPackage(User user, Package package)
        {
            return await _dbc.Homeworks
                .Where(p => p.user == user && p.packageLesson.package == package)
                .OrderByDescending(p => p.id)
                .ToListAsync();
        }

        public async Task<Homework> add(User user, PackageLesson packageLesson)
        {
            Homework homework = new Homework();
            homework.user = user;
            homework.packageLesson = packageLesson;
            homework.hash = RandomComponent.RandomString(32);

            homework.date_of_add = DateTime.Now;

            await _dbc.Homeworks.AddAsync(homework);
            await _dbc.SaveChangesAsync();

            return homework;
        }

        public async Task<bool> isAnyUnreadForAdmin(User user, Package package)
        {
            return await _dbc.Homeworks
                .Where(p => p.user == user && p.packageLesson.package == package && p.statusOfSeen == 0)
                .AnyAsync();
        }

        public async Task<bool> isAnyUnreadForUser(User user, PackageLesson packageLesson)
        {
            return await _dbc.Homeworks
                .Where(p => p.user == user && p.packageLesson == packageLesson && p.statusOfSeen == 0)
                .AnyAsync();
        }


        public async Task<Homework> updateRenew(Homework homework)
        {
            homework.date_of_update = DateTime.Now;
            homework.status = 1;
            homework.statusOfSeen = 0;
            homework.statusOfUpload = 0;
            await _dbc.SaveChangesAsync();
            return homework;
        }

        public async Task<Homework> update(Homework homework, string comment)
        {
            homework.comment = comment;
            homework.date_of_update = DateTime.Now;
            homework.status = 1;
            homework.statusOfSeen = 0;
            homework.statusOfUpload = 1;
            homework.date_of_seen_by_admin = null;
            await _dbc.SaveChangesAsync();
            return homework;
        }

        public async Task<Homework> newAnswerFromAdmin(Homework homework, string answer)
        {
            homework.answer_from_teacher = answer;
            homework.status_of_seen_of_message_from_teacher = 0;
            //homework.date_of_update_of_teacher = DateTime.Now;
            await _dbc.SaveChangesAsync();
            return homework;
        }

        public async Task<bool> setSeenByAdmin(Homework homework)
        {
            homework.statusOfSeen = 1;
            homework.date_of_seen_by_admin = DateTime.Now;
            await _dbc.SaveChangesAsync();
            return true;
        }

        public async Task<bool> setSeenByUser(Homework homework)
        {
            homework.date_of_update_of_teacher = DateTime.Now;
            homework.status_of_seen_of_message_from_teacher = 1;
            await _dbc.SaveChangesAsync();
            return true;
        }

        public async Task<bool> delete(int id)
        {
            Homework homework = await findById(id);
            if (homework == null) return false;
            this._dbc.Homeworks.Remove(homework);
            await this._dbc.SaveChangesAsync();
            return true;
        }


    }
}
