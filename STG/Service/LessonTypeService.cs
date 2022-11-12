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
        public IEnumerable<LessonType> listAll()
        {
            return this._dbc.LessonTypes.OrderByDescending(p => p.id).ToList();
        }

        public List<LessonType> listAllActive()
        {
            return this._dbc.LessonTypes.Where(p => p.active == 1).ToList();
        }

        public LessonType findById(int id)
        {
            return this._dbc.LessonTypes.FirstOrDefault(p => p.id == id);
        }

        public LessonType add(LessonTypeNewDTO lessonTypeNewDTO)
        {
            LessonType lessonType = new LessonType();
            lessonType.name = lessonTypeNewDTO.name;

            this._dbc.LessonTypes.Add(lessonType);

            this._dbc.SaveChanges();

            return lessonType;
        }

        public bool update(LessonTypeDTO lessonTypeDTO)
        {
            LessonType lessonType = findById(lessonTypeDTO.id);

            if (lessonType == null) return false;

            lessonType.name = lessonTypeDTO.name;
            lessonType.active = lessonTypeDTO.active;

            this._dbc.SaveChanges();

            return true;
        }

        public bool delete(int id)
        {
            LessonType lessonType = findById(id);
            this._dbc.LessonTypes.Remove(lessonType);
            this._dbc.SaveChanges();
            return true;
        }
    }
}
