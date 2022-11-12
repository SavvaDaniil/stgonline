using Microsoft.EntityFrameworkCore;
using STG.Data;
using STG.DTO.Lesson;
using STG.Entities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace STG.Service
{
    public class LessonService
    {
        private ApplicationDbContext _dbc;

        public LessonService(ApplicationDbContext dbc)
        {
            this._dbc = dbc;
        }

        public Lesson findById(int id)
        {
            return this._dbc.Lessons
                .Include(p => p.level)
                .Include(p => p.lessonType)
                .Include(p => p.style)
                .Include(p => p.teacher)
                .Include(p => p.video)
                .FirstOrDefault(p => p.id == id);
        }

        public Lesson findPrevOfById(int id_of_current_lesson, int order_of_current_lesson)
        {
            return _dbc.Lessons
                .Where(p => p.id != id_of_current_lesson)
                .Where(p => p.orderInList < order_of_current_lesson)
                .OrderByDescending(p => p.orderInList)
                .FirstOrDefault();
        }
        public Lesson findNextOfById(int id_of_current_lesson, int order_of_current_lesson)
        {
            return _dbc.Lessons
                .Where(p => p.id != id_of_current_lesson)
                .Where(p => p.orderInList > order_of_current_lesson)
                .OrderBy(p => p.orderInList)
                .FirstOrDefault();
        }


        public Lesson add(LessonNewDTO lessonNewDTO)
        {
            Lesson lesson = new Lesson();
            lesson.name = lessonNewDTO.name;
            lesson.days = 30;

            this._dbc.Lessons.Add(lesson);

            this._dbc.SaveChanges();

            lesson.orderInList = lesson.id;
            this._dbc.SaveChanges();

            return lesson;
        }

        public IEnumerable<Lesson> listAll()
        {
            return this._dbc.Lessons.OrderByDescending(p => p.orderInList).ToList();
        }
        public List<Lesson> listAllActive()
        {
            return this._dbc.Lessons.Where(p => p.active == 1).ToList();
        }
        public List<Lesson> listAllActiveByFilter(string name, int id_of_style, int id_of_level = 0, int id_of_teacher = 0, int skip = 0, int isFree = 0, int take = 18)
        {
            if (take < 0) take = 18;

            if (id_of_level != 0 || id_of_teacher != 0)
            {
                if (id_of_level != 0)
                {

                }
                string levelQuery = (id_of_level != 0 ?
                    "INNER JOIN connection_lesson_to_level ON lesson.id = connection_lesson_to_level.lessonid AND connection_lesson_to_level.levelId = " 
                    : String.Empty);
                string id_of_levelStr = (id_of_level != 0 ? id_of_level.ToString() : String.Empty);

                string teacherQuery = (id_of_teacher != 0 ? "AND lesson.teacherid = " : String.Empty);
                string id_of_teacherStr = (id_of_teacher != 0 ? id_of_teacher.ToString() : String.Empty);

                string query = String.Format("SELECT lesson.* " +
                    " FROM lesson " +
                    " " + levelQuery + " {0} " +
                    " WHERE lesson.active > '0' AND lesson.is_visible ='1' "+ teacherQuery + " {1} " +
                    "GROUP BY lesson.id ORDER BY lesson.order_in_list DESC LIMIT {2}, {3}", id_of_levelStr, id_of_teacherStr, skip, take);

                var lessons = _dbc.Lessons.FromSql(query)
                    .Include(p => p.level)
                    .Include(p => p.lessonType)
                    .Include(p => p.style)
                    .Include(p => p.teacher)
                    .ToList();

                /*
                System.Diagnostics.Debug.WriteLine("SELECT lesson.* " +
                    " FROM lesson " +
                    " " + levelQuery + " {0} " +
                    " WHERE lesson.active = '1' AND lesson.is_visible ='1'" +
                    " " + teacherQuery + " {1} " +
                    "GROUP BY lesson.id ORDER BY lesson.id DESC LIMIT {2}, 18", id_of_levelStr, id_of_teacherStr, skip);

                foreach(var lesson in lessons)
                {
                    foreach (PropertyInfo propertyInfo in lesson.GetType().GetProperties())
                    {
                        System.Diagnostics.Debug.WriteLine("lesson.id : "+ lesson.id + " propertyInfo: " + propertyInfo);
                    }

                    System.Diagnostics.Debug.WriteLine("lesson.name : " + lesson.GetType().GetProperty("name").GetValue(lesson, null));
                    //System.Diagnostics.Debug.WriteLine("lesson.styleId : " + lesson.GetType().GetProperty("styleId").GetValue(lesson, null));


                    if (lesson.style != null)
                    {
                        System.Diagnostics.Debug.WriteLine("lesson.style : " + lesson.style.id);
                    }
                }
                */

                return lessons;
            }

            IQueryable<Lesson> q = _dbc.Lessons
                .Include(p => p.level)
                .Include(p => p.lessonType)
                .Include(p => p.style)
                .Include(p => p.teacher)
                .Where(p => p.active > 0)
                .Where(p => p.isVisible == 1)
                .OrderByDescending(p => p.orderInList);
            if (take == 0) take = 18;

            if (!string.IsNullOrEmpty(name)) q = q.Where(p => EF.Functions.Like(p.name, name));
            if (isFree == 1) q = q.Where(p => p.isFree == 1);
            
            //if(id_of_style != 0) q = q.Where(p => p.style.id == id_of_style);

            //LevelService levelService = new LevelService(_dbc);
            //Level level = levelService.findById(id_of_level);

            if (id_of_level != 0)
            {
                /*
                var lessonsJoin = q.Join(_dbc.ConnectionsLessonToLevel,
                    p => p,
                    c => c.lesson,
                    (p,c) => new
                    {
                        name = p.name
                    }
                );
                */
                /*
                LevelService levelService = new LevelService(this._dbc);
                Level level = levelService.findById(id_of_level);
                if (level != null)
                {
                    q = q.Where(p => p.level == level);
                }
                levelService = null;
                */
            }

            if (skip == 0)
            {
                q = q.Take(take);
            } else
            {
                q = q.Skip(skip).Take(take);
            }

            return q.ToList();
        }


        
        public Lesson save(LessonDTO lessonDTO)
        {
            Lesson lesson = findById(lessonDTO.id);

            if (lesson == null) return null;

            lesson.name = lessonDTO.name;
            lesson.shortName = lessonDTO.shortName;
            lesson.musicName = lessonDTO.musicName;
            lesson.active = lessonDTO.active;
            lesson.isVisible = lessonDTO.isVisible;
            lesson.price = lessonDTO.price;
            lesson.days = lessonDTO.days;
            lesson.isFree = lessonDTO.isFree;
            lesson.description = lessonDTO.description;
            /*
            if(lessonDTO.idOfLevel != 0)
            {
                LevelService levelService = new LevelService(_dbc);
                Level level = levelService.findById(lessonDTO.idOfLevel);
                if (level != null) lesson.level = level;
            } else
            {
                lesson.level = null;
            }
            */
            if(lessonDTO.idOfLessonType != 0)
            {
                LessonTypeService lessonTypeService = new LessonTypeService(_dbc);
                LessonType lessonType = lessonTypeService.findById(lessonDTO.idOfLessonType);
                if (lessonType != null) lesson.lessonType = lessonType;
            }
            else
            {
                lesson.lessonType = null;
            }
            if (lessonDTO.idOfStyle != 0)
            {
                StyleService styleService = new StyleService(_dbc);
                Style style = styleService.findById(lessonDTO.idOfStyle);
                if (style != null) lesson.style = style;
            }
            else
            {
                lesson.style = null;
            }
            if (lessonDTO.idOfTeacher != 0)
            {
                TeacherService teacherService = new TeacherService(_dbc);
                Teacher teacher = teacherService.findById(lessonDTO.idOfTeacher);
                if (teacher != null) lesson.teacher = teacher;
            }
            else
            {
                lesson.teacher = null;
            }
            if (lessonDTO.idOfVideo != 0)
            {
                VideoService videoService = new VideoService(_dbc);
                Video video = videoService.findById(lessonDTO.idOfVideo);
                if (video != null) lesson.video = video;
            }
            else
            {
                lesson.video = null;
            }





            this._dbc.SaveChanges();

            return lesson;
        }



        public bool delete(int id)
        {
            Lesson lesson = findById(id);
            this._dbc.Lessons.Remove(lesson);
            this._dbc.SaveChanges();
            return true;
        }

        public bool changeOrder(LessonOrderDTO lessonOrderDTO)
        {
            Lesson lessonCurrent = findById(lessonOrderDTO.id_of_lesson);

            Lesson lessonForChangingOrderInList = new Lesson();
            switch (lessonOrderDTO.change_order_to_0_down_1_up)
            {
                case 0:
                    lessonForChangingOrderInList = findPrevOfById(lessonCurrent.id, lessonCurrent.orderInList);
                    break;
                case 1:
                    lessonForChangingOrderInList = findNextOfById(lessonCurrent.id, lessonCurrent.orderInList);
                    break;
                default:
                    break;
            }

            if (lessonForChangingOrderInList == null) return false;

            changeOrderOfEachOther(
                lessonCurrent,
                lessonForChangingOrderInList
            );
            _dbc.SaveChanges();

            return true;
        }

        private void changeOrderOfEachOther(Lesson lesson0, Lesson lesson1)
        {
            int orderMemory = lesson0.orderInList;
            lesson0.orderInList = lesson1.orderInList;
            lesson1.orderInList = orderMemory;
        }
    }
}
