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

        public Package findById(int id)
        {
            return _dbc.Packages
                .Include(p => p.level)
                .Include(p => p.style)
                .Include(p => p.teacher)
                .Include(p => p.tariff)
                .FirstOrDefault(p => p.id == id);
        }
        public Package findActiveById(int id)
        {
            return _dbc.Packages
                .Where(p => p.active == 1)
                .FirstOrDefault(p => p.id == id);
        }

        public IEnumerable<Package> listAll()
        {
            return _dbc.Packages
                .OrderByDescending(p => p.orderInList)
                .Include(p => p.level)
                .Include(p => p.style)
                .Include(p => p.teacher)
                .Include(p => p.tariff)
                .ToList();
        }

        public IEnumerable<Package> listAllOnlyPrivateActive(int skip, int take)
        {
            if (skip < 0) skip = 0;
            if (take < 0) take = 18;
            return _dbc.Packages
                .Include(p => p.level)
                .Include(p => p.style)
                .Include(p => p.teacher)
                .Include(p => p.tariff)
                .Where(p => p.active == 2)
                .Skip(skip)
                .Take(take)
                .OrderByDescending(p => p.orderInList)
                .ToList();
        }

        public IEnumerable<Package> listAllOnlyPublicActive(int skip, int take)
        {
            if (skip < 0) skip = 0;
            if (take < 0) take = 18;
            return _dbc.Packages
                .Include(p => p.level)
                .Include(p => p.style)
                .Include(p => p.teacher)
                .Include(p => p.tariff)
                .Where(p => p.active == 1)
                .Skip(skip)
                .Take(take)
                .OrderByDescending(p => p.orderInList)
                .ToList();
        }

        public IEnumerable<Package> listAllActive(int skip, int take)
        {
            if (skip < 0) skip = 0;
            if (take < 0) take = 18;
            return _dbc.Packages
                .Include(p => p.level)
                .Include(p => p.style)
                .Include(p => p.teacher)
                .Include(p => p.tariff)
                .Where(p => p.active >= 1)
                .Skip(skip)
                .Take(take)
                .OrderByDescending(p => p.orderInList)
                .ToList();
        }

        public IEnumerable<Package> listAllActivePrivate()
        {
            return _dbc.Packages.Where(p => p.active == 2).OrderByDescending(p => p.orderInList).ToList();
        }

        public IEnumerable<Package> listAllCuratorActive()
        {
            return _dbc.Packages.Where(p => p.active == 1).OrderByDescending(p => p.orderInList).ToList();
        }

        public bool add(PackageNewDTO packageNewDTO)
        {
            Package package = new Package();
            package.name = packageNewDTO.name;
            package.days = 90;
            _dbc.Packages.Add(package);
            _dbc.SaveChanges();

            package.orderInList = package.id;
            _dbc.SaveChanges();


            return true;
        }

        public bool delete(int id)
        {
            Package package = _dbc.Packages.Where(p => p.id == id).FirstOrDefault();
            if (package == null) return false;
            this._dbc.Packages.Remove(package);
            this._dbc.SaveChanges();
            return true;
        }

        public bool delete(Package package)
        {
            this._dbc.Packages.Remove(package);
            this._dbc.SaveChanges();
            return true;
        }

        public bool update(PackageDTO packageDTO, Level level, Style style, Teacher teacher, Tariff tariff)
        {
            Package package = findById(packageDTO.id);
            if (package == null) return false;
            package.name = packageDTO.name;
            package.active = packageDTO.active;

            if(package.priceWithChat == 0 && packageDTO.active == 2 && teacher != null)
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
                if (package.priceWithChat == 0) package.priceWithChat = price_from_teacher;
                
            } else
            {
                package.priceWithChat = packageDTO.priceWithChat;
            }


            package.price = packageDTO.price;
            package.priceFake = packageDTO.priceFake;
            package.priceWithChatFake = packageDTO.priceWithChatFake;
            package.days = packageDTO.days;
            package.description = packageDTO.description;
            package.statusOfCouldBePurchasedBlank0OnlyChat2BlankOrChat3 = packageDTO.statusOfCouldBePurchasedBlank0OnlyChat2BlankOrChat3;

            if (package.level != level) package.level = level;
            if (package.style != style) package.style = style;
            if (package.teacher != teacher) package.teacher = teacher;
            if (package.tariff != tariff) package.tariff = tariff;

            this._dbc.SaveChanges();

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
