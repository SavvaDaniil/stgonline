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
        //IWebHostEnvironment _appEnvironment;

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



        public async Task<IActionResult> Index()
        {
            /*
            System.Diagnostics.Debug.WriteLine("Роль: " + HttpContext.User.FindFirstValue(ClaimTypes.Role));

            foreach (var claim in User.Claims.ToList())
            {
                System.Diagnostics.Debug.WriteLine("Claim: " + claim.Type + " | " + claim.Value);
            }
            */

            if (!is_checked_extends)
            {
                if (_httpContextAccessor.HttpContext.Request.Host.Value == "stgonline.pro") {
                    ExtendFacade extendFacade = new ExtendFacade(_dbc, _serviceScopeFactory);
                    await extendFacade.checkExtendsAndLaunch(_httpContextAccessor.HttpContext.Request.Host.Value);
                }

                is_checked_extends = true;
            }

            TeacherFacade teacherFacade = new TeacherFacade(_dbc);

            List<(int, string)> l = new List<(int, string)>();
            l.Add((1, "Сотни классов для танцоров любого уровня подготовки"));
            l.Add((2, "Уникальная возможность развития под руководством топовых наставников"));
            l.Add((3, "Секреты и практики от многократных чемпионов"));

            SubscriptionFacade subscriptionFacade = new SubscriptionFacade(_dbc);
            RegionService regionService = new RegionService(_dbc);

            FirstViewModel firstViewModel = new FirstViewModel(
                l,
                await teacherFacade.listAllLiteForIndexActive(),
                await teacherFacade.listAllCurator(),
                await subscriptionFacade.listAllActiveForAnyLesson(null),
                await regionService.listAll()
            );
            teacherFacade = null;

            ViewData["idMenuActive"] = 1;
            //Response.Headers[HeaderNames.CacheControl] = "no-cache, no-store, must-revalidate";
            //Response.Headers[HeaderNames.Expires] = "0";
            //Response.Headers[HeaderNames.Pragma] = "no-cache";

            return View(firstViewModel);
        }


        public async Task<IActionResult> Index2()
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
                await teacherFacade.listAllLiteForIndexActive(),
                await teacherFacade.listAllCurator(),
                await subscriptionFacade.listAllActiveForAnyLesson(null),
                await regionService.listAll()
            );

            ViewData["idMenuActive"] = 1;
            Response.Headers[HeaderNames.CacheControl] = "no-cache, no-store, must-revalidate";
            Response.Headers[HeaderNames.Expires] = "0";
            Response.Headers[HeaderNames.Pragma] = "no-cache";

            return View("index1", firstViewModel);
        }



        public async Task<IActionResult> Lessons()
        {
            string path = Directory.GetCurrentDirectory();
            string uploadsForFiles = path + "/wwwroot/uploads";
            if (!Directory.Exists(uploadsForFiles))
            {
                //Directory.CreateDirectory(uploadsForFiles);
            }

            //List<LessonLiteViewModel> lessons = await LessonFacade.getAllActiveByFilter(_dbc, null, null, null, null, 0);
            //System.Diagnostics.Debug.WriteLine("lessons = " + lessons);
            //List<Level> levelList = await LevelService.listAll(this._dbc);
            LevelService levelService = new LevelService(this._dbc);
            ViewData["levelList"] = await levelService.listAll();

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

        public async Task<IActionResult> Registration()
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index");
            }

            RegionService regionService = new RegionService(_dbc);
            TeacherFacade teacherFacade = new TeacherFacade(_dbc);
            RegistrationViewModel registrationViewModel = new RegistrationViewModel(
                await regionService.listAll(),
                await teacherFacade.listAllCurator()
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
            //Console.WriteLine(ClaimsPrincipal.Current.Identities.First().Claims.ToString());
            //Console.WriteLine(ClaimsPrincipal.Current.FindFirstValue(ClaimTypes.NameIdentifier));

            /*
            System.Diagnostics.Debug.WriteLine(HttpContext.User.Identity.IsAuthenticated);
            System.Diagnostics.Debug.WriteLine("+++++++++++++++++++++++++++++++++++++++++++");
            System.Diagnostics.Debug.WriteLine("+++++++++ NameIdentifier = " + HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
            */

            ViewData["idMenuActive"] = 0;
            return View();
        }

        [Authorize(AuthenticationSchemes = "UserCookie")]
        public async Task<IActionResult> Profile()
        {
            ViewData["idMenuActive"] = 0;
            UserFacade userFacade = new UserFacade(this._dbc);
            UserProfileViewModel userProfileViewModel = await userFacade.getUserProfile(HttpContext);
            if(userProfileViewModel == null)
            {
                await HttpContext.SignOutAsync();
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


        public async Task<IActionResult> Test()
        {
            ViewData["idMenuActive"] = 0;
            System.Diagnostics.Debug.WriteLine("action test");

            /*
            StatementObserver statementObserver = new StatementObserver();
            StatementService statementService = new StatementService(_dbc);
            Statement statement = await statementService.findById(5);


            if (statement != null)
            {
                System.Diagnostics.Debug.WriteLine(statement.listOfTeachers);
                TeacherService teacherService = new TeacherService(_dbc);
                List<Teacher> curators = await teacherService.listAllByListString(statement.listOfTeachers);

                statementObserver.sendStatementToAdmins(statement, curators);
            } else
            {
                System.Diagnostics.Debug.WriteLine("statement не найден");
            }
            */
            /*
            AmoCRMFacade amoCRMFacade = new AmoCRMFacade(_dbc);
            await amoCRMFacade.test();
            */
            int start = 0;
            int end = 0;
            
            FindElementsForSum(new List<uint> { 0, 1, 2, 3, 4, 5, 6, 7 }, 11, out start, out end); //start будет равен 5 и end 7;
            System.Diagnostics.Debug.WriteLine("start: " + start + " | end: " + end + "\n--------------------------------------------");
            FindElementsForSum(new List<uint> { 4, 5, 6, 7 }, 18, out start, out end); //start будет равен 1 и end 4;
            System.Diagnostics.Debug.WriteLine("start: " + start + " | end: " + end + "\n--------------------------------------------");

            FindElementsForSum(new List<uint> { 0, 1, 2, 3, 4, 5, 6, 7 }, 88, out start, out end); //start будет равен 0 и end 0;
            System.Diagnostics.Debug.WriteLine("start: " + start + " | end: " + end + "\n--------------------------------------------");
            



            FindElementsForSum(new List<uint> { 0, 5, 88, 100}, 4, out start, out end); ;
            FindElementsForSum(new List<uint> { 0, 1, 2, 3 }, 1, out start, out end);
            FindElementsForSum(new List<uint> { 0, 8, 9, 11 }, 11, out start, out end);


            return View("Secret");
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            ViewData["idMenuActive"] = 0;
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private void FindElementsForSum(List<uint> list, ulong sum, out int start, out int end)
        {
            start = 0;
            end = 0;
            if (sum == 0) return;

            uint innerSum = 0;
            bool isFinished = false;
            int j = 0;
            for (int i = 0; i < list.Count; i++)
            {
                if (isFinished) break;
                innerSum = list[i];
                if (innerSum > sum) break;
                j = i + 1;

                while (j < list.Count + 1)
                {

                    if (innerSum == sum)
                    {
                        System.Diagnostics.Debug.WriteLine("Финиш ");
                        start = i;
                        end = j;
                        isFinished = true;
                        break;
                    }
                    if (innerSum > sum) break;
                    if (j >= list.Count) break;
                    System.Diagnostics.Debug.WriteLine("i = " + i + "(" + list[i] + ") | j = " + j + "(" + list[j] + ") | текущее innerSum: " + innerSum);
                    System.Diagnostics.Debug.WriteLine("innerSum " + innerSum + " + list[j] " + list[j] + " = " + (innerSum + list[j]));
                    innerSum += list[j];
                    j++;
                }

                if (i == 0 && innerSum < sum) isFinished = true;


            }
            System.Diagnostics.Debug.WriteLine("Ответ start: " + start + " | end: " + end + "\n---------------------");
        }
    }
}
