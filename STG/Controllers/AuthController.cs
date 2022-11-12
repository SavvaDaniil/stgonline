using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using STG.Data;
using STG.DTO;
using STG.DTO.UserDTO;
using STG.Facade;
using STG.Factory;
using STG.Entities;
using STG.ViewModels;

namespace STG.Controllers
{
    //[Route("api/[controller]")]
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private ApplicationDbContext _dbc;
        public AuthController(ApplicationDbContext dbc)
        {
            this._dbc = dbc;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Login")]
        public IActionResult Login([FromForm]UserLoginDTO userLoginDTO)
        {
            if (ModelState.IsValid)
            {
                UserFacade userFacade = new UserFacade(_dbc);

                JsonAnswerViewModel jsonAnswer = userFacade.login(userLoginDTO);

                if (jsonAnswer.user != null)
                {
                    signIn(jsonAnswer.user);
                    jsonAnswer.user = null;
                }
                userFacade = null;

                return Ok(jsonAnswer);
            }

            return Ok();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Registration")]
        public IActionResult Registration([FromForm]UserNewDTO userNewDTO)
        {
            if (ModelState.IsValid)
            {
                //UserFactory userFactory = new UserFactory(_dbc);
                //JsonAnswerStatus jsonAnswer = userFactory.createByRegistration(userNewDTO);
                UserFacade userFacade = new UserFacade(_dbc);
                JsonAnswerStatus jsonAnswer = userFacade.addByRegistration(userNewDTO);

                if (jsonAnswer.user != null)
                {
                    signIn(jsonAnswer.user);
                    jsonAnswer.user = null;
                }

                return Ok(jsonAnswer);
            }

            return Ok("Success request");
        }

        private void signIn(User user)
        {
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, "User")
            };
            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "UserCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
            ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

            HttpContext.SignInAsync("UserCookie", claimsPrincipal);
        }

        [HttpGet]
        [Route("Logout")]
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync();
            return Ok();
        }
    }
}