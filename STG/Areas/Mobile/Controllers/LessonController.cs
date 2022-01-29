using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using STG.Data;
using STG.DTO.Lesson;
using STG.Facade;
using STG.ViewModels;
using STG.ViewModels.Lesson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace STG.Areas.Mobile.Controllers
{
    [Area("Mobile")]
    [Route("mobile/lesson")]
    [ApiController]
    public class LessonController : ControllerBase
    {
        private ApplicationDbContext _dbc;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private IServiceScopeFactory _serviceScopeFactory;
        public LessonController(IHttpContextAccessor httpContextAccessor, ApplicationDbContext dbc, IServiceScopeFactory serviceScopeFactory)
        {
            this._dbc = dbc;
            _httpContextAccessor = httpContextAccessor;
            _serviceScopeFactory = serviceScopeFactory;
        }

        [Route("search")]
        public async Task<LessonsViewModel> search([FromForm] LessonFilterDTO lessonFilterDTO)
        {
            LessonFacade lessonFacade = new LessonFacade(this._dbc);
            return new LessonsViewModel(
                await lessonFacade.getAllActiveByFilter(
                    HttpContext,
                    lessonFilterDTO.style,
                    lessonFilterDTO.id_of_level,
                    lessonFilterDTO.id_of_teacher,
                    lessonFilterDTO.name,
                    lessonFilterDTO.skip,
                    lessonFilterDTO.isFree,
                    lessonFilterDTO.take
                )
            );
        }




        [Route("{id}")]
        public async Task<IActionResult> get(int id)
        {
            LessonFacade lessonFacade = new LessonFacade(_dbc, _serviceScopeFactory);
            LessonVideoViewModel lessonVideoViewModel = await lessonFacade.getWithVideo(id);

            return Ok(new JsonAnswerStatus("success", null, lessonVideoViewModel));
        }
    }
}
