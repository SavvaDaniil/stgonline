using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using STG.Data;
using STG.DTO.Package;
using STG.Facade;
using STG.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace STG.Controllers.api
{
    [Route("api/packageday")]
    [ApiController]
    public class ApiPackageDayController : ControllerBase
    {
        private ApplicationDbContext _dbc;
        public ApiPackageDayController(ApplicationDbContext dbc)
        {
            this._dbc = dbc;
        }


        [Route("add")]
        [Authorize(AuthenticationSchemes = "AdminCookie")]
        public IActionResult add([FromForm] PackageDayNewDTO PackageDayNewDTO)
        {
            if (!ModelState.IsValid) return Ok(new JsonAnswerStatus("error", "no_data"));

            PackageDayFacade packageDayFacade = new PackageDayFacade(_dbc);

            return Ok(packageDayFacade.add(PackageDayNewDTO)
                ? new JsonAnswerStatus("success", null)
                : new JsonAnswerStatus("error", "unknown")
            );
        }

        [Route("all")]
        public IActionResult all([FromForm] PackageIdDTO packageIdDTO)
        {
            if (!ModelState.IsValid) return Ok(new JsonAnswerStatus("error", "no_data"));

            PackageDayFacade packageDayFacade = new PackageDayFacade(_dbc);

            JsonAnswerStatus jsonAnswerStatus = new JsonAnswerStatus("success", null);
            jsonAnswerStatus.package_day_list = packageDayFacade.listAllByIdOfPackage(null, packageIdDTO);

            LessonFacade lessonFacade = new LessonFacade(_dbc);
            jsonAnswerStatus.lessonList = lessonFacade.listAllByMicro();

            return Ok(
                jsonAnswerStatus
            );
        }

        [Route("update")]
        [Authorize(AuthenticationSchemes = "AdminCookie")]
        public IActionResult update([FromForm] PackageDayDTO packageDayDTO)
        {
            if (!ModelState.IsValid) return Ok(new JsonAnswerStatus("error", "no_data"));
            PackageDayFacade packageDayFacade = new PackageDayFacade(_dbc);

            return Ok(packageDayFacade.update(packageDayDTO)
                ? new JsonAnswerStatus("success", null)
                : new JsonAnswerStatus("error", "unknown")
            );
        }

        [Route("delete")]
        [Authorize(AuthenticationSchemes = "AdminCookie")]
        public IActionResult delete([FromForm] PackageDayIdDTO packageDayIdDTO)
        {
            if (!ModelState.IsValid) return Ok(new JsonAnswerStatus("error", "no_data"));
            PackageDayFacade packageDayFacade = new PackageDayFacade(_dbc);

            return Ok(packageDayFacade.delete(packageDayIdDTO)
                ? new JsonAnswerStatus("success", null)
                : new JsonAnswerStatus("error", "unknown")
            );
        }

    }
}
