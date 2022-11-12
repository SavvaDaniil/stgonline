using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using STG.Data;
using STG.DTO.Video;
using STG.Facade;
using STG.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace STG.Controllers.api
{
    [Route("api/videosubsection")]
    [Authorize(AuthenticationSchemes = "AdminCookie")]
    [ApiController]
    public class ApiVideoSubsectionController : ControllerBase
    {
        private ApplicationDbContext _dbc;
        public ApiVideoSubsectionController(ApplicationDbContext dbc)
        {
            this._dbc = dbc;
        }


        [Route("add")]
        public IActionResult add([FromForm] VideosubsectionNewDTO videosubsectionNewDTO)
        {
            if (!ModelState.IsValid) return Ok(new JsonAnswerStatus("error", "no_data"));

            VideoSubsectionFacade videoSubsectionFacade = new VideoSubsectionFacade(_dbc);

            return Ok(videoSubsectionFacade.add(videosubsectionNewDTO)
                ? new JsonAnswerStatus("success", null)
                : new JsonAnswerStatus("error", "unknown")
            );
        }

        [Route("update")]
        public IActionResult update([FromForm] VideoSubsectionDTO videoSubsectionDTO)
        {
            if (!ModelState.IsValid) return Ok(new JsonAnswerStatus("error", "no_data"));

            VideoSubsectionFacade videoSubsectionFacade = new VideoSubsectionFacade(_dbc);

            return Ok(videoSubsectionFacade.update(videoSubsectionDTO)
                ? new JsonAnswerStatus("success", null)
                : new JsonAnswerStatus("error", "unknown")
            );
        }


        [Route("delete")]
        public IActionResult delete([FromForm] VideoSubsectionIdDTO videoSubsectionIdDTO)
        {
            if (!ModelState.IsValid) return Ok(new JsonAnswerStatus("error", "no_data"));

            VideoSubsectionFacade videoSubsectionFacade = new VideoSubsectionFacade(_dbc);

            return Ok(videoSubsectionFacade.delete(videoSubsectionIdDTO)
                ? new JsonAnswerStatus("success", null)
                : new JsonAnswerStatus("error", "unknown")
            );
        }

        /*


            public function actionDelete(){
                $request = Yii::$app->request;
                if(!(Yii::$app->request->isPost))return null;

                $id_of_videosubsection = SecurityComponent::check_values_150($request->post('id_of_videosubsection'));
                if($id_of_videosubsection == null)return $this -> asJson(new JsonAnswerViewModel("error","no_data"));

                $videosubsectionService = new VideosubsectionService();
                return (
                    $videosubsectionService -> delete($id_of_videosubsection)
                    ? $this -> asJson(new JsonAnswerViewModel("success", null))
                    : $this -> asJson(new JsonAnswerViewModel("error", "unknown"))
                );
            }
        */
    }
}
