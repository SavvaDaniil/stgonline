using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using STG.Data;
using STG.DTO.ConnectionUserToPrivatePackage;
using STG.DTO.Package;
using STG.DTO.PackageChat;
using STG.Facade;
using STG.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace STG.Controllers.api
{
    [Route("api/package_chat")]
    [ApiController]
    public class ApiPackageChatController : ControllerBase
    {
        private ApplicationDbContext _dbc;
        public ApiPackageChatController(ApplicationDbContext dbc)
        {
            this._dbc = dbc;
        }


        [HttpPost]
        [Route("get")]
        [Authorize(AuthenticationSchemes = "UserCookie")]
        public IActionResult get([FromForm] PackageIdDTO packageIdDTO)
        {
            if (ModelState.IsValid)
            {
                PackageChatFacade packageChatFacade = new PackageChatFacade(_dbc);
                return Ok(
                    new JsonAnswerStatus(
                        "success",
                        null,
                        packageChatFacade.get(HttpContext, packageIdDTO.id_of_package)
                    )
                );
            }

            return Ok(new JsonAnswerStatus("error", null));
        }


        [Route("get_chat")]
        public IActionResult get_chat([FromForm] ConnectionUserToPackagePrivateIdDTO connectionUserToPackagePrivateIdDTO)
        {
            if (ModelState.IsValid)
            {
                PackageChatFacade packageChatFacade = new PackageChatFacade(_dbc);
                return Ok(
                    new JsonAnswerStatus(
                        "success",
                        null,
                        packageChatFacade.get(connectionUserToPackagePrivateIdDTO.id)
                    )
                );
            }

            return Ok(new JsonAnswerStatus("error", null));
        }


        [HttpPost]
        [Authorize(AuthenticationSchemes = "AdminCookie")]
        [Route("message_from_admin")]
        public IActionResult newMessageFromAdmin([FromForm] PackageChatNewMessageFromAdminDTO packageChatNewMessageFromAdminDTO)
        {
            if (ModelState.IsValid)
            {
                PackageChatFacade packageChatFacade = new PackageChatFacade(_dbc);
                return Ok(packageChatFacade.sendMessageFromAdminToUser(HttpContext, packageChatNewMessageFromAdminDTO));
            }

            return Ok(new JsonAnswerStatus("error", "no_date"));
        }


        [HttpPost]
        [Authorize(AuthenticationSchemes = "UserCookie")]
        [Route("message_from_user")]
        public IActionResult newMessageFromUser([FromForm] PackageChatNewMessageFromUserDTO packageChatNewMessageFromUserDTO)
        {
            if (ModelState.IsValid)
            {
                PackageChatFacade packageChatFacade = new PackageChatFacade(_dbc);
                return Ok(packageChatFacade.sendMessageFromUser(HttpContext, packageChatNewMessageFromUserDTO));
            }

            return Ok(new JsonAnswerStatus("error", "no_date"));
        }


    }
}
