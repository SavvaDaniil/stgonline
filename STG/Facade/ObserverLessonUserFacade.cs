using Microsoft.AspNetCore.Http;
using STG.Data;
using STG.DTO.ObserverLessonUser;
using STG.Models;
using STG.Entities;
using STG.Service;
using STG.ViewModels.ObserverLessonUser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace STG.Facade
{
    public class ObserverLessonUserFacade
    {
        private ApplicationDbContext _dbc;
        public ObserverLessonUserFacade(ApplicationDbContext dbc)
        {
            _dbc = dbc;
        }

        public async Task<ObserverLessonUser> findByUserAndLesson(User user, Lesson lesson)
        {
            ObserverLessonUserService observerLessonUserService = new ObserverLessonUserService(_dbc);
            return await observerLessonUserService.findByIdUserAndLesson(user, lesson);
        }

        public async Task<List<ObserverLessonUser>> listAllByUser(User user)
        {
            ObserverLessonUserService observerLessonUserService = new ObserverLessonUserService(_dbc);
            return await observerLessonUserService.listAllByUser(user);
        }


        public async Task<Dictionary<int, ObserverLessonUserOnlyTime>> dictAllByUser(User user)
        {
            ObserverLessonUserService observerLessonUserService = new ObserverLessonUserService(_dbc);
            List<ObserverLessonUser> observersLessonUser = await observerLessonUserService.listAllByUser(user);

            Dictionary<int, ObserverLessonUserOnlyTime> dictObserversLessonUser = new Dictionary<int, ObserverLessonUserOnlyTime>();
            foreach (ObserverLessonUser observerLessonUser in observersLessonUser)
            {
                dictObserversLessonUser.Add(observerLessonUser.lesson.id, new ObserverLessonUserOnlyTime(observerLessonUser.currentTime, observerLessonUser.maxViewedTime, observerLessonUser.length));
            }
            observersLessonUser = null;
            observerLessonUserService = null;

            return dictObserversLessonUser;
        }


        public async Task<ObserverLessonUser> update(HttpContext httpContext, ObserverLessonUserDTO observerLessonUserDTO)
        {
            UserFacade userFacade = new UserFacade(_dbc);
            User user = await userFacade.getCurrentOrNull(httpContext);
            if (user == null) return null;

            LessonService lessonService = new LessonService(_dbc);
            Lesson lesson = await lessonService.findById(observerLessonUserDTO.id_of_lesson);
            if (lesson == null) return null;

            ObserverLessonUserService observerLessonUserService = new ObserverLessonUserService(_dbc);
            return await observerLessonUserService.update(user, lesson, observerLessonUserDTO.currentTime, observerLessonUserDTO.length);
        }

        public async Task<ObserverLessonUserLiteViewModel> getLast(User user, Lesson lesson)
        {
            ObserverLessonUserService observerLessonUserService = new ObserverLessonUserService(_dbc);
            ObserverLessonUser observerLessonUser = await observerLessonUserService.findWithoutInnerByIdUserAndLesson(user, lesson);
            if (observerLessonUser == null) return null;

            return new ObserverLessonUserLiteViewModel(observerLessonUser.currentTime);
        }


        public async Task<bool> deleteById(int id_of_observer_lesson_user)
        {
            ObserverLessonUserService observerLessonUserService = new ObserverLessonUserService(_dbc);
            return await observerLessonUserService.delete(id_of_observer_lesson_user);
        }
    }
}
