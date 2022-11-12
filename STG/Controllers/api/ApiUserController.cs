using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using STG.Data;
using STG.DTO;
using STG.DTO.UserDTO;
using STG.Facade;
using STG.Factory;
using STG.Entities;
using STG.ViewModels;
using System.IdentityModel.Tokens.Jwt;
using STG.Component;
using Microsoft.IdentityModel.Tokens;

namespace STG.Controllers.api
{
    [Route("api/user")]
    [ApiController]
    //[Authorize(AuthenticationSchemes = "UserCookie")]
    public class ApiUserController : ControllerBase
    {
        private ApplicationDbContext _dbc;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public ApiUserController(IHttpContextAccessor httpContextAccessor, ApplicationDbContext dbc)
        {
            this._dbc = dbc;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("login")]
        public async Task<IActionResult> Login([FromForm] UserLoginDTO userLoginDTO)
        {
            if (ModelState.IsValid)
            {
                UserFacade userFacade = new UserFacade(_dbc);

                JsonAnswerViewModel jsonAnswer = userFacade.login(userLoginDTO);

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
        [Route("registration")]
        public async Task<IActionResult> Registration([FromForm] UserNewDTO userNewDTO)
        {
            if (ModelState.IsValid)
            {
                //return Ok("Success request");

                //UserFactory userFactory = new UserFactory(_dbc);
                UserFacade userFacade = new UserFacade(_dbc);

                JsonAnswerStatus jsonAnswer;
                if (userNewDTO.is_need_curator > 1)
                {
                    PreUserWithAppointmentFacade preUserWithAppointmentFacade = new PreUserWithAppointmentFacade(_dbc);
                    jsonAnswer = await preUserWithAppointmentFacade.registration(_httpContextAccessor.HttpContext.Request.Host.Value, userNewDTO);
                } else
                {
                    jsonAnswer = userFacade.addByRegistration(userNewDTO);
                }


                if (jsonAnswer.user != null)
                {
                    await signIn(jsonAnswer.user);
                    jsonAnswer.user = null;


                }

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
        [Route("logout")]
        //[Authorize(AuthenticationSchemes = "UserCookie")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return Ok();
        }

        [HttpPost]
        [Route("save")]
        [Authorize(AuthenticationSchemes = "UserCookie")]
        public async Task<IActionResult> save([FromForm]UserProfileDTO userProfileDTO)
        {
            if (ModelState.IsValid)
            {
                UserFacade userFacade = new UserFacade(this._dbc);

                JsonAnswerViewModel jsonAnswer = userFacade.save(HttpContext, userProfileDTO);

                if(jsonAnswer.user != null)
                {
                    await HttpContext.SignOutAsync();

                    var claims = new List<Claim>()
                    {
                        new Claim(ClaimTypes.NameIdentifier, jsonAnswer.user.Id.ToString()),
                        new Claim(ClaimTypes.Name, jsonAnswer.user.Username),
                        new Claim(ClaimTypes.Role, "User")
                    };
                    ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "UserCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
                    ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

                    await HttpContext.SignInAsync(claimsPrincipal);
                }

                return Ok(new JsonAnswerViewModel(jsonAnswer.status, jsonAnswer.errors));
            }

            return Ok();
        }


        [HttpPost]
        [Route("app/get")]
        [Authorize(AuthenticationSchemes = "UserJWT")]
        public IActionResult appGet()
        {
            UserFacade userFacade = new UserFacade(this._dbc);
            UserProfileViewModel userProfileViewModel = userFacade.getUserProfile(HttpContext);
            if (userProfileViewModel == null) return Ok(new JsonAnswerStatus("error", "unknown"));
            return Ok(new JsonAnswerStatus("success", null, userProfileViewModel));
        }

        [HttpPost]
        [Route("app/save")]
        [Authorize(AuthenticationSchemes = "UserJWT")]
        public IActionResult appSave([FromForm] UserProfileDTO userProfileDTO)
        {
            if (ModelState.IsValid)
            {
                UserFacade userFacade = new UserFacade(this._dbc);
                JsonAnswerViewModel jsonAnswer = userFacade.save(HttpContext, userProfileDTO);
                if (jsonAnswer.user != null)
                {
                    jsonAnswer.access_token = this.generateJWT(jsonAnswer.user);
                    jsonAnswer.user = null;
                }
                return Ok(jsonAnswer);
            }
            return Ok(new JsonAnswerStatus("error", "no_data"));
        }




        [HttpPost]
        [AllowAnonymous]
        [Route("forget")]
        public IActionResult forget([FromForm] UserForgetDTO userForgetDTO)
        {
            if (ModelState.IsValid)
            {
                UserFacade userFacade = new UserFacade(this._dbc);
                return Ok(userFacade.forget(userForgetDTO));
            }
            return Ok();
        }





        [HttpPost]
        [Route("search")]
        public IActionResult search([FromForm] UserSearchDTO UserSearchDTO)
        {
            if (ModelState.IsValid)
            {
                UserFacade userFacade = new UserFacade(this._dbc);
                return Ok(userFacade.search(UserSearchDTO));
            }
            return Ok();
        }


        [HttpPost]
        [Route("get")]
        public IActionResult get([FromForm] UserIdDTO userIdDTO)
        {
            if (ModelState.IsValid)
            {
                UserFacade userFacade = new UserFacade(this._dbc);
                UserEditViewModel userEditViewModel = userFacade.get(userIdDTO.id);
                return Ok(
                    userEditViewModel != null
                    ? new JsonAnswerStatus("success", null, userEditViewModel)
                    : new JsonAnswerStatus("error", "not_found")
                );
            }
            return Ok();
        }

        [Route("edit")]
        public IActionResult Edit([FromForm] UserEditDTO userEditDTO)
        {
            if (ModelState.IsValid)
            {
                UserFacade userFacade = new UserFacade(_dbc);
                return Ok(userFacade.edit(userEditDTO));
            }
            return Ok(new JsonAnswerStatus("error", null));
        }


        [HttpPost]
        [Route("check_count_active_packages")]
        public IActionResult checkCountActivePackages()
        {
            UserFacade userFacade = new UserFacade(_dbc);
            return Ok(userFacade.checkCountActivePackages(HttpContext));
        }


        /*
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
        */


        [Route("app/login")]
        public IActionResult appLogin([FromForm] UserLoginDTO userLoginDTO)
        {
            if (ModelState.IsValid)
            {
                UserFacade userFacade = new UserFacade(_dbc);

                JsonAnswerViewModel jsonAnswer = userFacade.login(userLoginDTO);

                if (jsonAnswer.user != null)
                {
                    jsonAnswer.access_token = this.generateJWT(jsonAnswer.user);
                    jsonAnswer.user = null;
                }
                userFacade = null;

                return Ok(jsonAnswer);

            } else
            {
                return Ok(new JsonAnswerStatus("error", "no_data"));
            }

            /*
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, "1"),
                new Claim(ClaimTypes.Name, "emailHere"),
                new Claim(ClaimTypes.Role, "UserJWT")
            };
            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "UserJWT", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);


            var now = DateTime.UtcNow;
            // создаем JWT-токен
            var jwt = new JwtSecurityToken(
                    issuer: AuthUserJWTOptions.ISSUER,
                    audience: AuthUserJWTOptions.AUDIENCE,
                    notBefore: now,
                    claims: claimsIdentity.Claims,
                    //expires: now.Add(TimeSpan.FromMinutes(AuthUserJWTOptions.LIFETIME)),
                    signingCredentials: new SigningCredentials(AuthUserJWTOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
            var access_token = new JwtSecurityTokenHandler().WriteToken(jwt);
            */

            //return Ok(new { access_token });

        }

        [HttpPost]
        [Route("app/registration")]
        public async Task<IActionResult> appRegistration([FromForm] UserNewDTO userNewDTO)
        {
            if (ModelState.IsValid)
            {
                UserFacade userFacade = new UserFacade(_dbc);

                JsonAnswerStatus jsonAnswer;
                if (userNewDTO.is_need_curator > 1)
                {
                    PreUserWithAppointmentFacade preUserWithAppointmentFacade = new PreUserWithAppointmentFacade(_dbc);
                    jsonAnswer = await preUserWithAppointmentFacade.registration(_httpContextAccessor.HttpContext.Request.Host.Value, userNewDTO);
                }
                else
                {
                    jsonAnswer = userFacade.addByRegistration(userNewDTO);
                }

                if (jsonAnswer.user != null)
                {
                    JsonAnswerViewModel jsonAnswerViewModel = new JsonAnswerViewModel("success", null);
                    jsonAnswerViewModel.access_token = this.generateJWT(jsonAnswer.user);
                    jsonAnswer.user = null;
                    return Ok(jsonAnswerViewModel);
                }
                return Ok(jsonAnswer);
            }

            return Ok(new JsonAnswerStatus("error", "no_data"));
        }



        [Route("app/secret")]
        [Authorize(AuthenticationSchemes = "UserJWT")]
        public IActionResult appSecret()
        {
            UserFacade userFacade = new UserFacade(this._dbc);
            User user = userFacade.getCurrentOrNull(HttpContext);
            int id = (user != null ? user.Id : 0);

            return Ok( User.FindFirstValue(ClaimTypes.NameIdentifier));
        }

        [Route("app/secret2")]
        [Authorize(AuthenticationSchemes = "UserJWT")]
        [AllowAnonymous]
        public IActionResult appSecret2()
        {
            UserFacade userFacade = new UserFacade(this._dbc);
            User user = userFacade.getCurrentOrNull(HttpContext);
            int id = (user != null ? user.Id : 0);

            if (user != null) return Ok(User.FindFirstValue(ClaimTypes.NameIdentifier));
            return Ok();
        }


        private string generateJWT(User user)
        {
            var claims = new List<Claim>()
                    {
                        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                        new Claim(ClaimTypes.Name, user.Username),
                        new Claim(ClaimTypes.Role, "User")
                    };
            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "UserJWT", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);

            var now = DateTime.UtcNow;
            // создаем JWT-токен
            var jwt = new JwtSecurityToken(
                    issuer: AuthUserJWTOptions.ISSUER,
                    audience: AuthUserJWTOptions.AUDIENCE,
                    notBefore: now,
                    claims: claimsIdentity.Claims,
                    //expires: now.Add(TimeSpan.FromMinutes(AuthUserJWTOptions.LIFETIME)),
                    signingCredentials: new SigningCredentials(AuthUserJWTOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }

    }
}