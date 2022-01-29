using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using STG.Data;
using STG.DTO;
using STG.Interface.Factory;
using STG.Entities;
using STG.Service;
using STG.ViewModels;
using STG.DTO.UserDTO;

namespace STG.Factory
{
    public class UserFactory
    {
        private ApplicationDbContext _dbc;
        public UserFactory(ApplicationDbContext dbc)
        {
            this._dbc = dbc;
        }

        public User createByAdmin(UserNewDTO userNewDTO)
        {
            throw new NotImplementedException();
        }

        public async Task<JsonAnswerStatus> createByRegistration(UserNewDTO userNewDTO)
        {
            UserService userService = new UserService(_dbc);

            if (userService.isAlreadyExistByUsername(userNewDTO.username))
            {
                return new JsonAnswerStatus("error","username_already_exist");
            }

            User user = await userService.add(userNewDTO);
            if (user == null)
            {
                return new JsonAnswerStatus("error", "unknown_error");
            }

            return new JsonAnswerStatus("success", null, user);
        }
        
    }
}
