using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using STG.Data;
using STG.DTO.Admin;
using STG.Facade;
using STG.Entities;
using STG.ViewModels;
using STG.ViewModels.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace STG.Controllers.api
{
    [Route("api/admin")]
    [Authorize(AuthenticationSchemes = "AdminCookie")]
    [ApiController]
    public class ApiAdminController : ControllerBase
    {
        private ApplicationDbContext _dbc;
        public ApiAdminController(ApplicationDbContext dbc)
        {
            this._dbc = dbc;
        }

        [Route("update")]
        public IActionResult update([FromForm] AdminDTO adminDTO)
        {
            if (ModelState.IsValid)
            {
                AdminFacade adminFacade = new AdminFacade(_dbc);
                JsonAnswerViewModel jsonAnswerViewModel = adminFacade.update(HttpContext, adminDTO);

                if (jsonAnswerViewModel.isNeedRelogin && jsonAnswerViewModel.admin != null)
                {
                    HttpContext.SignOutAsync("AdminCookie");
                    this.signIn(jsonAnswerViewModel.admin);
                }

                return Ok(new JsonAnswerViewModel(jsonAnswerViewModel.status, jsonAnswerViewModel.errors));
            }
            return Ok(new JsonAnswerViewModel("error", null));
        }

        [Route("add")]
        public IActionResult add([FromForm] AdminNewDTO adminNewDTO)
        {
            if (ModelState.IsValid)
            {
                AdminFacade adminFacade = new AdminFacade(_dbc);
                return Ok(adminFacade.add(adminNewDTO));
            }
            return Ok(new JsonAnswerViewModel("error", null));
        }

        [Route("delete")]
        public IActionResult delete([FromForm] AdminSearchIdDTO adminSearchIdDTO)
        {
            if (ModelState.IsValid)
            {
                AdminFacade adminFacade = new AdminFacade(_dbc);
                return Ok(adminFacade.delete(adminSearchIdDTO));
            }
            return Ok(new JsonAnswerViewModel("error", null));
        }



        [Route("login")]
        [AllowAnonymous]
        public IActionResult login([FromForm] AdminLoginDTO adminLoginDTO)
        {
            if (ModelState.IsValid)
            {
                AdminFacade adminFacade = new AdminFacade(_dbc);
                Admin admin = adminFacade.login(adminLoginDTO);
                if(admin != null)
                {
                    this.signIn(admin);
                    return Ok(new JsonAnswerViewModel("success",null));
                }
            }
            return Ok(new JsonAnswerViewModel("error", "wrong"));

        }

        private void signIn(Admin admin)
        {
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, admin.Id.ToString()),
                new Claim(ClaimTypes.Name, admin.Username),
                new Claim(ClaimTypes.Role, "Admin"),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, "Admin")
            };
            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "AdminCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
            ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

            HttpContext.SignInAsync("AdminCookie", claimsPrincipal);
        }


        [HttpGet]
        [Route("logout")]
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync("AdminCookie");
            return Ok(new JsonAnswerViewModel("success", null));
        }


        [HttpPost]
        [Route("search")]
        public IActionResult Search([FromForm] AdminSearchDTO adminSearchDTO)
        {
            if (ModelState.IsValid)
            {
                AdminFacade adminFacade = new AdminFacade(_dbc);
                return Ok(adminFacade.search(adminSearchDTO));
            }
            return Ok();
        }




        [HttpPost]
        [Route("get")]
        public IActionResult get([FromForm] AdminSearchIdDTO adminSearchIdDTO)
        {
            if (ModelState.IsValid)
            {
                AdminFacade adminFacade = new AdminFacade(_dbc);
                AdminEditViewModel adminEditViewModel = adminFacade.get(adminSearchIdDTO.id);
                return Ok(
                    adminEditViewModel != null
                    ? new JsonAnswerStatus("success", null, adminEditViewModel)
                    : new JsonAnswerStatus("error", "not_found")
                );
            }
            return Ok();
        }

        [Route("edit")]
        public IActionResult Edit([FromForm] AdminEditDTO adminEditDTO)
        {
            if (ModelState.IsValid)
            {
                AdminFacade adminFacade = new AdminFacade(_dbc);
                return Ok(adminFacade.edit(adminEditDTO));
            }
            return Ok(new JsonAnswerStatus("error", null));
        }

    }
}
