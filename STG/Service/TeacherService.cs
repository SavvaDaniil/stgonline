using Microsoft.EntityFrameworkCore;
using STG.Data;
using STG.DTO.Teacher;
using STG.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace STG.Service
{
    public class TeacherService
    {
        private ApplicationDbContext _dbc;

        public TeacherService(ApplicationDbContext dbc)
        {
            this._dbc = dbc;
        }

        public Teacher add(TeacherNewDTO teacherNewDTO)
        {
            Teacher teacher = new Teacher();
            teacher.name = teacherNewDTO.fio;
            teacher.instagram = teacherNewDTO.instagram;

            this._dbc.Teachers.Add(teacher);

            this._dbc.SaveChanges();
            teacher.orderInList = teacher.id;
            this._dbc.SaveChanges();

            return teacher;
        }

        public Teacher findById(int id)
        {
            return this._dbc.Teachers.FirstOrDefault(p => p.id == id);
        }

        public Teacher save(TeacherDTO teacherDTO)
        {
            Teacher teacher = findById(teacherDTO.id);

            if (teacher == null) return null;

            teacher.name = teacherDTO.name;
            teacher.email = teacherDTO.email;
            teacher.instagram = teacherDTO.instagram;
            teacher.shortDescription = teacherDTO.shortDescription;
            teacher.description = teacherDTO.description;
            teacher.mentor_bio = teacherDTO.mentorBio;
            teacher.mentor_awards = teacherDTO.mentorAwards;
            teacher.active = teacherDTO.active;
            teacher.isCurator = teacherDTO.is_curator;
            teacher.priceCurator = teacherDTO.price_curator;
            teacher.priceTariff1 = teacherDTO.price_tariff_1;
            teacher.priceTariff2 = teacherDTO.price_tariff_2;
            teacher.priceTariff3 = teacherDTO.price_tariff_3;

            this._dbc.SaveChanges();

            return teacher;
        }

        public bool delete(int id)
        {
            Teacher teacher = findById(id);
            this._dbc.Teachers.Remove(teacher);
            this._dbc.SaveChanges();
            return true;
        }

        public Teacher findByid(int id)
        {
            return this._dbc.Teachers.FirstOrDefault(p => p.id == id);
        }

        public List<Teacher> listAll()
        {
            return this._dbc.Teachers
                .OrderByDescending(p => p.orderInList)
                .ToList();
        }

        public List<Teacher> listAllActive()
        {
            return this._dbc.Teachers.Where(p => p.active == 1)
                .OrderByDescending(p => p.orderInList)
                .ToList();
        }

        public IEnumerable<Teacher> listAllActiveCurator()
        {
            return this._dbc.Teachers
                .Where(p => p.active == 1 && p.isCurator == 1)
                .OrderByDescending(p => p.orderInList).ToList();
        }

        public IEnumerable<Teacher> listAllActiveNotCurator()
        {
            return this._dbc.Teachers
                .Where(p => p.active == 1 && p.isCurator == 0)
                .OrderByDescending(p => p.orderInList).ToList();
        }

        public List<Teacher> listAllByListString(string listOfIdOfTeacher)
        {
            List<Teacher> listOfTeachers = new List<Teacher>();
            Teacher teacher;
            foreach (string id_of_teacher in listOfIdOfTeacher.Split(","))
            {
                try
                {
                    teacher = findById(int.Parse(id_of_teacher));
                    if (teacher != null) listOfTeachers.Add(teacher);
                }
                catch { }
            }

            return listOfTeachers;
        }

        public Teacher findPrevOfById(int id_of_current, int order_of_current)
        {
            return _dbc.Teachers
                .Where(p => p.id != id_of_current)
                .Where(p => p.orderInList < order_of_current)
                .OrderByDescending(p => p.orderInList)
                .FirstOrDefault();
        }
        public Teacher findNextOfById(int id_of_current, int order_of_current)
        {
            return _dbc.Teachers
                .Where(p => p.id != id_of_current)
                .Where(p => p.orderInList > order_of_current)
                .OrderBy(p => p.orderInList)
                .FirstOrDefault();
        }

        public bool changeOrder(TeacherOrderDTO teacherOrderDTO)
        {
            Teacher teacherCurrent = findById(teacherOrderDTO.id_of_teacher);

            Teacher lessonForChangingOrderInList = new Teacher();
            switch (teacherOrderDTO.change_order_to_0_down_1_up)
            {
                case 0:
                    lessonForChangingOrderInList = findPrevOfById(teacherCurrent.id, teacherCurrent.orderInList);
                    break;
                case 1:
                    lessonForChangingOrderInList = findNextOfById(teacherCurrent.id, teacherCurrent.orderInList);
                    break;
                default:
                    break;
            }

            if (lessonForChangingOrderInList == null) return false;

            changeOrderOfEachOther(
                teacherCurrent,
                lessonForChangingOrderInList
            );
            _dbc.SaveChanges();

            return true;
        }

        private void changeOrderOfEachOther(Teacher teacher0, Teacher teacher1)
        {
            int orderMemory = teacher0.orderInList;
            teacher0.orderInList = teacher1.orderInList;
            teacher1.orderInList = orderMemory;
        }
    }
}
