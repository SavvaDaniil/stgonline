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

        public ConnectionUserToPrivatePackage findById(int id)
        {
            return _dbc.ConnectionsUserToPrivatePackage
                .Include(p => p.package)
                .Include(p => p.user)
                .FirstOrDefault(p => p.id == id);
        }

        public bool isAny(User user, Package package)
        {
            return _dbc.ConnectionsUserToPrivatePackage
                .Where(p => p.user == user && p.package == package)
                .Any();
        }

        public ConnectionUserToPrivatePackage find(User user, Package package)
        {
            return _dbc.ConnectionsUserToPrivatePackage
                .OrderByDescending(p => p.id)
                .FirstOrDefault(p => p.user == user && p.package == package);
        }

        public ConnectionUserToPrivatePackage find(User user, int id_of_package)
        {
            return _dbc.ConnectionsUserToPrivatePackage
                .Include(p => p.package)
                .Include(p => p.user)
                .OrderByDescending(p => p.id)
                .FirstOrDefault(p => p.user == user && p.package.id == id_of_package);
        }

        public ConnectionUserToPrivatePackage add(User user, Package package)
        {
            ConnectionUserToPrivatePackage connectionUserToPrivatePackage = find(user, package);
            if (connectionUserToPrivatePackage != null) return connectionUserToPrivatePackage;

            connectionUserToPrivatePackage = new ConnectionUserToPrivatePackage();
            connectionUserToPrivatePackage.user = user;
            connectionUserToPrivatePackage.package = package;
            connectionUserToPrivatePackage.date_of_add = DateTime.Now;

            _dbc.ConnectionsUserToPrivatePackage.Add(connectionUserToPrivatePackage);
            _dbc.SaveChanges();

            return connectionUserToPrivatePackage;
        }

        public List<ConnectionUserToPrivatePackage> listAllByUser(User user)
        {
            return this._dbc.ConnectionsUserToPrivatePackage
                .Include(p => p.package)
                .Where(p => p.user == user)
                .OrderByDescending(p => p.id)
                .ToList();
        }
        public List<ConnectionUserToPrivatePackage> listAll()
        {
            return _dbc.ConnectionsUserToPrivatePackage
                .Include(p => p.package)
                .Include(p => p.user)
                .OrderByDescending(p => p.date_of_add)
                .ToList();
        }

        public List<int> listIdAllByUser(User user)
        {
            IEnumerable<ConnectionUserToPrivatePackage> connectionUserToPrivatePackages = listAllByUser(user);
            List<int> array = new List<int>();
            foreach(ConnectionUserToPrivatePackage connectionUserToPrivatePackage in connectionUserToPrivatePackages)
            {
                array.Add(connectionUserToPrivatePackage.package.id);
            }
            return array;
        }

        public bool delete(User user, Package package)
        {
            ConnectionUserToPrivatePackage connectionUserToPrivatePackage = find(user, package);
            if (connectionUserToPrivatePackage == null) return false;
            this._dbc.ConnectionsUserToPrivatePackage.Remove(connectionUserToPrivatePackage);
            this._dbc.SaveChanges();
            return true;
        }

        public bool deleteAllByPackage(Package package)
        {
            List<ConnectionUserToPrivatePackage> connectionUserToPrivatePackages = _dbc.ConnectionsUserToPrivatePackage.Where(p => p.package == package).ToList();

            foreach (ConnectionUserToPrivatePackage connectionUserToPrivatePackage in connectionUserToPrivatePackages)
            {
                this._dbc.ConnectionsUserToPrivatePackage.Remove(connectionUserToPrivatePackage);
            }

            this._dbc.SaveChanges();
            return true;
        }

        public bool delete(int id)
        {
            ConnectionUserToPrivatePackage connectionUserToPrivatePackage = findById(id);
            if (connectionUserToPrivatePackage == null) return false;
            this._dbc.ConnectionsUserToPrivatePackage.Remove(connectionUserToPrivatePackage);
            this._dbc.SaveChanges();
            return true;
        }
    }
}
