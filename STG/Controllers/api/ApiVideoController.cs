using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using STG.Data;
using STG.DTO;
using STG.DTO.Video;
using STG.Facade;
using STG.Entities;
using STG.Service;
using STG.ViewModels;

namespace STG.Controllers.api
{
    [Route("api/video")]
    [ApiController]
    public class ApiVideoController : ControllerBase
    {
        private ApplicationDbContext _dbc;
        public ApiVideoController(ApplicationDbContext dbc)
        {
            this._dbc = dbc;
        }

        [Route("add")]
        public IActionResult add([FromForm]VideoNewDTO videoNewDTO)
        {
            if (!ModelState.IsValid) return Ok(new JsonAnswerStatus("error", null));
            VideoService videoService = new VideoService(this._dbc);
            Video video = videoService.add(videoNewDTO);
            videoService = null;
            return (video != null) ? Ok(new JsonAnswerStatus("success", null)) : Ok(new JsonAnswerStatus("error", null));
        }

        [Route("update")]
        public async Task<IActionResult> update([FromForm]VideoDTO videoDTO)
        {
            if (!ModelState.IsValid) return Ok(new JsonAnswerStatus("error", null));
            VideoFacade videoFacade = new VideoFacade(_dbc);

            return Ok(await videoFacade.update(videoDTO));
        }

        [Route("upload")]
        //[RequestSizeLimit(10_000_000_000)]
        public async Task<IActionResult> upload([FromForm]VideoFileDTO videoFileDTO)
        {
            if (!ModelState.IsValid) return Ok(new JsonAnswerStatus("error", null));
            VideoFacade videoFacade = new VideoFacade(_dbc);

            //if (videoFileDTO.dzchunkbyteoffset != null) videoFileDTO.offset = videoFileDTO.dzchunkbyteoffset;

            return Ok(await videoFacade.uploadVideoFile(videoFileDTO));
            //return Ok(new JsonAnswerStatus("success", null));
        }

        [Route("upload_dropzone")]
        public async Task<IActionResult> uploadDropzone([FromForm]VideoFileDropzoneDTO videoFileDropzoneDTO)
        {
            if (!ModelState.IsValid) return Ok(new JsonAnswerStatus("error", null));
            VideoFacade videoFacade = new VideoFacade(_dbc);
            JsonAnswerStatus jsonAnswerStatus = await videoFacade.uploadVideoFileDropzone(videoFileDropzoneDTO);
            videoFacade = null;

            if(jsonAnswerStatus.status == "error")
            {
                return StatusCode(500);
            }

            return Ok(jsonAnswerStatus);
        }
        

        [Route("delete")]
        public IActionResult delete([FromForm]VideoDTO videoDTO)
        {
            if (!ModelState.IsValid) return Ok(new JsonAnswerStatus("error", null));

            VideoFacade videoFacade = new VideoFacade(this._dbc);

            return Ok(videoFacade.delete(videoDTO));
        }


    }
}