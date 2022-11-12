using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using STG.Data;
using STG.DTO.ConnectionUserToPrivatePackage;
using STG.Facade;
using STG.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace STG.Controllers.api
{
    [Route("api/connection_user_to_private_package")]
    [ApiController]
    public class ApiConnectionUserToPrivatePackageController : ControllerBase
    {
        private ApplicationDbContext _dbc;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public ApiConnectionUserToPrivatePackageController(IHttpContextAccessor httpContextAccessor, ApplicationDbContext dbc)
        {
            this._dbc = dbc;
            _httpContextAccessor = httpContextAccessor;
        }



        [Route("update")]
        public IActionResult update([FromForm] ConnectionUserToPrivatePackageEditDTO connectionUserToPrivatePackageEditDTO)
        {
            if (ModelState.IsValid)
            {
                ConnectionUserToPrivatePackageFacade connectionUserToPrivatePackageFacade = new ConnectionUserToPrivatePackageFacade(_dbc);
                return Ok(connectionUserToPrivatePackageFacade.update(connectionUserToPrivatePackageEditDTO));
            }
            return Ok(new JsonAnswerStatus("error", null));
        }
    }
}
