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

        public async Task<ConnectionLessonToLevel> findById(int id)
        {
            return await _dbc.ConnectionsLessonToLevel
                .Where(p => p.id == id)
                .FirstOrDefaultAsync();
        }

        public async Task<ConnectionLessonToLevel> find(Level level, Lesson lesson)
        {
            return await _dbc.ConnectionsLessonToLevel
                .Where(p => p.level == level && p.lesson == lesson)
                .FirstOrDefaultAsync();
        }

        public async Task<ConnectionLessonToLevel> add(Level level, Lesson lesson)
        {
            ConnectionLessonToLevel connectionLessonToLevel = await find(level, lesson);
            if (connectionLessonToLevel != null) return connectionLessonToLevel;
            connectionLessonToLevel = new ConnectionLessonToLevel();
            connectionLessonToLevel.level = level;
            connectionLessonToLevel.lesson = lesson;
            connectionLessonToLevel.dateOfAdd = DateTime.Now;

            await _dbc.ConnectionsLessonToLevel.AddAsync(connectionLessonToLevel);
            await _dbc.SaveChangesAsync();

            return connectionLessonToLevel;
        }

        public async Task<bool> deleteAll(Lesson lesson)
        {
            List<ConnectionLessonToLevel> connectionLessonToLevels = await listAllByLesson(lesson);
            foreach (ConnectionLessonToLevel connectionLessonToLevel in connectionLessonToLevels)
            {
                this._dbc.ConnectionsLessonToLevel.Remove(connectionLessonToLevel);
                await this._dbc.SaveChangesAsync();
            }
            return true;
        }

        public async Task<bool> delete(Level level, Lesson lesson)
        {
            ConnectionLessonToLevel connectionLessonToLevel = await find(level, lesson);
            if (connectionLessonToLevel == null) return false;
            this._dbc.ConnectionsLessonToLevel.Remove(connectionLessonToLevel);
            await this._dbc.SaveChangesAsync();
            return true;
        }

        public async Task<bool> delete(int id)
        {
            ConnectionLessonToLevel connectionLessonToLevel = await findById(id);
            if (connectionLessonToLevel == null) return false;
            this._dbc.ConnectionsLessonToLevel.Remove(connectionLessonToLevel);
            await this._dbc.SaveChangesAsync();
            return true;
        }
        public async Task<bool> delete(ConnectionLessonToLevel connectionLessonToLevel)
        {
            this._dbc.ConnectionsLessonToLevel.Remove(connectionLessonToLevel);
            await this._dbc.SaveChangesAsync();
            return true;
        }

        public async Task<List<ConnectionLessonToLevel>> listAllByLesson(Lesson lesson)
        {
            return await _dbc.ConnectionsLessonToLevel
                .Include(p => p.level)
                .Where(p => p.lesson == lesson)
                .ToListAsync();
        }

        public async Task<List<ConnectionLessonToLevel>> listAll()
        {
            return await _dbc.ConnectionsLessonToLevel
                .ToListAsync();
        }

        public async Task<string[]> arrayOfLevelName(Lesson lesson)
        {
            List<ConnectionLessonToLevel> connectionLessonToLevels = await listAllByLesson(lesson);
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
