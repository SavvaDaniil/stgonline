using Microsoft.EntityFrameworkCore;
using STG.Data;
using STG.DTO.Package;
using STG.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace STG.Service
{
    public class PackageLessonService
    {
        private ApplicationDbContext _dbc;
        public PackageLessonService(ApplicationDbContext dbc)
        {
            this._dbc = dbc;
        }

        public PackageLesson findById(int id)
        {
            return _dbc.PackageLessons
                .Include(p => p.package)
                .Include(p => p.lesson)
                .FirstOrDefault(p => p.id == id);
        }

        public PackageLesson find(Package package, Lesson lesson)
        {
            return _dbc.PackageLessons
                .OrderByDescending(p => p.id)
                .FirstOrDefault(p => p.package == package && p.lesson == lesson);
        }

        public List<PackageLesson> listAllByLesson(Lesson lesson)
        {
            return _dbc.PackageLessons
                .Include(p => p.package)
                .Where(p => p.lesson == lesson)
                .OrderBy(p => p.orderInList)
                .ToList();
        }
        public List<PackageLesson> listAllByPackage(Package package)
        {
            return _dbc.PackageLessons
                .Include(p => p.lesson)
                .Where(p => p.package == package)
                .OrderBy(p => p.orderInList)
                .ToList();
        }
        public List<PackageLesson> listAllByPackageWithHomeworks(Package package)
        {
            return _dbc.PackageLessons
                .Where(p => p.package == package && p.homeworkStatus > 0)
                .Include(p => p.lesson)
                .OrderBy(p => p.orderInList)
                .ToList();
        }

        public int countAllByPackage(Package package)
        {
            return _dbc.PackageLessons.Count(p => p.package == package);
        }

        public List<PackageLesson> listAllByPackageDay(PackageDay packageDay)
        {
            return _dbc.PackageLessons.Where(p => p.packageDay == packageDay).ToList();
        }

        public bool add(Package package, PackageDay packageDay)
        {
            PackageLesson packageLesson = new PackageLesson();
            packageLesson.package = package;
            packageLesson.packageDay = packageDay;
            packageLesson.dateOfAdd = DateTime.Now;

            _dbc.PackageLessons.Add(packageLesson);
            _dbc.SaveChanges();
            packageLesson.orderInList = packageLesson.id;
            _dbc.SaveChanges();
            return true;
        }

        public bool update(PackageLessonDTO packageLessonDTO, Lesson lesson)
        {
            PackageLesson packageLesson = findById(packageLessonDTO.id);
            if (packageLesson == null) return false;

            packageLesson.homeworkStatus = packageLessonDTO.homework_status;
            packageLesson.homeworkText = packageLessonDTO.homework_text;
            if (lesson != null && packageLesson.lesson != lesson) packageLesson.lesson = lesson;

            _dbc.SaveChanges();

            return true;
        }

        public bool delete(int id)
        {
            PackageLesson packageLesson = findById(id);
            if (packageLesson == null) return false;
            _dbc.PackageLessons.Remove(packageLesson);
            _dbc.SaveChanges();
            return true;
        }

        public bool deleteAllByPackageDay(PackageDay packageDay)
        {
            List<PackageLesson> packageLessons = listAllByPackageDay(packageDay);
            foreach (PackageLesson packageLesson in packageLessons)
            {
                _dbc.PackageLessons.Remove(packageLesson);
            }
            _dbc.SaveChanges();

            return true;
        }

        public bool deleteAllByPackage(Package package)
        {
            List<PackageLesson> packageLessons = _dbc.PackageLessons.Where(p => p.package == package).ToList();
            foreach (PackageLesson packageLesson in packageLessons)
            {
                _dbc.PackageLessons.Remove(packageLesson);
            }
            _dbc.SaveChanges();

            return true;
        }


        /*
        
            public function findById(int $id) {
                return Programlesson::find()
                -> where("id = :id", ["id" => $id])
                -> one();
            }

            public function listAllByIdOfProgram(int $id_of_program){
                return Programlesson::find()
                -> where("id_of_program = :id_of_program", ["id_of_program" => $id_of_program])
                -> orderBy(["order_in_list" => SORT_ASC])
                -> all();
            }

            public function listAllByIdOfProgramAndIdOfProgramday(int $id_of_program, int $id_of_program_day){
                return Programlesson::find()
                -> where("id_of_program = :id_of_program", ["id_of_program" => $id_of_program])
                -> andWhere("id_of_program_day = :id_of_program_day", ["id_of_program_day" => $id_of_program_day])
                -> orderBy(["order_in_list" => SORT_ASC])
                -> all();
            }

            public function add(ProgramlessonNewDTO $programlessonNewDTO): bool{
                $programlesson = new Programlesson();
                $programlesson -> id_of_program = $programlessonNewDTO -> id_of_program;
                $programlesson -> id_of_program_day = $programlessonNewDTO -> id_of_program_day;

                if(!$programlesson -> save())return false;

                $programlesson -> order_in_list = $programlesson -> id;
                return $programlesson -> save();
            }
    
            public function delete(int $id): bool{
                $programlesson = $this -> findById($id);
                if($programlesson == null)return false;
                return $programlesson -> delete();
            }

            public function deleteAllByIdOfProgramday(int $id_of_program_day): bool{

                Programlesson::deleteAll("id_of_program_day = :id_of_program_day", ["id_of_program_day" => $id_of_program_day]);
                return true;
            }


            public function update(ProgramlessonDTO $programlessonDTO): bool {
                $programlesson = $this -> findById($programlessonDTO -> id);
                if($programlesson == null)return false;
        
                $programlesson -> id_of_lesson = $programlessonDTO -> id_of_lesson;
                $programlesson -> homework_status = $programlessonDTO -> homework_status;
                $programlesson -> homework_text = $programlessonDTO -> homework_text;


                return $programlesson -> save();
            }
        */
    }
}
