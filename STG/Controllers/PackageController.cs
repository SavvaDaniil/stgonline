using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using STG.Data;
using STG.Facade;
using STG.ViewModels.Package;
using STG.ViewModels.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace STG.Controllers
{
    [Authorize(AuthenticationSchemes = "UserCookie")]
    public class PackageController : Controller
    {
        ApplicationDbContext _dbc;

        public PackageController(ApplicationDbContext dbc)
        {
            this._dbc = dbc;
        }

        [HttpGet]
        [Route("/package/{id}")]
        public async Task<IActionResult> Index(int id)
        {
            ViewData["idMenuActive"] = 0;
            PackageFacade packageFacade = new PackageFacade(_dbc);
            PackageInfoViewModel packageInfoViewModel = await packageFacade.getFullInfoForUser(HttpContext, id);
            if (packageInfoViewModel == null) return Redirect("/packages");

            return View(packageInfoViewModel);
        }

        [HttpGet]
        [Route("/package/chat/{id}")]
        public async Task<IActionResult> Chat(int id)
        {
            ViewData["idMenuActive"] = 0;
            PackageFacade packageFacade = new PackageFacade(_dbc);

            UserPassingPackageViewModel userPassingPackageViewModel = await packageFacade.getUserPassingPackageForUser(HttpContext, id);

            if (userPassingPackageViewModel == null) return Redirect("/packages");

            return View(userPassingPackageViewModel);
        }

        public async Task<IActionResult> Packages()
        {
            ViewData["idMenuActive"] = 3;
            PackageFacade packageFacade = new PackageFacade(_dbc);
            ListPackagePreviewViewModel listPackagePreviewViewModel = new ListPackagePreviewViewModel(
                await packageFacade.listAllPrivatePreview(HttpContext)
            );

            return View(listPackagePreviewViewModel);
        }

        [HttpGet]
        [Route("/package/buy/{id}")]
        public async Task<IActionResult> Buy(int id)
        {
            ViewData["idMenuActive"] = 0;

            PackageFacade packageFacade = new PackageFacade(_dbc);
            PackageBuyViewModel packageBuyViewModel = await packageFacade.getInfoForBuying(id);

            return View(packageBuyViewModel);
        }
    }
}
