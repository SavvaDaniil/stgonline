using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using STG.Data;
using STG.DTO;
using STG.DTO.LessonType;
using STG.Entities;
using STG.Service;
using STG.ViewModels;

namespace STG.Controllers.api
{
    [Route("api/lessontype")]
    [ApiController]
    public class ApiLessonTypeController : ControllerBase
    {

        private ApplicationDbContext _dbc;
        public ApiLessonTypeController(ApplicationDbContext dbc)
        {
            this._dbc = dbc;
        }

        [Route("add")]
        public IActionResult add([FromForm]LessonTypeNewDTO lessonTypeNewDTO)
        {
            if (!ModelState.IsValid) return Ok(new JsonAnswerStatus("error", null));
            LessonTypeService lessonTypeService = new LessonTypeService(this._dbc);
            LessonType lessonType = lessonTypeService.add(lessonTypeNewDTO);
            return (lessonType != null) ? Ok(new JsonAnswerStatus("success", null)) : Ok(new JsonAnswerStatus("error", null));
        }

        [Route("delete")]
        public IActionResult delete([FromForm]LessonTypeDTO lessonTypeDTO)
        {
            if (!ModelState.IsValid) return Ok(new JsonAnswerStatus("error", null));
            LessonTypeService lessonTypeService = new LessonTypeService(this._dbc);
            bool result = lessonTypeService.delete(lessonTypeDTO.id);
            return Ok(new JsonAnswerStatus("success", null));
        }

        [Route("get")]
        public IActionResult get([FromForm]LessonTypeDTO lessonTypeDTO)
        {
            if (!ModelState.IsValid) return Ok(new JsonAnswerStatus("error", null));
            LessonTypeService lessonTypeService = new LessonTypeService(this._dbc);
            LessonType lessonType = lessonTypeService.findById(lessonTypeDTO.id);
            if (lessonType == null) return Ok(new JsonAnswerStatus("error", "not_found"));

            JsonAnswerStatus jsonAnswerStatus = (lessonType != null
                ? new JsonAnswerStatus("success", null, new LessonTypeLiteViewModel(
                    lessonType.id,
                    lessonType.name,
                    lessonType.active
                    ))
                : new JsonAnswerStatus("success", null)
                );
            lessonTypeService = null;

            return Ok(jsonAnswerStatus);
        }

        [Route("update")]
        public IActionResult update([FromForm]LessonTypeDTO lessonTypeDTO)
        {
            if (!ModelState.IsValid) return Ok(new JsonAnswerStatus("error", null));
            LessonTypeService lessonTypeService = new LessonTypeService(this._dbc);

            return Ok(lessonTypeService.update(lessonTypeDTO)
                ? new JsonAnswerStatus("success", null)
                : new JsonAnswerStatus("error", "unknown"));
        }
    }
}