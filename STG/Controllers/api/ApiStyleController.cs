using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using STG.Data;
using STG.DTO;
using STG.DTO.Style;
using STG.Entities;
using STG.Service;
using STG.ViewModels;

namespace STG.Controllers.api
{
    [Route("api/style")]
    [ApiController]
    public class ApiStyleController : ControllerBase
    {
        private ApplicationDbContext _dbc;
        public ApiStyleController(ApplicationDbContext dbc)
        {
            this._dbc = dbc;
        }

        [Route("add")]
        public IActionResult add([FromForm]StyleNewDTO styleNewDTO)
        {
            if (!ModelState.IsValid) return Ok(new JsonAnswerStatus("error", null));
            StyleService styleService = new StyleService(this._dbc);
            Style style = styleService.add(styleNewDTO);
            return (style != null) ? Ok(new JsonAnswerStatus("success", null)) : Ok(new JsonAnswerStatus("error", null));
        }

        [Route("delete")]
        public IActionResult delete([FromForm]StyleDTO styleDTO)
        {
            if (!ModelState.IsValid) return Ok(new JsonAnswerStatus("error", null));
            StyleService styleService = new StyleService(this._dbc);
            bool result = styleService.delete(styleDTO.id);
            return Ok(new JsonAnswerStatus("success", null));
        }

        [Route("get")]
        public IActionResult get([FromForm]StyleDTO styleDTO)
        {
            if (!ModelState.IsValid) return Ok(new JsonAnswerStatus("error", null));
            StyleService styleService = new StyleService(this._dbc);
            Style style = styleService.findById(styleDTO.id);
            if (style == null) return Ok(new JsonAnswerStatus("error","not_found"));

            JsonAnswerStatus jsonAnswerStatus = (style != null
                ? new JsonAnswerStatus("success", null, new StyleLiteViewModel(
                    style.id,
                    style.name,
                    null,
                    style.active
                    ))
                : new JsonAnswerStatus("success", null)
                );
            styleService = null;

            return Ok(jsonAnswerStatus);
        }

        [Route("update")]
        public IActionResult update([FromForm]StyleDTO styleDTO)
        {
            if (!ModelState.IsValid) return Ok(new JsonAnswerStatus("error", null));
            StyleService styleService = new StyleService(this._dbc);

            return Ok(styleService.update(styleDTO)
                ? new JsonAnswerStatus("success", null)
                : new JsonAnswerStatus("error", "unknown"));
        }

    }
}