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

        public async Task<PackageDay> findById(int id)
        {
            return await _dbc.PackageDays.FirstOrDefaultAsync(p => p.id == id);
        }

        public async Task<bool> add(PackageDayNewDTO packageDayNewDTO, Package package)
        {
            PackageDay packageDay = new PackageDay();
            packageDay.package = package;
            packageDay.name = packageDayNewDTO.name;

            await _dbc.PackageDays.AddAsync(packageDay);
            await _dbc.SaveChangesAsync();
            packageDay.orderInList = packageDay.id;
            await _dbc.SaveChangesAsync();
            return true;
        }

        public async Task<List<PackageDay>> listAllByPackage(Package package)
        {
            return await this._dbc.PackageDays
                .Where(p => p.package == package)
                .OrderBy(p => p.orderInList)
                .ToListAsync();
        }

        public async Task<bool> delete(int id)
        {
            PackageDay packageDay = await findById(id);
            if (packageDay == null) return false;
            _dbc.PackageDays.Remove(packageDay);
            await _dbc.SaveChangesAsync();
            return true;
        }

        public async Task<bool> deleteAllByPackage(Package package)
        {
            List<PackageDay> packageDays = await _dbc.PackageDays.Where(p => p.package == package).ToListAsync();
            foreach (PackageDay packageDay in packageDays)
            {
                _dbc.PackageDays.Remove(packageDay);
            }
            await _dbc.SaveChangesAsync();

            return true;
        }

        public async Task<bool> update(PackageDayDTO packageDayDTO)
        {
            PackageDay packageDay = await findById(packageDayDTO.id);
            if (packageDay == null) return false;

            packageDay.name = packageDayDTO.name;
            await _dbc.SaveChangesAsync();
            return true;
        }
    }
}
