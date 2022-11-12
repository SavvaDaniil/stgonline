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

        public Homework findById(int id)
        {
            return _dbc.Homeworks
                .Include(p => p.packageLesson)
                .Include(p => p.user)
                .Where(p => p.id == id)
                .OrderByDescending(p => p.id)
                .FirstOrDefault();
        }

        public Homework find(User user, PackageLesson packageLesson)
        {
            return _dbc.Homeworks
                .Where(p => p.user == user && p.packageLesson == packageLesson)
                .OrderByDescending(p => p.id)
                .FirstOrDefault();
        }

        public bool isAnySend(User user, PackageLesson packageLesson)
        {
            return _dbc.Homeworks
                .Where(p => p.user == user && p.packageLesson == packageLesson && p.statusOfUpload == 1 && p.status == 1)
                .OrderByDescending(p => p.id)
                .Any();
        }

        public int getCountNotReaded(User user)
        {
            return _dbc.Homeworks
                .Where(p => p.user == user && p.answer_from_teacher != null && p.status_of_seen_of_message_from_teacher == 0)
                .OrderByDescending(p => p.id)
                .Count();
        }

        public int getCountNotReadedByTeacher(Teacher teacher)
        {
            return _dbc.Homeworks
                .Where(p => p.statusOfSeen == 0 && p.packageLesson.package.teacher == teacher)
                .OrderByDescending(p => p.id)
                .Count();
        }

        public int getCountAllNotReadedByAnyTeacher()
        {
            return _dbc.Homeworks
                .Where(p => p.statusOfSeen == 0)
                .OrderByDescending(p => p.id)
                .Count();
        }


        public List<Homework> listAllByUser(User user)
        {
            return _dbc.Homeworks
                .Include(p => p.packageLesson)
                .Where(p => p.user == user)
                .OrderBy(p => p.date_of_add)
                .ToList();
        }

        public List<Homework> listAllByUserAndPackageLesson(User user, PackageLesson packageLesson)
        {
            return _dbc.Homeworks
                .Where(p => p.user == user && p.packageLesson == packageLesson)
                .OrderByDescending(p => p.id)
                .ToList();
        }

        public List<Homework> listAllByUserAndPackage(User user, Package package)
        {
            return _dbc.Homeworks
                .Where(p => p.user == user && p.packageLesson.package == package)
                .OrderByDescending(p => p.id)
                .ToList();
        }

        public Homework add(User user, PackageLesson packageLesson)
        {
            Homework homework = new Homework();
            homework.user = user;
            homework.packageLesson = packageLesson;
            homework.hash = RandomComponent.RandomString(32);

            homework.date_of_add = DateTime.Now;

            _dbc.Homeworks.Add(homework);
            _dbc.SaveChanges();

            return homework;
        }

        public bool isAnyUnreadForAdmin(User user, Package package)
        {
            return _dbc.Homeworks
                .Where(p => p.user == user && p.packageLesson.package == package && p.statusOfSeen == 0)
                .Any();
        }

        public bool isAnyUnreadForUser(User user, PackageLesson packageLesson)
        {
            return _dbc.Homeworks
                .Where(p => p.user == user && p.packageLesson == packageLesson && p.statusOfSeen == 0)
                .Any();
        }


        public Homework updateRenew(Homework homework)
        {
            homework.date_of_update = DateTime.Now;
            homework.status = 1;
            homework.statusOfSeen = 0;
            homework.statusOfUpload = 0;
            _dbc.SaveChanges();
            return homework;
        }

        public Homework update(Homework homework, string comment)
        {
            homework.comment = comment;
            homework.date_of_update = DateTime.Now;
            homework.status = 1;
            homework.statusOfSeen = 0;
            homework.statusOfUpload = 1;
            homework.date_of_seen_by_admin = null;
            _dbc.SaveChanges();
            return homework;
        }

        public Homework newAnswerFromAdmin(Homework homework, string answer)
        {
            homework.answer_from_teacher = answer;
            homework.status_of_seen_of_message_from_teacher = 0;
            //homework.date_of_update_of_teacher = DateTime.Now;
            _dbc.SaveChanges();
            return homework;
        }

        public bool setSeenByAdmin(Homework homework)
        {
            homework.statusOfSeen = 1;
            homework.date_of_seen_by_admin = DateTime.Now;
            _dbc.SaveChanges();
            return true;
        }

        public bool setSeenByUser(Homework homework)
        {
            homework.date_of_update_of_teacher = DateTime.Now;
            homework.status_of_seen_of_message_from_teacher = 1;
            _dbc.SaveChanges();
            return true;
        }

        public bool delete(int id)
        {
            Homework homework = findById(id);
            if (homework == null) return false;
            this._dbc.Homeworks.Remove(homework);
            this._dbc.SaveChanges();
            return true;
        }


    }
}
