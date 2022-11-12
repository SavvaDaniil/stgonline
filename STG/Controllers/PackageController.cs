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
        [AllowAnonymous]
        [Route("/package/{id}")]
        public IActionResult Index(int id)
        {
            ViewData["idMenuActive"] = 0;
            PackageFacade packageFacade = new PackageFacade(_dbc);
            PackageInfoViewModel packageInfoViewModel = packageFacade.getFullInfoForUser(HttpContext, id);
            if (packageInfoViewModel == null) return Redirect("/packages");

            return View(packageInfoViewModel);
        }

        [HttpGet]
        [Route("/package/chat/{id}")]
        public IActionResult Chat(int id)
        {
            ViewData["idMenuActive"] = 0;
            PackageFacade packageFacade = new PackageFacade(_dbc);

            UserPassingPackageViewModel userPassingPackageViewModel = packageFacade.getUserPassingPackageForUser(HttpContext, id);

            if (userPassingPackageViewModel == null) return Redirect("/packages");

            return View(userPassingPackageViewModel);
        }

        //[AllowAnonymous]
        public IActionResult Packages()
        {
            ViewData["idMenuActive"] = 3;
            /*
            PackageFacade packageFacade = new PackageFacade(_dbc);
            ListPackagePreviewViewModel listPackagePreviewViewModel = new ListPackagePreviewViewModel(
                packageFacade.listAllPrivatePreview(HttpContext)
            );
            */

            return View();
        }

        [HttpGet]
        [Route("/package/buy/{id}")]
        public IActionResult Buy(int id)
        {
            ViewData["idMenuActive"] = 0;

            PackageFacade packageFacade = new PackageFacade(_dbc);
            PackageBuyViewModel packageBuyViewModel = packageFacade.getInfoForBuying(id);
            if (packageBuyViewModel == null) return Redirect("/packages");

            return View(packageBuyViewModel);
        }
    }
}
