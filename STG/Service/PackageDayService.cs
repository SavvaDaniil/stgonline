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
    public class PackageDayService
    {
        private ApplicationDbContext _dbc;
        public PackageDayService(ApplicationDbContext dbc)
        {
            this._dbc = dbc;
        }

        public PackageDay findById(int id)
        {
            return _dbc.PackageDays.FirstOrDefault(p => p.id == id);
        }

        public bool add(PackageDayNewDTO packageDayNewDTO, Package package)
        {
            PackageDay packageDay = new PackageDay();
            packageDay.package = package;
            packageDay.name = packageDayNewDTO.name;

            _dbc.PackageDays.Add(packageDay);
            _dbc.SaveChanges();
            packageDay.orderInList = packageDay.id;
            _dbc.SaveChanges();
            return true;
        }

        public List<PackageDay> listAllByPackage(Package package)
        {
            return this._dbc.PackageDays
                .Where(p => p.package == package)
                .OrderBy(p => p.orderInList)
                .ToList();
        }

        public bool delete(int id)
        {
            PackageDay packageDay = findById(id);
            if (packageDay == null) return false;
            _dbc.PackageDays.Remove(packageDay);
            _dbc.SaveChanges();
            return true;
        }

        public bool deleteAllByPackage(Package package)
        {
            List<PackageDay> packageDays = _dbc.PackageDays.Where(p => p.package == package).ToList();
            foreach (PackageDay packageDay in packageDays)
            {
                _dbc.PackageDays.Remove(packageDay);
            }
            _dbc.SaveChanges();

            return true;
        }

        public bool update(PackageDayDTO packageDayDTO)
        {
            PackageDay packageDay = findById(packageDayDTO.id);
            if (packageDay == null) return false;

            packageDay.name = packageDayDTO.name;
            _dbc.SaveChanges();
            return true;
        }
    }
}
