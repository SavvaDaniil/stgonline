using Microsoft.EntityFrameworkCore;
using STG.Component;
using STG.Data;
using STG.DTO;
using STG.DTO.UserDTO;
using STG.Entities;
using STG.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace STG.Service
{
    public class UserService
    {

        public ApplicationDbContext _dbc;
        public UserService(ApplicationDbContext dbc)
        {
            this._dbc = dbc;
        }

        public bool isAlreadyExistByUsername(string username)
        {
            if (_dbc.Users.Any(p => p.Username == username))
            {
                return true;
            }
            return false;
        }

        public bool isAlreadyExistByUsernameExceptId(int id, string username)
        {
            if (_dbc.Users
                .Where(p => p.Username == username && p.Id != id)
                .Any())
            {
                return true;
            }
            return false;
        }

        public User findByUsername(string username)
        {
            return _dbc.Users.FirstOrDefault(p=> p.Username == username);
        }

        public User findById(int id_of_user)
        {
            return _dbc.Users
                .Include(p => p.region)
                .FirstOrDefault(p => p.Id == id_of_user);
        }

        public User add(UserNewDTO userNewDTO)
        {
            User user = new User();
            user.Username = userNewDTO.username;
            user.firstname = userNewDTO.firstname;
            user.secondname = userNewDTO.secondname;
            user.phone = userNewDTO.phone;
            user.instagram = userNewDTO.instagram;
            user.date_of_birthday = userNewDTO.date_of_birthday;

            user.Password = BCrypt.Net.BCrypt.HashPassword(userNewDTO.password);
            user.dateOfAdd = DateTime.Now;
            user.AuthKey = RandomComponent.RandomString(32);
            user.active = 1;

            _dbc.Users.Add(user);

            _dbc.SaveChanges();

            return user;
        }

        public User add(PreUserWithAppointment preUserWithAppointment)
        {
            User user = new User();
            user.Username = preUserWithAppointment.username;
            user.firstname = preUserWithAppointment.firstname;
            user.secondname = preUserWithAppointment.secondname;
            user.phone = preUserWithAppointment.phone;
            user.instagram = preUserWithAppointment.instagram;
            user.date_of_birthday = preUserWithAppointment.date_of_birthday;
            user.region = preUserWithAppointment.region;

            user.Password = preUserWithAppointment.password;
            user.dateOfAdd = DateTime.Now;
            user.AuthKey = RandomComponent.RandomString(32);
            user.active = 1;


            _dbc.Users.Add(user);

            _dbc.SaveChanges();

            return user;
        }

        public JsonAnswerViewModel save(User user, UserProfileDTO userProfileDTO)
        {
            user.firstname = userProfileDTO.firstname;
            user.secondname = userProfileDTO.secondname;
            user.phone = userProfileDTO.phone;
            user.instagram = userProfileDTO.instagram;
            user.prolongation = userProfileDTO.prolongation;
            user.date_of_birthday = userProfileDTO.date_of_birthday;

            bool isNeedReSign = false;
            if (userProfileDTO.newPassword != null)
            {
                isNeedReSign = true;
                user.Password = BCrypt.Net.BCrypt.HashPassword(userProfileDTO.newPassword);
            }
            if(userProfileDTO.username != user.Username)
            {
                isNeedReSign = true;
                user.Username = userProfileDTO.username;
            }

            _dbc.SaveChanges();

            return (isNeedReSign) ? new JsonAnswerViewModel("success",null, user) : new JsonAnswerViewModel("success", null);
        }

        public JsonAnswerStatus update(UserEditDTO userEditDTO)
        {
            User user = findById(userEditDTO.id);
            if (user == null) return null;

            user.firstname = userEditDTO.firstname;
            user.secondname = userEditDTO.secondname;
            user.phone = userEditDTO.phone;
            user.instagram = userEditDTO.instagram;
            user.prolongation = userEditDTO.prolongation;
            user.date_of_birthday = userEditDTO.date_of_birthday;
            user.isTest = userEditDTO.is_test;
            user.isLessonFullAccess = userEditDTO.is_lesson_full_access;
            user.isPublicPackageFullAccess = userEditDTO.is_public_package_full_access;
            user.active = userEditDTO.active;

            if (userEditDTO.new_password != null)
            {
                user.Password = BCrypt.Net.BCrypt.HashPassword(userEditDTO.new_password);
                user.AuthKey = RandomComponent.RandomString(32);
            }

            if (userEditDTO.username != user.Username)
            {
                if(isAlreadyExistByUsernameExceptId(user.Id, userEditDTO.username))
                {
                    return new JsonAnswerStatus("error", "username_already_exist");
                } 
                user.Username = userEditDTO.username;
            }

            _dbc.SaveChanges();

            return new JsonAnswerStatus("success", null);
        }

        public bool setProngationTrue(User user)
        {
            user.prolongation = 1;
            _dbc.SaveChanges();
            return true;
        }


        public bool updateUserForgetCode(User user, string code)
        {
            user.forget_code = code;
            if(user.forgetDateOfLastTry.Value.AddMinutes(20) < DateTime.Now)
            {
                user.forget_count = 1;
            } else
            {
                user.forget_count++;
            }

            _dbc.SaveChanges();
            return true;
        }

        public bool forgetUpdateCount(User user, int count)
        {
            user.forget_count = count;
            _dbc.SaveChanges();
            return true;
        }

        public bool forgetCheckCode(int id, string code)
        {
            User user = findById(id);
            if (user == null) return false;
            if (user.forget_code != code) return false;
            return true;
        }

        public string setRandomPassword(User user)
        {
            string randomPassword = RandomComponent.RandomString(6);
            user.Password = BCrypt.Net.BCrypt.HashPassword(randomPassword);
            _dbc.SaveChanges();
            return randomPassword;
        }


        public bool updateUserIdInAmoCRM(User user, int id_in_amocrm)
        {
            user.id_in_amocrm = id_in_amocrm;
            _dbc.SaveChanges();
            return true;
        }


        public List<User> searchUsers(UserSearchDTO userSearchDTO)
        {
            userSearchDTO.page--;
            int take = 30;
            int skip = userSearchDTO.page * take;
            if (!string.IsNullOrEmpty(userSearchDTO.queryString))
            {
                //, take.ToString(), skip.ToString()
                return _dbc.Users.FromSql("SELECT * FROM user WHERE Username LIKE {0} OR firstname LIKE {0} OR secondname LIKE {0} OR instagram LIKE {0} ORDER BY Id DESC LIMIT {1}, {2}", "%"+userSearchDTO.queryString+"%", skip, take)
                    .ToList();
            } else
            {
                IQueryable<User> q = _dbc.Users.OrderByDescending(p => p.Id);


                q = q.Skip(skip).Take(take);
                /*
                q = q.Where(p =>
                    EF.Functions.Like(p.Username, userSearchDTO.queryString)
                    || EF.Functions.Like(p.secondname, userSearchDTO.queryString)
                    || EF.Functions.Like(p.firstname, userSearchDTO.queryString)
                    || EF.Functions.Like(p.instagram, userSearchDTO.queryString)
                    || p.Username.Contains(userSearchDTO.queryString)
                );
                */
                return q.ToList();
            }
        }

        public int searchCount(UserSearchDTO userSearchDTO)
        {
            if (!string.IsNullOrEmpty(userSearchDTO.queryString))
            {
                //, take.ToString(), skip.ToString()
                return _dbc.Users.FromSql("SELECT * FROM user WHERE Username LIKE {0}", "%" + userSearchDTO.queryString + "%")
                    .Count();
            }
            else
            {
                IQueryable<User> q = _dbc.Users.OrderByDescending(p => p.Id);
                return q.Count();
            }
        }


        public List<User> listAllWithoutIdOfAmocrm()
        {
            return _dbc.Users
                .Where(p => p.id_in_amocrm == 0)
                .OrderByDescending(p => p.Id)
                .ToList();
        }

    }

}
