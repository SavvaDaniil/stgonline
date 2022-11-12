using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using STG.Data;
using STG.DTO.ObserverLessonUser;
using STG.Facade;
using STG.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace STG.Controllers.api
{
    [Route("api/observer_lesson_user")]
    [ApiController]
    public class ApiObserverLessonUserController : ControllerBase
    {
        ApplicationDbContext _dbc;
        public ApiObserverLessonUserController(ApplicationDbContext dbc)
        {
            this._dbc = dbc;
        }


        [Route("update")]
        public IActionResult update([FromForm] ObserverLessonUserDTO observerLessonUserDTO)
        {
            if (!ModelState.IsValid) return Ok(new JsonAnswerStatus("error", "no_data"));
            ObserverLessonUserFacade observerLessonUserFacade = new ObserverLessonUserFacade(this._dbc);

            return Ok(
                observerLessonUserFacade.update(HttpContext, observerLessonUserDTO) != null
                ? new JsonAnswerStatus("success", null)
                : new JsonAnswerStatus("error", "unknown")
            );
        }

        [Route("delete")]
        public IActionResult delete([FromForm] int id_of_observer_lesson_user)
        {
            ObserverLessonUserFacade observerLessonUserFacade = new ObserverLessonUserFacade(this._dbc);
            return Ok(
                observerLessonUserFacade.deleteById(id_of_observer_lesson_user)
                ? new JsonAnswerStatus("success", null)
                : new JsonAnswerStatus("error", "unknown")
            );
        }

    }
}
