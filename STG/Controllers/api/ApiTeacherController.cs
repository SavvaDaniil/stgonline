using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using STG.Data;
using STG.DTO.Teacher;
using STG.Facade;
using STG.Entities;
using STG.Service;
using STG.ViewModels;
using Microsoft.AspNetCore.Authorization;

namespace STG.Controllers.api
{
    [Route("api/teacher")]
    [ApiController]
    public class ApiTeacherController : ControllerBase
    {
        private ApplicationDbContext _dbc;
        public ApiTeacherController(ApplicationDbContext dbc)
        {
            this._dbc = dbc;
        }

        [Route("add")]
        [Authorize(AuthenticationSchemes = "AdminCookie")]
        public IActionResult add([FromForm]TeacherNewDTO teacherNewDTO)
        {
            if(!ModelState.IsValid)return Ok(new JsonAnswerStatus("error", null));
            TeacherService teacherService = new TeacherService(this._dbc);
            Teacher teacher = teacherService.add(teacherNewDTO);
            return (teacher != null) ? Ok(new JsonAnswerStatus("success", null)) : Ok(new JsonAnswerStatus("error", null));
        }

        [Route("get_modal")]
        public IActionResult getModal([FromForm] TeacherIdDTO teacherIdDTO)
        {
            if (!ModelState.IsValid) return Ok(new JsonAnswerStatus("error", null));
            TeacherFacade teacherFacade = new TeacherFacade(_dbc);

            return Ok(teacherFacade.getModal(HttpContext, teacherIdDTO));
        }

        [Route("all")]
        public IActionResult all()
        {
            TeacherFacade teacherFacade = new TeacherFacade(_dbc);
            return Ok(
                new JsonAnswerStatus(
                    "success",
                    null,
                    teacherFacade.listAllActive()
                )    
            );
        }
        [Route("all_curators")]
        public IActionResult allCurator()
        {
            TeacherFacade teacherFacade = new TeacherFacade(_dbc);
            return Ok(
                new JsonAnswerStatus(
                    "success",
                    null,
                    teacherFacade.listAllCuratorForMentoring()
                )
            );
        }

        [Route("app/all_curators")]
        public IActionResult appAllCurator()
        {
            TeacherFacade teacherFacade = new TeacherFacade(_dbc);
            return Ok(
                new JsonAnswerStatus(
                    "success",
                    null,
                    teacherFacade.listAllCurator()
                )
            );
        }

        [Route("update")]
        [Authorize(AuthenticationSchemes = "AdminCookie")]
        public IActionResult update([FromForm]TeacherDTO teacherDTO)
        {
            if (!ModelState.IsValid) return Ok(new JsonAnswerStatus("error", null));
            TeacherFacade teacherFacade = new TeacherFacade(_dbc);

            return Ok(teacherFacade.update(teacherDTO));
        }

        [Route("change_order")]
        [Authorize(AuthenticationSchemes = "AdminCookie")]
        public IActionResult changeOrder([FromForm] TeacherOrderDTO teacherOrderDTO)
        {
            if (!ModelState.IsValid) return Ok(new JsonAnswerStatus("error", null));

            TeacherFacade teacherFacade = new TeacherFacade(this._dbc);
            return Ok(
                (teacherFacade.changeOrder(teacherOrderDTO)
                ? new JsonAnswerStatus("success", "unknown")
                : new JsonAnswerStatus("error", "unknown")
                )
            );
        }

        [Route("delete")]
        [Authorize(AuthenticationSchemes = "AdminCookie")]
        public IActionResult delete([FromForm]TeacherDTO teacherDTO)
        {
            if (!ModelState.IsValid) return Ok(new JsonAnswerStatus("error", null));
            TeacherService teacherService = new TeacherService(this._dbc);
            bool result = teacherService.delete(teacherDTO.id);
            return Ok(new JsonAnswerStatus("success", null));
        }
    }
}