using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using STG.Component;
using STG.Data;
using STG.DTO.Homework;
using STG.DTO.Package;
using STG.Facade;
using STG.ViewModels;

namespace STG.Controllers.api
{
    [Route("api/homework")]
    [ApiController]
    public class ApiHomeworkСontroller : ControllerBase
    {
        ApplicationDbContext _dbc;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public ApiHomeworkСontroller(IHttpContextAccessor httpContextAccessor, ApplicationDbContext dbc)
        {
            this._dbc = dbc;
            _httpContextAccessor = httpContextAccessor;
        }


        [HttpPost]
        [Authorize(AuthenticationSchemes = "UserCookie")]
        //[RequestSizeLimit(100_000_000)]
        [DisableRequestSizeLimit, RequestFormLimits(MultipartBodyLengthLimit = int.MaxValue,
        ValueLengthLimit = int.MaxValue)]
        //[RequestFormLimits(MultipartBodyLengthLimit = 2147483648)]
        [Route("add")]
        public async Task<IActionResult> add([FromForm] HomeworkNewFromUserDTO homeworkNewDTO)
        {
            try
            {
                if (!ModelState.IsValid) return Ok(new JsonAnswerStatus("error", "no_data"));

                HomeworkFacade homeworkFacade = new HomeworkFacade(_dbc);

                return Ok(
                    (await homeworkFacade.newHomework(HttpContext, homeworkNewDTO) != null)
                    ? new JsonAnswerStatus("success", null)
                    : new JsonAnswerStatus("error", "unknown")
                );
            } catch(Exception ex)
            {
                LoggerComponent.writeToLogError("Ошибка newHomework: " + ex.Message.ToString());
            }
            return Ok(new JsonAnswerStatus("error", "unknown"));
        }


        [HttpPost]
        [Authorize(AuthenticationSchemes = "UserCookie")]
        [DisableRequestSizeLimit, RequestFormLimits(MultipartBodyLengthLimit = int.MaxValue, ValueLengthLimit = int.MaxValue)]
        [Route("add_by_chunk")]
        public IActionResult addByChunk([FromForm] HomeworkNewFromUserByChunkDTO homeworkNewFromUserByChunkDTO)
        {
            if (!ModelState.IsValid) return Ok(new JsonAnswerStatus("error", "no_data"));

            HomeworkFacade homeworkFacade = new HomeworkFacade(_dbc);

            return Ok(homeworkFacade.newHomeworkByChunk(HttpContext, homeworkNewFromUserByChunkDTO));
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = "UserCookie")]
        [DisableRequestSizeLimit, RequestFormLimits(MultipartBodyLengthLimit = int.MaxValue, ValueLengthLimit = int.MaxValue)]
        [Route("add_only_comment")]
        public IActionResult addOnlyComment([FromForm] HomeworkNewFromUserDTO homeworkNewFromUserDTO)
        {
            if (!ModelState.IsValid) return Ok(new JsonAnswerStatus("error", "no_data"));

            HomeworkFacade homeworkFacade = new HomeworkFacade(_dbc);

            return Ok(homeworkFacade.newHomeworkOnlyComment(HttpContext, homeworkNewFromUserDTO));
        }


        [HttpPost]
        [Authorize(AuthenticationSchemes = "UserCookie")]
        [Route("set_seen_by_user")]
        public IActionResult setSeenByUser([FromForm] HomeworkIdDTO homeworkIdDTO)
        {
            if (!ModelState.IsValid) return Ok(new JsonAnswerStatus("error", "no_data"));

            HomeworkFacade homeworkFacade = new HomeworkFacade(_dbc);

            return Ok(
                homeworkFacade.setSeenByUser(HttpContext, homeworkIdDTO)
                ? new JsonAnswerStatus("success", null)
                : new JsonAnswerStatus("error", "unknown")
            );
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = "UserCookie")]
        [Route("get")]
        public IActionResult get([FromForm] PackageLessonIdDTO packageLessonIdDTO)
        {
            if (!ModelState.IsValid) return Ok(new JsonAnswerStatus("error", "no_data"));

            HomeworkFacade homeworkFacade = new HomeworkFacade(_dbc);

            return Ok(
                homeworkFacade.get(HttpContext, packageLessonIdDTO.id)
            );
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = "AdminCookie")]
        [Route("set_seen_by_admin")]
        public IActionResult setSeenByAdmin([FromForm] HomeworkIdDTO homeworkIdDTO)
        {
            if (!ModelState.IsValid) return Ok(new JsonAnswerStatus("error", "no_data"));

            HomeworkFacade homeworkFacade = new HomeworkFacade(_dbc);

            return Ok(
                homeworkFacade.setSeenByAdmin(HttpContext, homeworkIdDTO)
                ? new JsonAnswerStatus("success", null)
                : new JsonAnswerStatus("error", "unknown")
            );
        }


        [HttpPost]
        [Authorize(AuthenticationSchemes = "AdminCookie")]
        [Route("answer")]
        public IActionResult answerFromAdmin([FromForm] HomeworkAnswerFromAdminDTO homeworkAnswerFromAdminDTO)
        {
            if (!ModelState.IsValid) return Ok(new JsonAnswerStatus("error", "no_data"));

            HomeworkFacade homeworkFacade = new HomeworkFacade(_dbc);

            return Ok(homeworkFacade.newAnswerFromAdmin(HttpContext, homeworkAnswerFromAdminDTO));
        }
    }
}
