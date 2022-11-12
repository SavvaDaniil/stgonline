using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Net.Http.Headers;
using STG.Data;
using STG.Facade;
using STG.Models;
using STG.Observer;
using STG.Service;
using STG.ViewModels;
using STG.ViewModels.User;

namespace STG.Controllers
{
    [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
    public class HomeController : Controller
    {

        private ApplicationDbContext _dbc;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private IServiceScopeFactory _serviceScopeFactory;
        public HomeController(IHttpContextAccessor httpContextAccessor, ApplicationDbContext dbc, IServiceScopeFactory serviceScopeFactory)
        {
            _dbc = dbc;
            _httpContextAccessor = httpContextAccessor;
            _serviceScopeFactory = serviceScopeFactory;
        }
        private static bool is_checked_extends = false;



        public IActionResult Index()
        {

            TeacherFacade teacherFacade = new TeacherFacade(_dbc);

            List<(int, string)> l = new List<(int, string)>();
            l.Add((1, "Сотни классов для танцоров любого уровня подготовки"));
            l.Add((2, "Уникальная возможность развития под руководством топовых наставников"));
            l.Add((3, "Секреты и практики от многократных чемпионов"));

            SubscriptionFacade subscriptionFacade = new SubscriptionFacade(_dbc);
            RegionService regionService = new RegionService(_dbc);

            FirstViewModel firstViewModel = new FirstViewModel(
                l,
                null,//await teacherFacade.listAllLiteForIndexActive(),
                teacherFacade.listAllCurator(),
                subscriptionFacade.listAllActiveForAnyLesson(null),
                regionService.listAll()
            );
            teacherFacade = null;

            ViewData["idMenuActive"] = 1;

            return View(firstViewModel);
        }


        public IActionResult Index2()
        {
            TeacherFacade teacherFacade = new TeacherFacade(_dbc);

            List<(int, string)> l = new List<(int, string)>();
            l.Add((1, "Сотни классов для любого уровня подготовки"));
            l.Add((2, "Уникальная возможность развития под руководством топовых наставников"));
            l.Add((3, "Секреты и практики от многократных чемпионов"));

            SubscriptionFacade subscriptionFacade = new SubscriptionFacade(_dbc);
            RegionService regionService = new RegionService(_dbc);

            FirstViewModel firstViewModel = new FirstViewModel(
                l,
                teacherFacade.listAllLiteForIndexActive(),
                teacherFacade.listAllCurator(),
                subscriptionFacade.listAllActiveForAnyLesson(null),
                regionService.listAll()
            );

            ViewData["idMenuActive"] = 1;
            Response.Headers[HeaderNames.CacheControl] = "no-cache, no-store, must-revalidate";
            Response.Headers[HeaderNames.Expires] = "0";
            Response.Headers[HeaderNames.Pragma] = "no-cache";

            return View("index1", firstViewModel);
        }



        public IActionResult Lessons()
        {
            string path = Directory.GetCurrentDirectory();
            string uploadsForFiles = path + "/wwwroot/uploads";
            LevelService levelService = new LevelService(this._dbc);
            ViewData["levelList"] = levelService.listAll();

            return View();
        }


        public IActionResult Login()
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index");
            }

            ViewData["idMenuActive"] = 0;
            return View();
        }

        public IActionResult Forget()
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index");
            }

            ViewData["idMenuActive"] = 0;
            return View();
        }

        public IActionResult Registration()
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index");
            }

            RegionService regionService = new RegionService(_dbc);
            TeacherFacade teacherFacade = new TeacherFacade(_dbc);
            RegistrationViewModel registrationViewModel = new RegistrationViewModel(
                regionService.listAll(),
                teacherFacade.listAllCurator()
             );


            ViewData["idMenuActive"] = 0;
            return View(registrationViewModel);
        }

        public IActionResult Auth()
        {
            List<Claim> grandmaClaims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, "Bob"),
                new Claim(ClaimTypes.Email, "Bob@fmail.com"),
                new Claim("Grandma.Saya", "Naya")
            };

            List<Claim> licenceClaims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, "Bob K Foo"),
                new Claim("DrivingLicence", "A++")
            };


            ClaimsIdentity grandmaidentity = new ClaimsIdentity(grandmaClaims, "Grandma Identity");
            ClaimsIdentity licenceIdentity = new ClaimsIdentity(licenceClaims, "Government");

            ClaimsPrincipal userPrincipal = new ClaimsPrincipal(new[] { grandmaidentity, licenceIdentity });

            HttpContext.SignInAsync(userPrincipal);
            
            return RedirectToAction("Index");
        }

        public IActionResult Logout()
        {

            HttpContext.SignOutAsync();

            return RedirectToAction("Index");
        }

        [Authorize(AuthenticationSchemes = "CookieAuth")]
        public IActionResult Secret()
        {

            ViewData["idMenuActive"] = 0;
            return View();
        }

        [Authorize(AuthenticationSchemes = "UserCookie")]
        public IActionResult Profile()
        {
            ViewData["idMenuActive"] = 0;
            UserFacade userFacade = new UserFacade(this._dbc);
            UserProfileViewModel userProfileViewModel = userFacade.getUserProfile(HttpContext);
            if(userProfileViewModel == null)
            {
                HttpContext.SignOutAsync();
                return Redirect("/");
            }

            return View(userProfileViewModel);
        }
        
        public IActionResult Privacy()
        {
            ViewData["idMenuActive"] = 0;
            return View();
        }

        public IActionResult Agreement()
        {
            ViewData["idMenuActive"] = 0;
            return View();
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            ViewData["idMenuActive"] = 0;
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }
}
