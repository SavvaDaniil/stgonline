using Microsoft.AspNetCore.Mvc;
using STG.Data;
using STG.Facade;
using STG.ViewModels.TeacherViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace STG.Controllers
{
    public class TeacherController : Controller
    {
        ApplicationDbContext _dbc;

        public TeacherController(ApplicationDbContext dbc)
        {
            this._dbc = dbc;
        }

        [HttpGet]
        [Route("/teacher/{id}")]
        public IActionResult Teacher(int id)
        {
            ViewData["idMenuActive"] = 0;
            TeacherFacade teacherFacade = new TeacherFacade(_dbc);
            TeacherLiteViewModel teacherLiteViewModel = teacherFacade.get(id);
            if (teacherLiteViewModel == null) return RedirectToAction("Teachers");
            teacherFacade = null;

            return View(teacherLiteViewModel);
        }

        [HttpGet]
        [Route("/teachers")]
        public IActionResult Teachers()
        {
            ViewData["idMenuActive"] = 4;
            TeacherFacade teacherFacade = new TeacherFacade(_dbc);

            ListTeacherLiteViewModels listTeacherLiteViewModels = new ListTeacherLiteViewModels(
                teacherFacade.listAllActive()
            );

            return View(listTeacherLiteViewModels);
        }
    }
}
