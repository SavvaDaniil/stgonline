using Microsoft.EntityFrameworkCore;
using STG.Data;
using STG.DTO;
using STG.DTO.LessonType;
using STG.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace STG.Service
{
    public class LessonTypeService
    {
        private ApplicationDbContext _dbc;

        public LessonTypeService(ApplicationDbContext dbc)
        {
            this._dbc = dbc;
        }
        public async Task<IEnumerable<LessonType>> listAll()
        {
            return await this._dbc.LessonTypes.OrderByDescending(p => p.id).ToListAsync();
        }

        public async Task<List<LessonType>> listAllActive()
        {
            return await this._dbc.LessonTypes.Where(p => p.active == 1).ToListAsync();
        }

        public async Task<LessonType> findById(int id)
        {
            return await this._dbc.LessonTypes.FirstOrDefaultAsync(p => p.id == id);
        }

        public async Task<LessonType> add(LessonTypeNewDTO lessonTypeNewDTO)
        {
            LessonType lessonType = new LessonType();
            lessonType.name = lessonTypeNewDTO.name;

            this._dbc.LessonTypes.Add(lessonType);

            await this._dbc.SaveChangesAsync();

            return lessonType;
        }

        public async Task<bool> update(LessonTypeDTO lessonTypeDTO)
        {
            LessonType lessonType = await findById(lessonTypeDTO.id);

            if (lessonType == null) return false;

            lessonType.name = lessonTypeDTO.name;
            lessonType.active = lessonTypeDTO.active;

            await this._dbc.SaveChangesAsync();

            return true;
        }

        public async Task<bool> delete(int id)
        {
            LessonType lessonType = await findById(id);
            this._dbc.LessonTypes.Remove(lessonType);
            await this._dbc.SaveChangesAsync();
            return true;
        }
    }
}
