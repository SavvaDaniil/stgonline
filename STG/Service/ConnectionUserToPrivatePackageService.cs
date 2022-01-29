using Microsoft.EntityFrameworkCore;
using STG.Data;
using STG.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace STG.Service
{
    public class ConnectionUserToPrivatePackageService
    {
        private ApplicationDbContext _dbc;

        public ConnectionUserToPrivatePackageService(ApplicationDbContext dbc)
        {
            this._dbc = dbc;
        }

        public async Task<ConnectionUserToPrivatePackage> findById(int id)
        {
            return await _dbc.ConnectionsUserToPrivatePackage
                .Include(p => p.package)
                .Include(p => p.user)
                .FirstOrDefaultAsync(p => p.id == id);
        }

        public async Task<bool> isAny(User user, Package package)
        {
            return await _dbc.ConnectionsUserToPrivatePackage
                .Where(p => p.user == user && p.package == package)
                .AnyAsync();
        }

        public async Task<ConnectionUserToPrivatePackage> find(User user, Package package)
        {
            return await _dbc.ConnectionsUserToPrivatePackage
                .OrderByDescending(p => p.id)
                .FirstOrDefaultAsync(p => p.user == user && p.package == package);
        }

        public async Task<ConnectionUserToPrivatePackage> find(User user, int id_of_package)
        {
            return await _dbc.ConnectionsUserToPrivatePackage
                .Include(p => p.package)
                .Include(p => p.user)
                .OrderByDescending(p => p.id)
                .FirstOrDefaultAsync(p => p.user == user && p.package.id == id_of_package);
        }

        public async Task<ConnectionUserToPrivatePackage> add(User user, Package package)
        {
            ConnectionUserToPrivatePackage connectionUserToPrivatePackage = await find(user, package);
            if (connectionUserToPrivatePackage != null) return connectionUserToPrivatePackage;

            connectionUserToPrivatePackage = new ConnectionUserToPrivatePackage();
            connectionUserToPrivatePackage.user = user;
            connectionUserToPrivatePackage.package = package;
            connectionUserToPrivatePackage.date_of_add = DateTime.Now;

            await _dbc.ConnectionsUserToPrivatePackage.AddAsync(connectionUserToPrivatePackage);
            await _dbc.SaveChangesAsync();

            return connectionUserToPrivatePackage;
        }

        public async Task<List<ConnectionUserToPrivatePackage>> listAllByUser(User user)
        {
            return await this._dbc.ConnectionsUserToPrivatePackage
                .Include(p => p.package)
                .Where(p => p.user == user)
                .OrderByDescending(p => p.id)
                .ToListAsync();
        }
        public async Task<List<ConnectionUserToPrivatePackage>> listAll()
        {
            return await _dbc.ConnectionsUserToPrivatePackage
                .Include(p => p.package)
                .Include(p => p.user)
                .OrderByDescending(p => p.date_of_add)
                .ToListAsync();
        }

        public async Task<List<int>> listIdAllByUser(User user)
        {
            IEnumerable<ConnectionUserToPrivatePackage> connectionUserToPrivatePackages = await listAllByUser(user);
            List<int> array = new List<int>();
            foreach(ConnectionUserToPrivatePackage connectionUserToPrivatePackage in connectionUserToPrivatePackages)
            {
                array.Add(connectionUserToPrivatePackage.package.id);
            }
            return array;
        }

        public async Task<bool> delete(User user, Package package)
        {
            ConnectionUserToPrivatePackage connectionUserToPrivatePackage = await find(user, package);
            if (connectionUserToPrivatePackage == null) return false;
            this._dbc.ConnectionsUserToPrivatePackage.Remove(connectionUserToPrivatePackage);
            await this._dbc.SaveChangesAsync();
            return true;
        }

        public async Task<bool> deleteAllByPackage(Package package)
        {
            List<ConnectionUserToPrivatePackage> connectionUserToPrivatePackages = await _dbc.ConnectionsUserToPrivatePackage.Where(p => p.package == package).ToListAsync();

            foreach (ConnectionUserToPrivatePackage connectionUserToPrivatePackage in connectionUserToPrivatePackages)
            {
                this._dbc.ConnectionsUserToPrivatePackage.Remove(connectionUserToPrivatePackage);
            }

            await this._dbc.SaveChangesAsync();
            return true;
        }

        public async Task<bool> delete(int id)
        {
            ConnectionUserToPrivatePackage connectionUserToPrivatePackage = await findById(id);
            if (connectionUserToPrivatePackage == null) return false;
            this._dbc.ConnectionsUserToPrivatePackage.Remove(connectionUserToPrivatePackage);
            await this._dbc.SaveChangesAsync();
            return true;
        }
    }
}
