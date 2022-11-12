using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using STG.Data;
using STG.DTO.ConnectionUserToPrivatePackage;
using STG.DTO.Package;
using STG.DTO.PurchasePackage;
using STG.Facade;
using STG.ViewModels;
using STG.ViewModels.Package;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace STG.Controllers.api
{
    [Route("api/package")]
    [ApiController]
    public class ApiPackageController : ControllerBase
    {
        private ApplicationDbContext _dbc;
        public ApiPackageController(ApplicationDbContext dbc)
        {
            _dbc = dbc;
        }

        [Route("add")]
        [Authorize(AuthenticationSchemes = "AdminCookie")]
        public IActionResult add([FromForm] PackageNewDTO packageNewDTO)
        {
            if (!ModelState.IsValid) return Ok(new JsonAnswerStatus("error", null));

            System.Diagnostics.Debug.WriteLine("ApiPackageController packageNewDTO: " + packageNewDTO.name);

            PackageFacade packageFacade = new PackageFacade(_dbc);
            return Ok(packageFacade.add(packageNewDTO)
                ? new JsonAnswerStatus("success", null)
                : new JsonAnswerStatus("error", "unknown")
            );
        }


        [Route("update")]
        [Authorize(AuthenticationSchemes = "AdminCookie")]
        public IActionResult update([FromForm] PackageDTO packageDTO)
        {
            if (!ModelState.IsValid) return Ok(new JsonAnswerStatus("error", null));

            PackageFacade packageFacade = new PackageFacade(_dbc);

            return Ok(packageFacade.update(packageDTO));
        }


        [Route("delete")]
        [Authorize(AuthenticationSchemes = "AdminCookie")]
        public IActionResult delete([FromForm] PackageIdDTO packageIdDTO)
        {
            if (!ModelState.IsValid) return Ok(new JsonAnswerStatus("error", null));

            PackageFacade packageFacade = new PackageFacade(_dbc);

            return Ok(packageFacade.delete(packageIdDTO));
        }


        [HttpPost]
        [Route("user_passing_package")]
        public IActionResult getUserPassingPackage([FromForm] ConnectionUserToPackagePrivateIdDTO connectionUserToPackagePrivateIdDTO)
        {
            if (!ModelState.IsValid) return Ok(new JsonAnswerStatus("error", null));

            PackageFacade packageFacade = new PackageFacade(_dbc);

            return Ok(new JsonAnswerStatus("success", null, packageFacade.getUserPassingPackageForAdmin(connectionUserToPackagePrivateIdDTO)));
        }

        [HttpPost]
        [Route("user_passing_package_public")]
        public IActionResult getUserPassingPackagePublic([FromForm] PackageUserPassingDTO packageUserPassingDTO)
        {
            if (!ModelState.IsValid) return Ok(new JsonAnswerStatus("error", null));

            PackageFacade packageFacade = new PackageFacade(_dbc);

            return Ok(new JsonAnswerStatus(
                "success", 
                null, 
                packageFacade.getUserPassingPackagePublicForAdmin(packageUserPassingDTO.packageId, packageUserPassingDTO.userId))
            );
        }

        [HttpPost]
        [Route("search")]
        public IActionResult search([FromForm] PackageSearchDTO packageSearchDTO)
        {
            PackageFacade packageFacade = new PackageFacade(_dbc);
            return Ok(new JsonAnswerStatus("success", null, 
                packageFacade.listAllPreview(HttpContext, packageSearchDTO.active, packageSearchDTO.skip, packageSearchDTO.take)));
        }

        [HttpPost]
        [Route("first_public_6")]
        public IActionResult firstPublic6()
        {
            PackageFacade packageFacade = new PackageFacade(_dbc);
            return Ok(new JsonAnswerStatus("success", null, packageFacade.listAllPreview(HttpContext, 1, 0, 6)));
        }


        [HttpPost]
        [Route("app/search")]
        [Authorize(AuthenticationSchemes = "UserJWT")]
        public IActionResult appSearch([FromForm] PackageSearchDTO packageSearchDTO)
        {
            PackageFacade packageFacade = new PackageFacade(_dbc);
            List<PackagePreviewViewModel> packagePreviews = packageFacade.listAllPreview(HttpContext, 2);
            return Ok(new JsonAnswerStatus("success", null, packagePreviews));
        }

        [HttpPost]
        [Route("app/get")]
        [Authorize(AuthenticationSchemes = "UserJWT")]
        public IActionResult appGet([FromForm] PackageIdDTO packageIdDTO)
        {
            if (!ModelState.IsValid) return Ok(new JsonAnswerStatus("error", null));
            PackageFacade packageFacade = new PackageFacade(_dbc);
            PackageInfoViewModel packageInfo = packageFacade.getFullInfoForUser(HttpContext, packageIdDTO.id_of_package);
            return Ok(new JsonAnswerStatus("success", null, packageInfo));
        }

        [HttpPost]
        [Route("app/buy")]
        [Authorize(AuthenticationSchemes = "UserJWT")]
        public IActionResult appBuy([FromForm] PackageIdDTO packageIdDTO)
        {
            if (!ModelState.IsValid) return Ok(new JsonAnswerStatus("error", null));
            PackageFacade packageFacade = new PackageFacade(_dbc);
            PackageBuyViewModel packageBuy = packageFacade.getInfoForBuying(packageIdDTO.id_of_package);
            return Ok(new JsonAnswerStatus("success", null, packageBuy));
        }


        /*
        
            public function actionAdd(){
                $request = Yii::$app->request;
                if(!(Yii::$app->request->isPost))return null;

                $programService = new ProgramService();
                $programNewDTO = new ProgramNewDTO();

                $name = SecurityComponent::check_values_150($request->post('name'));
                if($name == null) return $this -> asJson(new JsonAnswerViewModel("error", "no_data"));

                $programNewDTO -> name = SecurityComponent::check_values_150($request->post('name'));


                return $this -> asJson(
                    $programService -> add($programNewDTO)
                    ? new JsonAnswerViewModel("success", null)
                    : new JsonAnswerViewModel("error", null)
                );
            }
            public function actionDelete(){
                $request = Yii::$app->request;
                if(!(Yii::$app->request->isPost))return null;

                $programFacade = new ProgramFacade();

                $id = SecurityComponent::check_values_150($request->post('id'));
                if($id == null) return $this -> asJson(new JsonAnswerViewModel("error", "no_data"));

                return $this -> asJson(
                    $programFacade -> delete($id)
                    ? new JsonAnswerViewModel("success", null)
                    : new JsonAnswerViewModel("error", null)
                );
            }
    
            public function actionUpdate(){
                $request = Yii::$app->request;
                if(!(Yii::$app->request->isPost))return null;

                $programPosterFileDTO = new ProgramPosterFileDTO();
                $programPosterFileDTO -> imageFile = UploadedFile::getInstance($programPosterFileDTO, 'imageFile');

                $programDTO = new ProgramDTO(
                    SecurityComponent::check_values_150($request->post('id')),
                    SecurityComponent::check_values_150($request->post('name')),
                    SecurityComponent::check_values_150($request->post('price')),
                    SecurityComponent::check_values_150($request->post('days')),
                    SecurityComponent::check_values_150($request->post('active')),
                    SecurityComponent::check_values_long($request->post('description')),
                    SecurityComponent::check_values_150($request->post('id_of_level')),
                    SecurityComponent::check_values_150($request->post('id_of_style')),
                    SecurityComponent::check_values_150($request->post('id_of_teacher')),
                    SecurityComponent::check_values_150($request->post('isPosterDelete')),
                    $programPosterFileDTO
                );

                if(!$programDTO -> validate())return $this -> asJson(new JsonAnswerViewModel("error", "no_data")); 

                $programFacade = new ProgramFacade();

                return $this -> asJson(
                    $programFacade -> update($programDTO)
                    ? new JsonAnswerViewModel("success", null)
                    : new JsonAnswerViewModel("error", null)
                );
            } 
        */
    }
}
