using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using STG.Data;
using STG.Facade;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace STG.Controllers.api
{
    [Route("api/amocrm")]
    [ApiController]
    public class ApiAmoCRMController : ControllerBase
    {
        private ApplicationDbContext _dbc;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public ApiAmoCRMController(IHttpContextAccessor httpContextAccessor, ApplicationDbContext dbc)
        {
            _httpContextAccessor = httpContextAccessor;
            this._dbc = dbc;
        }

        [Route("test")]
        public IActionResult test()
        {
            AmoCRMFacade amoCRMFacade = new AmoCRMFacade(_dbc);

            //System.Diagnostics.Debug.WriteLine("_httpContextAccessor.HttpContext.Request.Host.Value: " + _httpContextAccessor.HttpContext.Request.Host.Value);
            //await amoCRMFacade.test()
            return Ok() ;
        }
    }
}
