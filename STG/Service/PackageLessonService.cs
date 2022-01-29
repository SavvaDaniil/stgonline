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

        public async Task<PackageLesson> findById(int id)
        {
            return await _dbc.PackageLessons
                .Include(p => p.package)
                .Include(p => p.lesson)
                .FirstOrDefaultAsync(p => p.id == id);
        }

        public async Task<PackageLesson> find(Package package, Lesson lesson)
        {
            return await _dbc.PackageLessons
                .OrderByDescending(p => p.id)
                .FirstOrDefaultAsync(p => p.package == package && p.lesson == lesson);
        }

        public async Task<List<PackageLesson>> listAllByLesson(Lesson lesson)
        {
            return await _dbc.PackageLessons
                .Include(p => p.package)
                .Where(p => p.lesson == lesson)
                .OrderBy(p => p.orderInList)
                .ToListAsync();
        }
        public async Task<List<PackageLesson>> listAllByPackage(Package package)
        {
            return await _dbc.PackageLessons
                .Include(p => p.lesson)
                .Where(p => p.package == package)
                .OrderBy(p => p.orderInList)
                .ToListAsync();
        }
        public async Task<List<PackageLesson>> listAllByPackageWithHomeworks(Package package)
        {
            return await _dbc.PackageLessons
                .Where(p => p.package == package && p.homeworkStatus > 0)
                .Include(p => p.lesson)
                .OrderBy(p => p.orderInList)
                .ToListAsync();
        }

        public async Task<int> countAllByPackage(Package package)
        {
            return await _dbc.PackageLessons.CountAsync(p => p.package == package);
        }

        public async Task<List<PackageLesson>> listAllByPackageDay(PackageDay packageDay)
        {
            return await _dbc.PackageLessons.Where(p => p.packageDay == packageDay).ToListAsync();
        }

        public async Task<bool> add(Package package, PackageDay packageDay)
        {
            PackageLesson packageLesson = new PackageLesson();
            packageLesson.package = package;
            packageLesson.packageDay = packageDay;
            packageLesson.dateOfAdd = DateTime.Now;

            await _dbc.PackageLessons.AddAsync(packageLesson);
            await _dbc.SaveChangesAsync();
            packageLesson.orderInList = packageLesson.id;
            await _dbc.SaveChangesAsync();
            return true;
        }

        public async Task<bool> update(PackageLessonDTO packageLessonDTO, Lesson lesson)
        {
            PackageLesson packageLesson = await findById(packageLessonDTO.id);
            if (packageLesson == null) return false;

            packageLesson.homeworkStatus = packageLessonDTO.homework_status;
            packageLesson.homeworkText = packageLessonDTO.homework_text;
            if (lesson != null && packageLesson.lesson != lesson) packageLesson.lesson = lesson;

            await _dbc.SaveChangesAsync();

            return true;
        }

        public async Task<bool> delete(int id)
        {
            PackageLesson packageLesson = await findById(id);
            if (packageLesson == null) return false;
            _dbc.PackageLessons.Remove(packageLesson);
            await _dbc.SaveChangesAsync();
            return true;
        }

        public async Task<bool> deleteAllByPackageDay(PackageDay packageDay)
        {
            List<PackageLesson> packageLessons = await listAllByPackageDay(packageDay);
            foreach (PackageLesson packageLesson in packageLessons)
            {
                _dbc.PackageLessons.Remove(packageLesson);
            }
            await _dbc.SaveChangesAsync();

            return true;
        }

        public async Task<bool> deleteAllByPackage(Package package)
        {
            List<PackageLesson> packageLessons = await _dbc.PackageLessons.Where(p => p.package == package).ToListAsync();
            foreach (PackageLesson packageLesson in packageLessons)
            {
                _dbc.PackageLessons.Remove(packageLesson);
            }
            await _dbc.SaveChangesAsync();

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
