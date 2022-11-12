using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using STG.Data;
using STG.DTO.Package;
using STG.Facade;
using STG.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace STG.Controllers.api
{
    [Route("api/packagelesson")]
    [Authorize(AuthenticationSchemes = "AdminCookie")]
    [ApiController]
    public class ApiPackageLessonController : ControllerBase
    {
        private ApplicationDbContext _dbc;
        public ApiPackageLessonController(ApplicationDbContext dbc)
        {
            this._dbc = dbc;
        }


        [Route("add")]
        public IActionResult add([FromForm] PackageLessonNewDTO packageLessonNewDTO)
        {
            if (!ModelState.IsValid) return Ok(new JsonAnswerStatus("error", "no_data"));

            PackageLessonFacade packageLessonFacade = new PackageLessonFacade(_dbc);

            return Ok(packageLessonFacade.add(packageLessonNewDTO)
                ? new JsonAnswerStatus("success", null)
                : new JsonAnswerStatus("error", "unknown")
            );
        }

        [Route("update")]
        public IActionResult update([FromForm] PackageLessonDTO packageLessonDTO)
        {
            if (!ModelState.IsValid) return Ok(new JsonAnswerStatus("error", "no_data"));

            PackageLessonFacade packageLessonFacade = new PackageLessonFacade(_dbc);

            return Ok(packageLessonFacade.update(packageLessonDTO)
                ? new JsonAnswerStatus("success", null)
                : new JsonAnswerStatus("error", "unknown")
            );
        }


        [Route("delete")]
        public IActionResult delete([FromForm] PackageLessonIdDTO packageLessonIdDTO)
        {
            if (!ModelState.IsValid) return Ok(new JsonAnswerStatus("error", "no_data"));

            PackageLessonFacade packageLessonFacade = new PackageLessonFacade(_dbc);

            return Ok(packageLessonFacade.delete(packageLessonIdDTO)
                ? new JsonAnswerStatus("success", null)
                : new JsonAnswerStatus("error", "unknown")
            );
        }

        /*
        
            public function actionAdd(){
                $request = Yii::$app->request;
                if(!(Yii::$app->request->isPost))return null;

                $programlessonNewDTO = new ProgramlessonNewDTO(
                    $id_of_program = SecurityComponent::check_values_150($request->post('id_of_program')),
                    $id_of_program_day = SecurityComponent::check_values_150($request->post('id_of_programday'))
                );
                if(!$programlessonNewDTO -> validate())return $this -> asJson(new JsonAnswerViewModel("error","no_data"));

                $programlessonService = new ProgramlessonService();
                return (
                    $programlessonService -> add($programlessonNewDTO)
                    ? $this -> asJson(new JsonAnswerViewModel("success", null))
                    : $this -> asJson(new JsonAnswerViewModel("error", "unknown"))
                );
            }

            public function actionGet(){
                $request = Yii::$app->request;
                if(!(Yii::$app->request->isPost))return null;

                $id_of_video = SecurityComponent::check_values_150($request->post('id_of_video'));
                if($id_of_video == null)return $this -> asJson(new JsonAnswerViewModel("error","no_data"));

                $programlessonFacade = new ProgramlessonFacade();

                $jsonAnswerViewModel = new JsonAnswerViewModel("success", null);
                $jsonAnswerViewModel -> ProgramlessonList = $programlessonFacade -> listAllByIdOfVideo($id_of_video);
                $programlessonFacade = null;

                return $this -> asJson($jsonAnswerViewModel);
            }

            public function actionUpdate(){
                $request = Yii::$app->request;
                if(!(Yii::$app->request->isPost))return null;

                $programlessonDTO = new ProgramlessonDTO(
                    SecurityComponent::check_values_150($request->post('id')),
                    SecurityComponent::check_values_150($request->post('id_of_program')),
                    SecurityComponent::check_values_150($request->post('id_of_program_day')),
                    SecurityComponent::check_values_150($request->post('id_of_lesson')),
                    SecurityComponent::check_values_150($request->post('homework_status')),
                    SecurityComponent::check_values_150($request->post('homework_text'))
                );

                if(!$programlessonDTO -> validate())return $this -> asJson(new JsonAnswerViewModel("error","no_data"));

                $programlessonFacade = new ProgramlessonFacade();

                return $this -> asJson(
                    $programlessonFacade -> update($programlessonDTO)
                    ? new JsonAnswerViewModel("success", null)
                    : new JsonAnswerViewModel("error", "unknown")
                );
            }



            public function actionDelete(){
                $request = Yii::$app->request;
                if(!(Yii::$app->request->isPost))return null;

                $id_of_program_lesson = SecurityComponent::check_values_150($request->post('id_of_program_lesson'));
                if($id_of_program_lesson == null)return $this -> asJson(new JsonAnswerViewModel("error","no_data"));

                $programlessonService = new ProgramlessonService();
                return (
                    $programlessonService -> delete($id_of_program_lesson)
                    ? $this -> asJson(new JsonAnswerViewModel("success", null))
                    : $this -> asJson(new JsonAnswerViewModel("error", "unknown"))
                );
            }
        */
    }
}
