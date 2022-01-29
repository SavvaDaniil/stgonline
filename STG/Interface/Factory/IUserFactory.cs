using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using STG.DTO.UserDTO;
using STG.Entities;
using STG.ViewModels;

namespace STG.Interface.Factory
{
    interface IUserFactory
    {
        Task<JsonAnswerViewModel> createByRegistration(UserNewDTO userNewDTO);
        User createByAdmin(UserNewDTO userNewDTO);
    }
}
