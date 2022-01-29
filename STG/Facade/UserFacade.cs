using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using STG.Data;
using STG.DTO;
using STG.Interface.Facade;
using STG.Entities;
using STG.Service;
using STG.ViewModels;
using STG.DTO.UserDTO;
using STG.Component;
using STG.Observer;
using STG.ViewModels.User;
using STG.ViewModels.Package;

namespace STG.Facade
{
    public class UserFacade
    {
        private ApplicationDbContext _dbc;
        public UserFacade(ApplicationDbContext dbc)
        {
            this._dbc = dbc;
        }

        public User add(UserNewDTO userNewDTO)
        {
            throw new NotImplementedException();
        }

        public async Task<User> registrationAfterPayedStatement(PreUserWithAppointment preUserWithAppointment)
        {
            UserService userService = new UserService(_dbc);
            return await userService.add(preUserWithAppointment);
        }

        public async Task<JsonAnswerViewModel> login(UserLoginDTO userLoginDTO)
        {
            UserService userService = new UserService(_dbc);
            User user = await userService.findByUsername(userLoginDTO.Username);

            if (user != null && BCrypt.Net.BCrypt.Verify(userLoginDTO.Password, user.Password))
            {
                return new JsonAnswerViewModel("success",null, user);
            }

            return new JsonAnswerViewModel("error", "wrong", null);
        }

        public async Task<UserProfileViewModel> getUserProfile(HttpContext httpContext)
        {
            UserService userService = new UserService(this._dbc);
            User user = await userService.findById(int.Parse(httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier).ToString()));
            if (user == null) return null;
            userService = null;
            GC.Collect();

            string date_of_birthday_str = null;
            if(user.date_of_birthday != null)
            {
                DateTime date_of_birthday = user.date_of_birthday.Value;
                date_of_birthday_str = date_of_birthday.Date.ToString("yyyy-MM-dd");
            }


            return new UserProfileViewModel(
                user.firstname,
                user.secondname,
                user.Username,
                user.sex,
                user.instagram,
                date_of_birthday_str,
                user.prolongation);
        }

        public async Task<UserEditViewModel> get(int id)
        {
            UserService userService = new UserService(this._dbc);
            User user = await userService.findById(id);
            if (user == null) return null;


            string date_of_birthday_str = null;
            if (user.date_of_birthday != null)
            {
                DateTime date_of_birthday = user.date_of_birthday.Value;
                date_of_birthday_str = date_of_birthday.Date.ToString("yyyy-MM-dd");
            }

            RegionService regionService = new RegionService(_dbc);
            List<Region> regions = await regionService.listAll();

            PackageService packageService = new PackageService(_dbc);
            IEnumerable<Package> privatePackages = await packageService.listAll();

            ConnectionUserToPrivatePackageService connectionUserToPrivatePackageService = new ConnectionUserToPrivatePackageService(_dbc);
            List<int> listIdConnectedPrivatePackesToUser = await connectionUserToPrivatePackageService.listIdAllByUser(user);

            List<PackagePrivateConnectionToUser> packagePrivateConnectionToUsers = new List<PackagePrivateConnectionToUser>();

            foreach (Package package in privatePackages)
            {
                packagePrivateConnectionToUsers.Add(
                    new PackagePrivateConnectionToUser(
                        package.id,
                        package.name,
                        (listIdConnectedPrivatePackesToUser.Contains(package.id) ? 1 : 0)
                    )
                );
            }
            //List<>

            return new UserEditViewModel(
                user.Id,
                user.Username,
                user.active,
                user.firstname,
                user.secondname,
                user.instagram,
                date_of_birthday_str,
                user.isTest,
                user.isLessonFullAccess,
                user.region,
                regions,
                user.prolongation,
                packagePrivateConnectionToUsers
            );
        }

        public async Task<JsonAnswerViewModel> save(HttpContext httpContext, UserProfileDTO userProfileDTO)
        {
            UserService userService = new UserService(_dbc);
            int id_of_user = int.Parse(httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier).ToString());
            User user = await userService.findById(id_of_user);
            if (user == null) return null;

            if (user.Username != userProfileDTO.username)
            {
                if (userProfileDTO.currentPassword == null)
                {
                    return new JsonAnswerViewModel("error", "need_current_password");
                }
                if (!BCrypt.Net.BCrypt.Verify(userProfileDTO.currentPassword, user.Password))
                {
                    return new JsonAnswerViewModel("error", "wrong");
                }
                if (await userService.isAlreadyExistByUsernameExceptId(id_of_user, userProfileDTO.username)) {
                    return new JsonAnswerViewModel("error", "username_already_exist");
                }
            }

            if (userProfileDTO.newPassword != null)
            {
                if (!BCrypt.Net.BCrypt.Verify(userProfileDTO.currentPassword, user.Password))
                {
                    return new JsonAnswerViewModel("error", "wrong");
                }
            }
            return await userService.save(user, userProfileDTO);

            /*
                {
                    user = await userService.save(user, userProfileDTO);
                    await userService.save(user, userProfileDTO);
                    userService = null;
                    GC.Collect();
                    return (user != null) ? new JsonAnswer("success", null, user) : new JsonAnswer("error", null);
                }
            } else
            {
                user = await userService.save(user, userProfileDTO);
                await userService.save(user, userProfileDTO);
                userService = null;
                GC.Collect();
                return (user != null) ? new JsonAnswer("success", null) : new JsonAnswer("error", null);
            }
            */
        }


        public async Task<JsonAnswerStatus> edit(UserEditDTO userEditDTO)
        {
            UserService userService = new UserService(this._dbc);
            return await userService.update(userEditDTO);
        }

        public async Task<bool> updateUserIdInAmoCRM(User user, int id_in_amocrm)
        {
            UserService userService = new UserService(this._dbc);
            return await userService.updateUserIdInAmoCRM(user, id_in_amocrm);
        }

        public async Task<User> getCurrentOrNull(HttpContext httpContext)
        {
            UserService userService = new UserService(this._dbc);
            if (!httpContext.User.IsInRole("User")) return null;
            if (httpContext.User.FindFirstValue(ClaimTypes.Role).ToString() != "User") return null;

            User user = await userService.findById(int.Parse(httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier).ToString()));
            if (user == null) return null;
            return user;
        }


        public async Task<JsonAnswerStatus> forget(UserForgetDTO userForgetDTO)
        {
            UserService userService = new UserService(_dbc);

            if (userForgetDTO.step == 0) {
                if (userForgetDTO.username == null) return new JsonAnswerStatus("error", "no_username");
                User user = await userService.findByUsername(userForgetDTO.username);
                if (user == null) return new JsonAnswerStatus("error", "not_found");
                if (user.forgetDateOfLastTry == null) user.forgetDateOfLastTry = DateTime.Now;
                if (user.forgetDateOfLastTry.Value.AddMinutes(20) > DateTime.Now)
                {
                    if (user.forget_count > 2) return new JsonAnswerStatus("error", "max_limit_try");
                }
                string code = RandomComponent.RandomIntAsString(6);
                await userService.updateUserForgetCode(user, code);

                UserForgetObserver userForgetObserver = new UserForgetObserver();
                userForgetObserver.sendCodeToUser(user.Username, code);

                return new JsonAnswerStatus("success", null, user.Id);
            } else if(userForgetDTO.step == 1) { 
                if(userForgetDTO.code == null || userForgetDTO.forget_id == 0) return new JsonAnswerStatus("error", "no_data");
                User user = await userService.findById(userForgetDTO.forget_id);
                if (user == null) return new JsonAnswerStatus("error", "not_found");

                if(user.forget_code != userForgetDTO.code)
                {
                    if(user.forget_count > 2)
                    {
                        return new JsonAnswerStatus("error", "wrong_max_limit");
                    } else if(user.forget_count > 1)
                    {
                        await userService.forgetUpdateCount(user, 3);
                        return new JsonAnswerStatus("error", "wrong_2");
                    } else if(user.forget_count > 0)
                    {
                        await userService.forgetUpdateCount(user, 2);
                        return new JsonAnswerStatus("error", "wrong_1");
                    } else
                    {
                        await userService.forgetUpdateCount(user, 1);
                        return new JsonAnswerStatus("error", "wrong");
                    }
                }
                string newPassword = await userService.setRandomPassword(user);
                //отправляем письмо с новым паролем
                UserForgetObserver userForgetObserver = new UserForgetObserver();
                userForgetObserver.sendNewPasswodToUser(user.Username, newPassword);

                return new JsonAnswerStatus("success", null);
            }

            return new JsonAnswerStatus("error", null);
        }


        public async Task<JsonAnswerStatus> search(UserSearchDTO userSearchDTO)
        {
            UserService userService = new UserService(_dbc);
            List<User> usersSearchResult = await userService.searchUsers(userSearchDTO);
            int searchCount = await userService.searchCount(userSearchDTO);

            List<UserSearchPreviewViewModel> userSearchPreviewViewModels = new List<UserSearchPreviewViewModel>();
            foreach (User user in usersSearchResult)
            {
                userSearchPreviewViewModels.Add(new UserSearchPreviewViewModel(
                    user.Id,
                    user.Username,
                    user.firstname,
                    user.secondname,
                    user.instagram,
                    (user.region != null ? user.region.name : null)
                    )
                );
            }

            return new JsonAnswerStatus("success", null, new UserSearchViewModel(userSearchPreviewViewModels, searchCount));
        }



        public async Task<JsonAnswerStatus> checkCountActivePackages(HttpContext httpContext)
        {
            User user = await getCurrentOrNull(httpContext);
            if (user == null) return new JsonAnswerStatus("error", "no_user");

            PurchasePackageService purchasePackageService = new PurchasePackageService(_dbc);
            int countAllActivePackages = await purchasePackageService.countAllActive(user);

            return (
                countAllActivePackages < 4
                ? new JsonAnswerStatus("success", null)
                : new JsonAnswerStatus("error","limit")
            );
        }


    }
}
