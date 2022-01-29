using STG.Data;
using STG.DTO.ConnectionUserToPrivatePackage;
using STG.DTO.Package;
using STG.Factory;
using STG.Entities;
using STG.Service;
using STG.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace STG.Facade
{
    public class ConnectionUserToPrivatePackageFacade
    {
        private ApplicationDbContext _dbc;
        public ConnectionUserToPrivatePackageFacade(ApplicationDbContext dbc)
        {
            this._dbc = dbc;
        }

        public async Task<ConnectionUserToPrivatePackage> add(ConnectionUserToPrivatePackageNewDTO connectionUserToPrivatePackageNewDTO)
        {
            ConnectionUserToPrivatePackageFactory connectionUserToPrivatePackageFactory = new ConnectionUserToPrivatePackageFactory(_dbc);
            return await connectionUserToPrivatePackageFactory.create(connectionUserToPrivatePackageNewDTO);
        }

        public async Task<bool> delete(int id)
        {
            ConnectionUserToPrivatePackageService connectionUserToPrivatePackageService = new ConnectionUserToPrivatePackageService(_dbc);
            return await connectionUserToPrivatePackageService.delete(id);
        }

        public async Task<bool> isAvailable(User user, Package package)
        {
            ConnectionUserToPrivatePackageService connectionUserToPrivatePackageService = new ConnectionUserToPrivatePackageService(_dbc);
            return await connectionUserToPrivatePackageService.isAny(user, package);
        }

        public async Task<List<ConnectionUserToPrivatePackage>> listAllByUser(User user)
        {
            ConnectionUserToPrivatePackageService connectionUserToPrivatePackageService = new ConnectionUserToPrivatePackageService(_dbc);
            return await connectionUserToPrivatePackageService.listAllByUser(user);
        }

        public async Task<List<int>> listAllIdOfPrivatePackagesConnectedToUser(User user)
        {
            ConnectionUserToPrivatePackageService connectionUserToPrivatePackageService = new ConnectionUserToPrivatePackageService(_dbc);
            List<ConnectionUserToPrivatePackage> listAllByUser = await connectionUserToPrivatePackageService.listAllByUser(user);

            List<int> listAllIdOfPrivatePackagesConnectedToUser = new List<int>();
            foreach (ConnectionUserToPrivatePackage connectionUserToPrivatePackage in listAllByUser)
            {
                listAllIdOfPrivatePackagesConnectedToUser.Add(connectionUserToPrivatePackage.package.id);
            }

            return listAllIdOfPrivatePackagesConnectedToUser;
        }

        public async Task<JsonAnswerStatus> update(ConnectionUserToPrivatePackageEditDTO connectionUserToPrivatePackageEditDTO)
        {
            UserService userService = new UserService(_dbc);
            User user = await userService.findById(connectionUserToPrivatePackageEditDTO.id_of_user);
            if (user == null) return new JsonAnswerStatus("error", "user_not_found");

            PackageService packageService = new PackageService(_dbc);
            Package package = await packageService.findById(connectionUserToPrivatePackageEditDTO.id_of_package);
            if (package == null) return new JsonAnswerStatus("error", "package_not_found");

            ConnectionUserToPrivatePackageService connectionUserToPrivatePackageService = new ConnectionUserToPrivatePackageService(_dbc);
            if(connectionUserToPrivatePackageEditDTO.active == 1)
            {
                if(await connectionUserToPrivatePackageService.add(user, package) != null)return new JsonAnswerStatus("success", null);
                
            } else if (connectionUserToPrivatePackageEditDTO.active == 0)
            {
                if(await connectionUserToPrivatePackageService.delete(user, package))return new JsonAnswerStatus("success", null);
            }

            return new JsonAnswerStatus("error", "unknown");
        }




    }
}
