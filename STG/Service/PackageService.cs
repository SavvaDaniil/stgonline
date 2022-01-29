using Microsoft.EntityFrameworkCore;
using STG.Data;
using STG.DTO.Package;
using STG.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace STG.Service
{
    public class PackageService
    {
        private ApplicationDbContext _dbc;

        public PackageService(ApplicationDbContext dbc)
        {
            this._dbc = dbc;
        }

        public async Task<Package> findById(int id)
        {
            return await _dbc.Packages
                .Include(p => p.level)
                .Include(p => p.style)
                .Include(p => p.teacher)
                .Include(p => p.tariff)
                .FirstOrDefaultAsync(p => p.id == id);
        }
        public async Task<Package> findActiveById(int id)
        {
            return await _dbc.Packages
                .Where(p => p.active == 1)
                .FirstOrDefaultAsync(p => p.id == id);
        }

        public async Task<IEnumerable<Package>> listAll()
        {
            return await _dbc.Packages
                .OrderByDescending(p => p.orderInList)
                .Include(p => p.level)
                .Include(p => p.style)
                .Include(p => p.teacher)
                .Include(p => p.tariff)
                .ToListAsync();
        }

        public async Task<IEnumerable<Package>> listAllPrivateActive()
        {
            return await _dbc.Packages
                .Include(p => p.level)
                .Include(p => p.style)
                .Include(p => p.teacher)
                .Include(p => p.tariff)
                .Where(p => p.active == 2)
                .OrderByDescending(p => p.orderInList)
                .ToListAsync();
        }

        public async Task<IEnumerable<Package>> listAllActive()
        {
            return await _dbc.Packages
                .Where(p => p.active == 1)
                .OrderByDescending(p => p.orderInList)
                .ToListAsync();
        }

        public async Task<IEnumerable<Package>> listAllActivePrivate()
        {
            return await _dbc.Packages.Where(p => p.active == 2).OrderByDescending(p => p.orderInList).ToListAsync();
        }

        public async Task<IEnumerable<Package>> listAllCuratorActive()
        {
            return await _dbc.Packages.Where(p => p.active == 1).OrderByDescending(p => p.orderInList).ToListAsync();
        }

        public async Task<bool> add(PackageNewDTO packageNewDTO)
        {
            Package package = new Package();
            package.name = packageNewDTO.name;
            package.days = 90;
            await _dbc.Packages.AddAsync(package);
            await _dbc.SaveChangesAsync();

            package.orderInList = package.id;
            await _dbc.SaveChangesAsync();


            return true;
        }

        public async Task<bool> delete(int id)
        {
            Package package = await _dbc.Packages.Where(p => p.id == id).FirstOrDefaultAsync();
            if (package == null) return false;
            this._dbc.Packages.Remove(package);
            await this._dbc.SaveChangesAsync();
            return true;
        }

        public async Task<bool> delete(Package package)
        {
            this._dbc.Packages.Remove(package);
            await this._dbc.SaveChangesAsync();
            return true;
        }

        public async Task<bool> update(PackageDTO packageDTO, Level level, Style style, Teacher teacher, Tariff tariff)
        {
            Package package = await findById(packageDTO.id);
            if (package == null) return false;
            package.name = packageDTO.name;
            package.active = packageDTO.active;

            if(packageDTO.active == 2)
            {
                if (teacher != null)
                {
                    int price_from_teacher = 0;
                    switch (packageDTO.id_of_tariff)
                    {
                        case 1:
                            price_from_teacher = (teacher != null ? (teacher.priceTariff1 - 1000) : 0);
                            break;
                        case 2:
                            price_from_teacher = (teacher != null ? (teacher.priceTariff2 - 1000) : 0);
                            break;
                        case 3:
                            price_from_teacher = (teacher != null ? (teacher.priceTariff3 - 1000) : 0);
                            break;
                        default:
                            break;
                    }

                    if (price_from_teacher < 0) price_from_teacher = 0;
                    package.price = price_from_teacher;
                }
            } else
            {
                package.price = packageDTO.price;
            }

            package.days = packageDTO.days;
            package.description = packageDTO.description;
            package.statusOfChatNone0Homework1AndChat2 = packageDTO.statusOfChatNone0Homework1AndChat2;

            if (package.level != level) package.level = level;
            if (package.style != style) package.style = style;
            if (package.teacher != teacher) package.teacher = teacher;
            if (package.tariff != tariff) package.tariff = tariff;

            await this._dbc.SaveChangesAsync();

            return true;
        }

        /*
        

            public function listAllCuratorActive(): ?array{
                return Program::find()
                -> where(["active" => 1])
                -> andWhere(["is_curator" => 1])
                -> orderBy(["order_in_list" => SORT_DESC])
                -> all();
            }


            public function update(ProgramDTO $programDTO): bool {

                $program = Program::find()
                -> where("id = :id", ["id" => $programDTO -> id])
                -> one();
                if($program == null)return false;

                if($programDTO -> active < 0 && $programDTO -> active > 2)$programDTO -> active = 0;
                if($programDTO -> price < 0)$programDTO -> price = 0;

                $program -> name = $programDTO -> name;
                $program -> price = $programDTO -> price;
                $program -> days = $programDTO -> days;
                $program -> active = $programDTO -> active;
                $program -> description = $programDTO -> description;
        
                $program -> id_of_level = $programDTO -> id_of_level;
                $program -> id_of_style = $programDTO -> id_of_style;
                $program -> id_of_teacher = $programDTO -> id_of_teacher;

                return $program -> save();
            } 
        */
    }
}
