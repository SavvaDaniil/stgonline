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

        public JsonAnswerStatus addByRegistration(UserNewDTO userNewDTO)
        {
            UserService userService = new UserService(_dbc);

            if (userService.isAlreadyExistByUsername(userNewDTO.username))
            {
                return new JsonAnswerStatus("error", "username_already_exist");
            }

            User user = userService.add(userNewDTO);
            if (user == null)
            {
                return new JsonAnswerStatus("error", "unknown_error");
            }


            return new JsonAnswerStatus("success", null, user);
        }

        public User registrationAfterPayedStatement(PreUserWithAppointment preUserWithAppointment)
        {
            UserService userService = new UserService(_dbc);
            User user = userService.add(preUserWithAppointment);

            return user;
        }

        public JsonAnswerViewModel login(UserLoginDTO userLoginDTO)
        {
            UserService userService = new UserService(_dbc);
            User user = userService.findByUsername(userLoginDTO.Username);

            if (user != null && BCrypt.Net.BCrypt.Verify(userLoginDTO.Password, user.Password))
            {
                return new JsonAnswerViewModel("success",null, user);
            }

            return new JsonAnswerViewModel("error", "wrong", null);
        }

        public UserProfileViewModel getUserProfile(HttpContext httpContext)
        {
            UserService userService = new UserService(this._dbc);
            User user = userService.findById(int.Parse(httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier).ToString()));
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
                user.phone,
                user.sex,
                user.instagram,
                date_of_birthday_str,
                user.prolongation);
        }

        public UserEditViewModel get(int id)
        {
            UserService userService = new UserService(this._dbc);
            User user = userService.findById(id);
            if (user == null) return null;


            string date_of_birthday_str = null;
            if (user.date_of_birthday != null)
            {
                DateTime date_of_birthday = user.date_of_birthday.Value;
                date_of_birthday_str = date_of_birthday.Date.ToString("yyyy-MM-dd");
            }

            RegionService regionService = new RegionService(_dbc);
            List<Region> regions = regionService.listAll();

            PackageService packageService = new PackageService(_dbc);
            IEnumerable<Package> privatePackages = packageService.listAllActivePrivate();

            ConnectionUserToPrivatePackageService connectionUserToPrivatePackageService = new ConnectionUserToPrivatePackageService(_dbc);
            List<int> listIdConnectedPrivatePackesToUser = connectionUserToPrivatePackageService.listIdAllByUser(user);

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
                user.phone,
                user.instagram,
                date_of_birthday_str,
                user.isTest,
                user.isLessonFullAccess,
                user.isPublicPackageFullAccess,
                user.region,
                regions,
                user.prolongation,
                packagePrivateConnectionToUsers
            );
        }

        public JsonAnswerViewModel save(HttpContext httpContext, UserProfileDTO userProfileDTO)
        {
            UserService userService = new UserService(_dbc);
            int id_of_user = int.Parse(httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier).ToString());
            User user = userService.findById(id_of_user);
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
                if (userService.isAlreadyExistByUsernameExceptId(id_of_user, userProfileDTO.username)) {
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
            return userService.save(user, userProfileDTO);

        }


        public JsonAnswerStatus edit(UserEditDTO userEditDTO)
        {
            UserService userService = new UserService(this._dbc);
            return userService.update(userEditDTO);
        }

        public bool updateUserIdInAmoCRM(User user, int id_in_amocrm)
        {
            UserService userService = new UserService(this._dbc);
            return userService.updateUserIdInAmoCRM(user, id_in_amocrm);
        }

        public User getCurrentOrNull(HttpContext httpContext)
        {
            UserService userService = new UserService(this._dbc);
            if (!httpContext.User.IsInRole("User")) return null;
            if (httpContext.User.FindFirstValue(ClaimTypes.Role).ToString() != "User") return null;

            User user = userService.findById(int.Parse(httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier).ToString()));
            if (user == null) return null;
            return user;
        }


        public JsonAnswerStatus forget(UserForgetDTO userForgetDTO)
        {
            UserService userService = new UserService(_dbc);

            if (userForgetDTO.step == 0) {
                if (userForgetDTO.username == null) return new JsonAnswerStatus("error", "no_username");
                User user = userService.findByUsername(userForgetDTO.username);
                if (user == null) return new JsonAnswerStatus("error", "not_found");
                if (user.forgetDateOfLastTry == null) user.forgetDateOfLastTry = DateTime.Now;
                if (user.forgetDateOfLastTry.Value.AddMinutes(20) > DateTime.Now)
                {
                    if (user.forget_count > 2) return new JsonAnswerStatus("error", "max_limit_try");
                }
                string code = RandomComponent.RandomIntAsString(6);
                userService.updateUserForgetCode(user, code);

                UserForgetObserver userForgetObserver = new UserForgetObserver();
                userForgetObserver.sendCodeToUser(user.Username, code);

                return new JsonAnswerStatus("success", null, user.Id);
            } else if(userForgetDTO.step == 1) { 
                if(userForgetDTO.code == null || userForgetDTO.forget_id == 0) return new JsonAnswerStatus("error", "no_data");
                User user = userService.findById(userForgetDTO.forget_id);
                if (user == null) return new JsonAnswerStatus("error", "not_found");

                if(user.forget_code != userForgetDTO.code)
                {
                    if(user.forget_count > 2)
                    {
                        return new JsonAnswerStatus("error", "wrong_max_limit");
                    } else if(user.forget_count > 1)
                    {
                        userService.forgetUpdateCount(user, 3);
                        return new JsonAnswerStatus("error", "wrong_2");
                    } else if(user.forget_count > 0)
                    {
                        userService.forgetUpdateCount(user, 2);
                        return new JsonAnswerStatus("error", "wrong_1");
                    } else
                    {
                        userService.forgetUpdateCount(user, 1);
                        return new JsonAnswerStatus("error", "wrong");
                    }
                }
                string newPassword = userService.setRandomPassword(user);
                //отправляем письмо с новым паролем
                UserForgetObserver userForgetObserver = new UserForgetObserver();
                userForgetObserver.sendNewPasswodToUser(user.Username, newPassword);

                return new JsonAnswerStatus("success", null);
            }

            return new JsonAnswerStatus("error", null);
        }


        public JsonAnswerStatus search(UserSearchDTO userSearchDTO)
        {
            UserService userService = new UserService(_dbc);
            List<User> usersSearchResult = userService.searchUsers(userSearchDTO);
            int searchCount = userService.searchCount(userSearchDTO);

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



        public JsonAnswerStatus checkCountActivePackages(HttpContext httpContext)
        {
            User user = getCurrentOrNull(httpContext);
            if (user == null) return new JsonAnswerStatus("error", "no_user");

            PurchasePackageService purchasePackageService = new PurchasePackageService(_dbc);
            int countAllActivePackages = purchasePackageService.countAllActive(user);

            return (
                countAllActivePackages < 4
                ? new JsonAnswerStatus("success", null)
                : new JsonAnswerStatus("error","limit")
            );
        }


        public List<UserAmoCRMData> checkUsersForIdOfAmocrm()
        {
            UserService userService = new UserService(_dbc);
            List<User> usersWithoutIdOfAmocrm = userService.listAllWithoutIdOfAmocrm();
            List<UserAmoCRMData> userAmoCRMDatas = new List<UserAmoCRMData>();
            AmoCRMFacade amoCRMFacade = new AmoCRMFacade(_dbc);


            foreach (User user in usersWithoutIdOfAmocrm)
            {
                userAmoCRMDatas.Add(
                    new UserAmoCRMData(
                        user.Id,
                        user.Username,
                        user.id_in_amocrm,
                        user.dateOfAdd
                    )
                );

            }

            return userAmoCRMDatas;
        }

    }
}
