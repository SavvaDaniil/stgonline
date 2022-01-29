using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using STG.Data;
using STG.Facade;
using STG.Service;
using STG.ViewModels.Lesson;

namespace STG.Controllers
{
    [Authorize(AuthenticationSchemes = "UserCookie")]
    public class LessonController : Controller
    {
        ApplicationDbContext _dbc;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private IServiceScopeFactory _serviceScopeFactory;
        public LessonController(IHttpContextAccessor httpContextAccessor, ApplicationDbContext dbc, IServiceScopeFactory serviceScopeFactory)
        {
            this._dbc = dbc;
            _httpContextAccessor = httpContextAccessor;
            _serviceScopeFactory = serviceScopeFactory;
        }


        [HttpGet]
        [Route("/lesson/{id}")]
        public async Task<IActionResult> Index(int id, [FromQuery(Name = "package")] string package)
        {
            ViewData["idMenuActive"] = 0;
            LessonFacade lessonFacade = new LessonFacade(_dbc, _serviceScopeFactory);
            int access_none0_buy1_granted2 = await lessonFacade.checkAccessForUserAndUpdateAccessIfNull_none0_buy1_granted2(HttpContext, _httpContextAccessor.HttpContext.Request.Host.Value, id);

            if (access_none0_buy1_granted2 == 0)
            {
                if(package != null) return Redirect("/package/" + package);
                return Redirect("/lessons");
            } else if (access_none0_buy1_granted2 == 1)
            {
                return Redirect("/lesson/buy/" + id.ToString());
            }
            LessonVideoViewModel lessonVideoViewModel = await lessonFacade.getWithVideoWithAddInfo(HttpContext, id, package);

            return View("IndexNativeJS", lessonVideoViewModel);
        }


        [HttpGet]
        [Route("/lesson_test")]
        public async Task<IActionResult> IndexLesson()
        {
            int id = 23;string package = null;
            ViewData["idMenuActive"] = 0;
            LessonFacade lessonFacade = new LessonFacade(_dbc, _serviceScopeFactory);
            int access_none0_buy1_granted2 = await lessonFacade.checkAccessForUserAndUpdateAccessIfNull_none0_buy1_granted2(HttpContext, _httpContextAccessor.HttpContext.Request.Host.Value, id);

            if (access_none0_buy1_granted2 == 0)
            {
                if (package != null) return Redirect("/package/" + package);
                return Redirect("/lessons");
            }
            else if (access_none0_buy1_granted2 == 1)
            {
                return Redirect("/lesson/buy/" + id.ToString());
            }
            LessonVideoViewModel lessonVideoViewModel = await lessonFacade.getWithVideoWithAddInfo(HttpContext, id, package);

            return View("IndexNativeJSTest", lessonVideoViewModel);
        }



        [HttpGet]
        [AllowAnonymous]
        [Route("/lessons")]
        public async Task<IActionResult> Lessons()
        {
            ViewData["idMenuActive"] = 2;
            LevelService levelService = new LevelService(this._dbc);
            ViewData["levelList"] = await levelService.listAll();
            return View();
        }

        [HttpGet]
        [Route("/lesson/buy/{id}")]
        public async Task<IActionResult> Buy(int id)
        {
            ViewData["idMenuActive"] = 0;
            /*
            PurchaseLessonFacade purchaseLessonFacade = new PurchaseLessonFacade(_dbc);
            if(await purchaseLessonFacade.getLastActive(HttpContext, id) != null){
                return RedirectToAction("Lessons");
            }
            */
            LessonFacade lessonFacade = new LessonFacade(_dbc);
            LessonBuyViewModel lessonBuyViewModel = await lessonFacade.getInfoForBuying(HttpContext, id);

            if (lessonBuyViewModel == null) return RedirectToAction("Lessons");

            return View(lessonBuyViewModel);
        }
    }
}