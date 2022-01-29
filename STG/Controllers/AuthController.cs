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
        public async Task<IActionResult> Login([FromForm]UserLoginDTO userLoginDTO)
        {
            if (ModelState.IsValid)
            {
                UserFacade userFacade = new UserFacade(_dbc);

                JsonAnswerViewModel jsonAnswer = await userFacade.login(userLoginDTO);

                if (jsonAnswer.user != null)
                {
                    await signIn(jsonAnswer.user);
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
        public async Task<IActionResult> Registration([FromForm]UserNewDTO userNewDTO)
        {
            if (ModelState.IsValid)
            {
                UserFactory userFactory = new UserFactory(_dbc);
                JsonAnswerStatus jsonAnswer = await userFactory.createByRegistration(userNewDTO);

                if (jsonAnswer.user != null)
                {
                    await signIn(jsonAnswer.user);
                    jsonAnswer.user = null;
                }
                userFactory = null;

                return Ok(jsonAnswer);
            }

            return Ok("Success request");
        }

        private async Task signIn(User user)
        {
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, "User")
            };
            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "UserCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
            ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

            await HttpContext.SignInAsync("UserCookie", claimsPrincipal);
        }

        [HttpGet]
        [Route("Logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return Ok();
        }
    }
}