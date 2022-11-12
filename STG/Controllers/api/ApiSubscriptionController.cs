using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using STG.Data;
using STG.Facade;
using STG.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace STG.Controllers.api
{
    [Route("api/subscription")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "AdminCookie")]
    public class ApiSubscriptionController : ControllerBase
    {
        ApplicationDbContext _dbc;
        public ApiSubscriptionController(ApplicationDbContext dbc)
        {
            this._dbc = dbc;
        }

        [HttpPost]
        [Route("list_all")]
        public IActionResult listAll()
        {
            SubscriptionFacade subscriptionFacade = new SubscriptionFacade(_dbc);
            return Ok(subscriptionFacade.listAll());
        }
    }
}
