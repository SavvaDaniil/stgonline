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

        public async Task<ObserverLessonUser> findById(int id)
        {
            return await this._dbc.ObserversLessonUser
                .Include(p => p.lesson)
                .Include(p => p.user)
                .FirstOrDefaultAsync(p => p.id == id);
        }
        public async Task<ObserverLessonUser> findByIdUserAndLesson(User user, Lesson lesson)
        {
            return await this._dbc.ObserversLessonUser
                .Include(p => p.lesson)
                .Include(p => p.user)
                .FirstOrDefaultAsync(p => p.user == user && p.lesson == lesson);
        }
        public async Task<ObserverLessonUser> findWithoutInnerByIdUserAndLesson(User user, Lesson lesson)
        {
            return await _dbc.ObserversLessonUser
                .FirstOrDefaultAsync(p => p.user == user && p.lesson == lesson);
        }

        public async Task<bool> isAny(User user, Lesson lesson)
        {
            return await _dbc.ObserversLessonUser
                .Where(p => p.user == user && p.lesson == lesson)
                .AnyAsync();
        }

        public async Task<ObserverLessonUser> update(User user, Lesson lesson, int currentTime, int length)
        {
            bool isAdded = false;
            ObserverLessonUser observerLessonUser = await findByIdUserAndLesson(user, lesson);
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

            if(isAdded) await _dbc.ObserversLessonUser.AddAsync(observerLessonUser);
            await _dbc.SaveChangesAsync();

            return observerLessonUser;
        }

        public async Task<List<ObserverLessonUser>> listAllByUser(User user)
        {
            return await this._dbc.ObserversLessonUser
                .Include(p => p.lesson)
                .Include(p => p.user)
                .Where(p => p.user == user)
                .OrderByDescending(p => p.id)
                .ToListAsync();
        }

        public async Task<bool> delete(int id)
        {
            ObserverLessonUser observerLessonUser = await findById(id);
            if (observerLessonUser == null) return false;
            this._dbc.ObserversLessonUser.Remove(observerLessonUser);
            await this._dbc.SaveChangesAsync();
            return true;
        }
    }
}
