using Microsoft.EntityFrameworkCore;
using STG.Data;
using STG.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace STG.Service
{
    public class ObserverLessonUserService
    {
        private ApplicationDbContext _dbc;
        public ObserverLessonUserService(ApplicationDbContext dbc)
        {
            this._dbc = dbc;
        }

        public ObserverLessonUser findById(int id)
        {
            return this._dbc.ObserversLessonUser
                .Include(p => p.lesson)
                .Include(p => p.user)
                .FirstOrDefault(p => p.id == id);
        }
        public ObserverLessonUser findByIdUserAndLesson(User user, Lesson lesson)
        {
            return this._dbc.ObserversLessonUser
                .Include(p => p.lesson)
                .Include(p => p.user)
                .FirstOrDefault(p => p.user == user && p.lesson == lesson);
        }
        public ObserverLessonUser findWithoutInnerByIdUserAndLesson(User user, Lesson lesson)
        {
            return _dbc.ObserversLessonUser
                .FirstOrDefault(p => p.user == user && p.lesson == lesson);
        }

        public bool isAny(User user, Lesson lesson)
        {
            return _dbc.ObserversLessonUser
                .Where(p => p.user == user && p.lesson == lesson)
                .Any();
        }

        public ObserverLessonUser update(User user, Lesson lesson, int currentTime, int length)
        {
            bool isAdded = false;
            ObserverLessonUser observerLessonUser = findByIdUserAndLesson(user, lesson);
            if(observerLessonUser == null)
            {
                observerLessonUser = new ObserverLessonUser();
                observerLessonUser.user = user;
                observerLessonUser.lesson = lesson;
                observerLessonUser.date_of_add = DateTime.Now;
                observerLessonUser.currentTime = 0;
                observerLessonUser.maxViewedTime = 0;
                isAdded = true;
            }
            observerLessonUser.currentTime = currentTime;
            if (observerLessonUser.maxViewedTime < currentTime)
            {
                observerLessonUser.maxViewedTime = currentTime;
            }
            observerLessonUser.length = length;
            observerLessonUser.date_of_update = DateTime.Now;

            if(isAdded) _dbc.ObserversLessonUser.Add(observerLessonUser);
            _dbc.SaveChanges();

            return observerLessonUser;
        }

        public List<ObserverLessonUser> listAllByUser(User user)
        {
            return this._dbc.ObserversLessonUser
                .Include(p => p.lesson)
                .Include(p => p.user)
                .Where(p => p.user == user)
                .OrderByDescending(p => p.id)
                .ToList();
        }

        public bool delete(int id)
        {
            ObserverLessonUser observerLessonUser = findById(id);
            if (observerLessonUser == null) return false;
            this._dbc.ObserversLessonUser.Remove(observerLessonUser);
            this._dbc.SaveChanges();
            return true;
        }
    }
}
