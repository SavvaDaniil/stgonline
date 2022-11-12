using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using STG.Data;
using STG.DTO;
using STG.DTO.Lesson;
using STG.Facade;
using STG.Entities;
using STG.Service;
using STG.ViewModels;
using STG.DTO.Teacher;
using Microsoft.AspNetCore.Authorization;
using STG.ViewModels.Lesson;

namespace STG.Controllers.api
{
    [Route("api/lesson")]
    [ApiController]
    public class ApiLessonController : ControllerBase
    {
        private ApplicationDbContext _dbc;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public ApiLessonController(IHttpContextAccessor httpContextAccessor, ApplicationDbContext dbc)
        {
            this._dbc = dbc;
            _httpContextAccessor = httpContextAccessor;
        }
        
        [Route("search")]
        public LessonsViewModel listAll([FromForm]LessonFilterDTO lessonFilterDTO)
        {
            LessonFacade lessonFacade = new LessonFacade(this._dbc);
            return new LessonsViewModel(
                lessonFacade.getAllActiveByFilter(
                    HttpContext,
                    lessonFilterDTO.id_of_style,
                    lessonFilterDTO.id_of_level,
                    lessonFilterDTO.id_of_teacher,
                    lessonFilterDTO.name,
                    lessonFilterDTO.skip,
                    lessonFilterDTO.isFree,
                    lessonFilterDTO.take
                )
            );
        }

        [Route("first_6")]
        public LessonsViewModel first6()
        {
            LessonFacade lessonFacade = new LessonFacade(this._dbc);
            return new LessonsViewModel(
                lessonFacade.getAllActiveByFilter(
                    HttpContext,
                    0,
                    0,
                    0,
                    null,
                    0,
                    0,
                    6
                )
            );
        }

        [Route("app/search")]
        [Authorize(AuthenticationSchemes = "UserJWT")]
        [AllowAnonymous]
        public LessonsViewModel appListAll([FromForm] LessonFilterDTO lessonFilterDTO)
        {
            LessonFacade lessonFacade = new LessonFacade(this._dbc);
            return new LessonsViewModel(
                lessonFacade.getAllActiveByFilter(
                    HttpContext,
                    lessonFilterDTO.id_of_style,
                    lessonFilterDTO.id_of_level,
                    lessonFilterDTO.id_of_teacher,
                    lessonFilterDTO.name,
                    lessonFilterDTO.skip,
                    lessonFilterDTO.isFree,
                    lessonFilterDTO.take
                )
            );
        }

        [Route("by_teacher")]
        public IActionResult listAll([FromForm] TeacherIdDTO teacherIdDTO)
        {
            LessonFacade lessonFacade = new LessonFacade(this._dbc);
            return Ok(
                new JsonAnswerStatus(
                    "success",
                    null,
                    lessonFacade.getAllActiveByFilter(
                        HttpContext,
                        0,
                        0,
                        teacherIdDTO.id,
                        null,
                        0,
                        0,
                        6
                    )
                )
            );
        }

        [Route("add")]
        [Authorize(AuthenticationSchemes = "AdminCookie")]
        public IActionResult add([FromForm]LessonNewDTO lessonNewDTO)
        {
            if (!ModelState.IsValid) return Ok(new JsonAnswerStatus("error", null));
            LessonService lessonService = new LessonService(this._dbc);
            Lesson lesson = lessonService.add(lessonNewDTO);
            return (lesson != null) ? Ok(new JsonAnswerStatus("success", null)) : Ok(new JsonAnswerStatus("error", null));
        }

        [Route("check_access")]
        public IActionResult checkAccess([FromForm] LessonIdDTO lessonIdDTO)
        {
            if (!ModelState.IsValid) return Ok(new JsonAnswerStatus("error", null));
            LessonFacade lessonFacade = new LessonFacade(_dbc);

            return Ok(lessonFacade.checkAccessForUser(HttpContext, _httpContextAccessor.HttpContext.Request.Host.Value, lessonIdDTO.id));
        }

        [Route("app/check_access")]
        [Authorize(AuthenticationSchemes = "UserJWT")]
        [AllowAnonymous]
        public IActionResult appCheckAccess([FromForm] LessonIdDTO lessonIdDTO)
        {
            if (!ModelState.IsValid) return Ok(new JsonAnswerStatus("error", null));
            LessonFacade lessonFacade = new LessonFacade(_dbc);

            return Ok(lessonFacade.checkAccessForUser(HttpContext, _httpContextAccessor.HttpContext.Request.Host.Value, lessonIdDTO.id));
        }


        [Route("app/buy")]
        [Authorize(AuthenticationSchemes = "UserJWT")]
        public IActionResult appBuy([FromForm] LessonIdDTO lessonIdDTO)
        {
            LessonFacade lessonFacade = new LessonFacade(_dbc);
            LessonBuyViewModel lessonBuyViewModel = lessonFacade.getInfoForBuying(HttpContext, lessonIdDTO.id);

            if (lessonBuyViewModel == null) return Ok(new JsonAnswerStatus("error", "unknown"));
            return Ok(new JsonAnswerStatus("success", null, lessonBuyViewModel));
        }

        [Route("update")]
        [Authorize(AuthenticationSchemes = "AdminCookie")]
        public IActionResult update([FromForm]LessonDTO lessonDTO)
        {
            if (!ModelState.IsValid) return Ok(new JsonAnswerStatus("error", null));

            LessonFacade lessonFacade = new LessonFacade(this._dbc);
            return Ok(lessonFacade.update(lessonDTO));
        }

        [Route("change_order")]
        [Authorize(AuthenticationSchemes = "AdminCookie")]
        public IActionResult changeOrder([FromForm] LessonOrderDTO lessonOrderDTO)
        {
            if (!ModelState.IsValid) return Ok(new JsonAnswerStatus("error", null));

            LessonFacade lessonFacade = new LessonFacade(this._dbc);
            return Ok(
                (lessonFacade.changeOrder(lessonOrderDTO)
                ? new JsonAnswerStatus("success", "unknown")
                : new JsonAnswerStatus("error", "unknown")
                )    
            );
        }

        [Route("delete")]
        [Authorize(AuthenticationSchemes = "AdminCookie")]
        public IActionResult delete([FromForm]LessonDTO lessonDTO)
        {
            if (!ModelState.IsValid) return Ok(new JsonAnswerStatus("error", null));

            LessonFacade lessonFacade = new LessonFacade(this._dbc);
            bool result = lessonFacade.delete(lessonDTO.id);
            lessonFacade = null;

            return Ok(new JsonAnswerStatus("success", null));
        }
    }
}