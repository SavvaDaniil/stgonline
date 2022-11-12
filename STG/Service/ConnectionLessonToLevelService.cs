using Microsoft.EntityFrameworkCore;
using STG.Data;
using STG.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace STG.Service
{
    public class ConnectionLessonToLevelService
    {
        private ApplicationDbContext _dbc;
        public ConnectionLessonToLevelService(ApplicationDbContext dbc)
        {
            this._dbc = dbc;
        }

        public ConnectionLessonToLevel findById(int id)
        {
            return _dbc.ConnectionsLessonToLevel
                .Where(p => p.id == id)
                .FirstOrDefault();
        }

        public ConnectionLessonToLevel find(Level level, Lesson lesson)
        {
            return _dbc.ConnectionsLessonToLevel
                .Where(p => p.level == level && p.lesson == lesson)
                .FirstOrDefault();
        }

        public ConnectionLessonToLevel add(Level level, Lesson lesson)
        {
            ConnectionLessonToLevel connectionLessonToLevel = find(level, lesson);
            if (connectionLessonToLevel != null) return connectionLessonToLevel;
            connectionLessonToLevel = new ConnectionLessonToLevel();
            connectionLessonToLevel.level = level;
            connectionLessonToLevel.lesson = lesson;
            connectionLessonToLevel.dateOfAdd = DateTime.Now;

            _dbc.ConnectionsLessonToLevel.Add(connectionLessonToLevel);
            _dbc.SaveChanges();

            return connectionLessonToLevel;
        }

        public bool deleteAll(Lesson lesson)
        {
            List<ConnectionLessonToLevel> connectionLessonToLevels = listAllByLesson(lesson);
            foreach (ConnectionLessonToLevel connectionLessonToLevel in connectionLessonToLevels)
            {
                this._dbc.ConnectionsLessonToLevel.Remove(connectionLessonToLevel);
                this._dbc.SaveChanges();
            }
            return true;
        }

        public bool delete(Level level, Lesson lesson)
        {
            ConnectionLessonToLevel connectionLessonToLevel = find(level, lesson);
            if (connectionLessonToLevel == null) return false;
            this._dbc.ConnectionsLessonToLevel.Remove(connectionLessonToLevel);
            this._dbc.SaveChanges();
            return true;
        }

        public bool delete(int id)
        {
            ConnectionLessonToLevel connectionLessonToLevel = findById(id);
            if (connectionLessonToLevel == null) return false;
            this._dbc.ConnectionsLessonToLevel.Remove(connectionLessonToLevel);
            this._dbc.SaveChanges();
            return true;
        }
        public bool delete(ConnectionLessonToLevel connectionLessonToLevel)
        {
            this._dbc.ConnectionsLessonToLevel.Remove(connectionLessonToLevel);
            this._dbc.SaveChanges();
            return true;
        }

        public List<ConnectionLessonToLevel> listAllByLesson(Lesson lesson)
        {
            return _dbc.ConnectionsLessonToLevel
                .Include(p => p.level)
                .Where(p => p.lesson == lesson)
                .ToList();
        }

        public List<ConnectionLessonToLevel> listAll()
        {
            return _dbc.ConnectionsLessonToLevel
                .ToList();
        }

        public string[] arrayOfLevelName(Lesson lesson)
        {
            List<ConnectionLessonToLevel> connectionLessonToLevels = listAllByLesson(lesson);
            List<string> levelNames = new List<string>();
            foreach (ConnectionLessonToLevel connectionLessonToLevel in connectionLessonToLevels)
            {
                if (connectionLessonToLevel.level == null) continue;
                levelNames.Add(connectionLessonToLevel.level.Name);
            }
            return levelNames.ToArray();
        }

    }
}
