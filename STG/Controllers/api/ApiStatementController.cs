using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using STG.Data;
using STG.DTO.Statement;
using STG.Facade;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace STG.Controllers.api
{
    [Route("api/statement")]
    [ApiController]
    public class ApiStatementController : ControllerBase
    {
        private ApplicationDbContext _dbc;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public ApiStatementController(IHttpContextAccessor httpContextAccessor, ApplicationDbContext dbc)
        {
            _dbc = dbc;
            _httpContextAccessor = httpContextAccessor;
        }


        [HttpPost]
        [Route("add_by_user")]
        [Authorize(AuthenticationSchemes = "UserCookie")]
        public async Task<IActionResult> addByUser([FromForm] StatementNewDTO statementNewDTO)
        {
            if (ModelState.IsValid)
            {
                StatementFacade statementFacade = new StatementFacade(_dbc);
                return Ok(await statementFacade.addByUser(HttpContext, _httpContextAccessor.HttpContext.Request.Host.Value, statementNewDTO));
            }
            return Ok();
        }


        [HttpPost]
        [Route("search")]
        [Authorize(AuthenticationSchemes = "AdminCookie")]
        public IActionResult search([FromForm] StatementSearchDTO statementSearchDTO)
        {
            if (ModelState.IsValid)
            {
                StatementFacade statementFacade = new StatementFacade(_dbc);
                return Ok(statementFacade.search(statementSearchDTO));
            }
            return Ok();
        }


        [HttpPost]
        [Route("get")]
        [Authorize(AuthenticationSchemes = "AdminCookie")]
        public IActionResult get([FromForm] StatementIdDTO statementIdDTO)
        {
            if (ModelState.IsValid)
            {
                StatementFacade statementFacade = new StatementFacade(_dbc);
                return Ok(statementFacade.get(HttpContext, statementIdDTO.id));
            }
            return Ok();
        }


        [HttpPost]
        [Route("edit")]
        [Authorize(AuthenticationSchemes = "AdminCookie")]
        public IActionResult edit([FromForm] StatementEditDTO statementEditDTO)
        {
            if (ModelState.IsValid)
            {
                StatementFacade statementFacade = new StatementFacade(_dbc);
                return Ok(statementFacade.edit(HttpContext, statementEditDTO));
            }
            return Ok();
        }



    }

}
