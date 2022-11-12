using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using STG.Data;
using STG.DTO.Video;
using STG.Facade;
using STG.Service;
using STG.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace STG.Controllers.api
{
    [Route("api/videosection")]
    [Authorize(AuthenticationSchemes = "AdminCookie")]
    [ApiController]
    public class ApiVideoSectionController : ControllerBase
    {
        private ApplicationDbContext _dbc;
        public ApiVideoSectionController(ApplicationDbContext dbc)
        {
            this._dbc = dbc;
        }

        [Route("add")]
        public IActionResult add([FromForm] VideoSectionNewDTO videoSectionNewDTO)
        {
            if (!ModelState.IsValid) return Ok(new JsonAnswerStatus("error", "no_data"));

            VideoSectionFacade videoSectionFacade = new VideoSectionFacade(_dbc);

            return Ok(videoSectionFacade.add(videoSectionNewDTO)
                ? new JsonAnswerStatus("success", null)
                : new JsonAnswerStatus("error", "unknown")
            );
        }

        [Route("get")]
        public IActionResult get([FromForm] VideoIdDTO videoIdDTO)
        {
            if (!ModelState.IsValid) return Ok(new JsonAnswerStatus("error", "no_data"));

            VideoSectionFacade videoSectionFacade = new VideoSectionFacade(_dbc);

            return Ok(
                new JsonAnswerStatus(
                    "success", null,
                    videoSectionFacade.listAllByIdOfVideo(videoIdDTO.id_of_video)
                )    
            );
        }

        [Route("update")]
        public IActionResult update([FromForm] VideoSectionDTO videoSectionDTO)
        {
            if (!ModelState.IsValid) return Ok(new JsonAnswerStatus("error", "no_data"));
            VideoSectionFacade videoSectionFacade = new VideoSectionFacade(_dbc);

            return Ok(videoSectionFacade.update(videoSectionDTO)
                ? new JsonAnswerStatus("success", null)
                : new JsonAnswerStatus("error", "unknown")
            );
        }

        [Route("delete")]
        public IActionResult delete([FromForm] VideoSectionIdDTO videoSectionIdDTO)
        {
            if (!ModelState.IsValid) return Ok(new JsonAnswerStatus("error", "no_data"));
            VideoSectionFacade videoSectionFacade = new VideoSectionFacade(_dbc);

            return Ok(videoSectionFacade.delete(videoSectionIdDTO)
                ? new JsonAnswerStatus("success", null)
                : new JsonAnswerStatus("error", "unknown")
            );
        }

    }
}
