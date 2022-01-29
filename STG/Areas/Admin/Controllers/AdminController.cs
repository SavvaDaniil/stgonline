using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using STG.Data;
using STG.DTO;
using STG.Facade;
using STG.Models;
using STG.Service;
using STG.ViewModels;
using STG.ViewModels.Video;
using STG.ViewModels.Lesson;
using STG.ViewModels.Admin;
using STG.ViewModels.Package;
using STG.ViewModels.TeacherViewModel;
using STG.ViewModels.PackageChat;

namespace STG.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(AuthenticationSchemes = "AdminCookie")]
    public class AdminController : Controller
    {
        private ApplicationDbContext _dbc;
        public AdminController(ApplicationDbContext dbc)
        {
            _dbc = dbc;
        }

        public async Task<IActionResult> Index()
        {
            AdminFacade adminFacade = new AdminFacade(_dbc);
            AdminProfileViewModel adminProfileViewModel = await adminFacade.getCurrentProfile(HttpContext);
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                System.Diagnostics.Debug.WriteLine("Авторизирован");
            }

            ViewData["menuActive"] = "";
            ViewData["AdminAuthorizeViewModel"] = await adminFacade.getAuthorizeViewModel(HttpContext, "");
            return View(adminProfileViewModel);
        }

        //[Authorize(AuthenticationSchemes = "AdminCookie")]
        public async Task<IActionResult> Teachers()
        {
            ViewData["menuActive"] = "teachers";
            AdminFacade adminFacade = new AdminFacade(_dbc);
            ViewData["AdminAuthorizeViewModel"] = await adminFacade.getAuthorizeViewModel(HttpContext, "teachers");
            TeacherService teacherService = new TeacherService(this._dbc);
            return View(new EnumTeachersViewModel(await teacherService.listAll()));
        }

        [HttpGet]
        [Route("~/admin/teacher/{id}")]
        public async Task<IActionResult> Teacher(int id)
        {
            ViewData["menuActive"] = "";
            AdminFacade adminFacade = new AdminFacade(_dbc);
            ViewData["AdminAuthorizeViewModel"] = await adminFacade.getAuthorizeViewModel(HttpContext);
            TeacherFacade teacherFacade = new TeacherFacade(_dbc);
            TeacherLiteViewModel teacherLiteViewModel = await teacherFacade.get(id);
            if (teacherLiteViewModel == null) return RedirectToAction("Teachers");
            teacherFacade = null;
            return View(teacherLiteViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Styles()
        {
            AdminFacade adminFacade = new AdminFacade(_dbc);
            ViewData["AdminAuthorizeViewModel"] = await adminFacade.getAuthorizeViewModel(HttpContext, "styles");
            ViewData["menuActive"] = "styles";
            StyleService styleService = new StyleService(this._dbc);
            return View(new EnumStylesViewModel(await styleService.listAll()));
        }

        [HttpGet]
        public async Task<IActionResult> Lessontypes()
        {
            AdminFacade adminFacade = new AdminFacade(_dbc);
            ViewData["AdminAuthorizeViewModel"] = await adminFacade.getAuthorizeViewModel(HttpContext, "lessontypes");
            ViewData["menuActive"] = "lessontypes";
            LessonTypeService lessonTypeService = new LessonTypeService(this._dbc);
            return View(new EnumLessonTypesViewModel(await lessonTypeService.listAll()));
        }

        [HttpGet]
        [Route("~/admin/video/{id}")]
        public async Task<IActionResult> Video(int id)
        {
            ViewData["menuActive"] = "";
            AdminFacade adminFacade = new AdminFacade(_dbc);
            ViewData["AdminAuthorizeViewModel"] = await adminFacade.getAuthorizeViewModel(HttpContext);
            VideoFacade videoFacade = new VideoFacade(this._dbc);
            VideoViewModel videoViewModel = await videoFacade.get(id);

            if (videoViewModel == null) return RedirectToAction("Videos");

            /*
            StyleService styleService = new StyleService(this._dbc);
            IEnumerable<Style> styles = await styleService.listAll();

            LessonTypeService lessonTypeService = new LessonTypeService(this._dbc);
            IEnumerable<LessonType> lessonTypes = await lessonTypeService.listAll();
            */

            //videoViewModel.videoConverterStatusViewModel = await videoFacade.getStatusVideoConverterApi();
            //videoViewModel.videoApiStatusViewModel = await videoFacade.getStatusOfVideoFromApi(videoViewModel.id, videoViewModel.hashPath);

            //System.Diagnostics.Debug.WriteLine("videoViewModel.videoApiStatusViewModel.status: " + videoViewModel.videoApiStatusViewModel.status);

            return View(videoViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Videos()
        {
            ViewData["menuActive"] = "videos";
            AdminFacade adminFacade = new AdminFacade(_dbc);
            ViewData["AdminAuthorizeViewModel"] = await adminFacade.getAuthorizeViewModel(HttpContext, "videos");
            VideoFacade videoFacade = new VideoFacade(this._dbc);
            return View(new EnumVideoLiteViewModel(await videoFacade.listAllLite()));
        }

        [HttpGet]
        public async Task<IActionResult> Lessons()
        {
            ViewData["menuActive"] = "lessons";
            AdminFacade adminFacade = new AdminFacade(_dbc);
            ViewData["AdminAuthorizeViewModel"] = await adminFacade.getAuthorizeViewModel(HttpContext, "lessons");
            LessonFacade lessonFacade = new LessonFacade(this._dbc);
            return View(new EnumLessonMicroViewModel(await lessonFacade.listAllByMicro()));
        }

        [HttpGet]
        [Route("~/admin/lesson/{id}")]
        public async Task<IActionResult> Lesson(int id)
        {
            AdminFacade adminFacade = new AdminFacade(_dbc);
            ViewData["AdminAuthorizeViewModel"] = await adminFacade.getAuthorizeViewModel(HttpContext);
            ViewData["menuActive"] = "";
            LessonFacade lessonFacade = new LessonFacade(this._dbc);
            LessonViewModel lessonViewModel = await lessonFacade.getFullInfo(id);
            if (lessonViewModel == null) return null;

            StyleService styleService = new StyleService(this._dbc);
            //IEnumerable<Style> styles = await styleService.listAll();

            LessonTypeService lessonTypeService = new LessonTypeService(this._dbc);
            //IEnumerable<LessonType> lessonTypes = await lessonTypeService.listAll();

            LevelService levelService = new LevelService(this._dbc);
            //IEnumerable<Level> levels = await levelService.listAll();

            TeacherService teacherService = new TeacherService(this._dbc);
            //IEnumerable<Teacher> teachers = await teacherService.listAll();

            VideoFacade videoFacade = new VideoFacade(_dbc);

            return View(
                new LessonEditViewModel(
                    lessonViewModel,
                    await levelService.listAll(),
                    await lessonTypeService.listAll(),
                    await styleService.listAll(),
                    await teacherService.listAll(),
                    await videoFacade.listAllOtionNameDuration()
                    )
            );
        }

        public async Task<IActionResult> Packages()
        {
            ViewData["menuActive"] = "packages";
            AdminFacade adminFacade = new AdminFacade(_dbc);
            ViewData["AdminAuthorizeViewModel"] = await adminFacade.getAuthorizeViewModel(HttpContext, "packages");
            PackageFacade packageFacade = new PackageFacade(this._dbc);

            ListPackageLiteViewModel listPackageLiteViewModel = new ListPackageLiteViewModel(await packageFacade.listAll());

            return View(listPackageLiteViewModel);
        }

        [HttpGet]
        [Route("~/admin/package/{id}")]
        public async Task<IActionResult> Package(int id)
        {
            ViewData["menuActive"] = "";
            AdminFacade adminFacade = new AdminFacade(_dbc);
            ViewData["AdminAuthorizeViewModel"] = await adminFacade.getAuthorizeViewModel(HttpContext);
            PackageFacade packageFacade = new PackageFacade(_dbc);

            PackageEditViewModel packageEditViewModel = await packageFacade.getForEdit(id);
            packageFacade = null;
            if (packageEditViewModel == null) return RedirectToAction("Packages");
            return View(packageEditViewModel);
        }





        [AllowAnonymous]
        public async Task<IActionResult> Login()
        {
            AdminService adminService = new AdminService(_dbc);
            await adminService.checkBasicExist();


            //System.Diagnostics.Debug.WriteLine("User: " + ((HttpContext.User != null) ? (HttpContext.User.FindFirst(x => x.Type == ClaimsIdentity.DefaultRoleClaimType).Value) : ""));
            System.Diagnostics.Debug.WriteLine("Роль: " + HttpContext.User.FindFirstValue(ClaimTypes.Role));

            foreach (var claim in User.Claims.ToList())
            {
                System.Diagnostics.Debug.WriteLine("Claim: " + claim.Type + " | " + claim.Value);
            }

                if (HttpContext.User.Identity.IsAuthenticated)
            {
                System.Diagnostics.Debug.WriteLine("User: " + ((HttpContext.User != null) ? (HttpContext.User.FindFirst(x => x.Type == ClaimsIdentity.DefaultRoleClaimType).Value) : ""));

                string role = HttpContext.User.FindFirst(x => x.Type == ClaimsIdentity.DefaultRoleClaimType).Value;
                System.Diagnostics.Debug.WriteLine("Роль DefaultRoleClaimType: " + role);

                if (HttpContext.User.FindFirstValue(ClaimTypes.Role) == "Admin")
                {
                    return RedirectToAction("Index");
                }
            }


            return View();
        }


        [HttpGet]
        public async Task<IActionResult> Admins()
        {
            AdminFacade adminFacade = new AdminFacade(_dbc);
            ViewData["AdminAuthorizeViewModel"] = await adminFacade.getAuthorizeViewModel(HttpContext, "admins");
            ViewData["menuActive"] = "admins";
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Users()
        {
            ViewData["menuActive"] = "users";
            AdminFacade adminFacade = new AdminFacade(_dbc);
            ViewData["AdminAuthorizeViewModel"] = await adminFacade.getAuthorizeViewModel(HttpContext, "users");
            return View();
        }


        [HttpGet]
        public async Task<IActionResult> Mentoring()
        {
            ViewData["menuActive"] = "mentoring";
            AdminFacade adminFacade = new AdminFacade(_dbc);
            ViewData["AdminAuthorizeViewModel"] = await adminFacade.getAuthorizeViewModel(HttpContext, "mentoring");

            TeacherFacade teacherFacade = new TeacherFacade(_dbc);
            ListTeacherCuratorPreviewsViewModel listPackageChatPreviewsViewModel = await teacherFacade.listAllCuratorForMentoring();

            return View(listPackageChatPreviewsViewModel);
        }


        [HttpGet]
        [Route("~/admin/homeworks/{id_of_teacher}")]
        public async Task<IActionResult> Homeworks(int id_of_teacher)
        {
            ViewData["menuActive"] = "homeworks";
            AdminFacade adminFacade = new AdminFacade(_dbc);
            ViewData["AdminAuthorizeViewModel"] = await adminFacade.getAuthorizeViewModel(HttpContext, "homeworks");

            PackageChatFacade packageChatFacade = new PackageChatFacade(_dbc);
            ListPackageChatPreviewsViewModel listPackageChatPreviewsViewModel = await packageChatFacade.getListPreviewByIdOfMentor(id_of_teacher);

            return View(listPackageChatPreviewsViewModel);
        }


        [HttpGet]
        public async Task<IActionResult> Statements()
        {
            ViewData["menuActive"] = "statements";
            AdminFacade adminFacade = new AdminFacade(_dbc);
            ViewData["AdminAuthorizeViewModel"] = await adminFacade.getAuthorizeViewModel(HttpContext, "statements");
            return View();
        }

    }
}