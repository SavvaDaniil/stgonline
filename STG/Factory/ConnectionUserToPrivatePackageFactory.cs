using STG.Data;
using STG.DTO.Package;
using STG.Entities;
using STG.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace STG.Factory
{
    public class ConnectionUserToPrivatePackageFactory
    {
        private ApplicationDbContext _dbc;
        public ConnectionUserToPrivatePackageFactory(ApplicationDbContext dbc)
        {
            _dbc = dbc;
        }

        public async Task<ConnectionUserToPrivatePackage> create(ConnectionUserToPrivatePackageNewDTO connectionUserToPrivatePackageNewDTO)
        {
            UserService userService = new UserService(_dbc);
            User user = await userService.findById(connectionUserToPrivatePackageNewDTO.id_of_user);
            if (user == null) return null;

            PackageService packageService = new PackageService(_dbc);
            Package package = await packageService.findById(connectionUserToPrivatePackageNewDTO.id_of_package);
            if (package == null) return null;

            ConnectionUserToPrivatePackageService connectionUserToPrivatePackageService = new ConnectionUserToPrivatePackageService(_dbc);
            return await connectionUserToPrivatePackageService.add(user, package);
        }

    }
}
