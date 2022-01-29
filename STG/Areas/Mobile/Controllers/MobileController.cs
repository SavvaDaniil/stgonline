using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using STG.Data;
using STG.DTO.Lesson;
using STG.Facade;
using STG.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace STG.Areas.Mobile.Controllers
{
    [Area("Mobile")]
    [Route("mobile")]
    [ApiController]
    public class MobileController : ControllerBase
    {
        private ApplicationDbContext _dbc;
        public MobileController(ApplicationDbContext dbc)
        {
            _dbc = dbc;
        }

        [Route("lessons")]
        public async Task<LessonsViewModel> lessons([FromForm] LessonFilterDTO lessonFilterDTO)
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

    }
}
