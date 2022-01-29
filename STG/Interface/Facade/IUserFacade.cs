using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using STG.Entities;
using STG.DTO.UserDTO;

namespace STG.Interface.Facade
{
    interface IUserFacade
    {
        User add(UserNewDTO userNewDTO);
        User save(User user);
    }
}
