using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using STG.Data;
using STG.DTO.Extend;
using STG.DTO.PurchaseSubscription;
using STG.DTO.UserDTO;
using STG.Facade;
using STG.ViewModels;
using STG.ViewModels.Json;

namespace STG.Controllers.api
{
    [Route("api/purchase_subscription")]
    [ApiController]
    public class ApiPurchaseSubscriptionController : ControllerBase
    {
        ApplicationDbContext _dbc;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private IServiceScopeFactory _serviceScopeFactory;
        public ApiPurchaseSubscriptionController(IHttpContextAccessor httpContextAccessor, ApplicationDbContext dbc, IServiceScopeFactory serviceScopeFactory)
        {
            this._dbc = dbc;
            _httpContextAccessor = httpContextAccessor;
            _serviceScopeFactory = serviceScopeFactory;
        }



        [HttpPost]
        [IgnoreAntiforgeryToken]
        [Route("relaunch_extend")]
        public IActionResult relaunch_extend([FromBody] PurchaseSubscriptionIdDTO purchaseSubscriptionIdDTO)
        {
            if (!ModelState.IsValid) return Ok(new JsonAnswerStatus("error", "no_data"));

            PurchaseSubscriptionFacade purchaseSubscriptionFacade = new PurchaseSubscriptionFacade(_dbc, _serviceScopeFactory);

            return Ok(
                purchaseSubscriptionFacade.relaunchExtendThread(_httpContextAccessor.HttpContext.Request.Host.Value, purchaseSubscriptionIdDTO.id)
                ? new JsonAnswerStatus("success", null)
                : new JsonAnswerStatus("error", "unknown")
            );
        }


        [HttpPost]
        [IgnoreAntiforgeryToken]
        [AllowAnonymous]
        [Route("extend")]
        public IActionResult extend([FromBody] ExtendOfPurchaseSubscriptionDTO extendOfPurchaseSubscriptionDTO)
        {
            if (!ModelState.IsValid) return Ok(new JsonAnswerStatus("error", "no_data"));

            PurchaseSubscriptionFacade purchaseSubscriptionFacade = new PurchaseSubscriptionFacade(_dbc, _serviceScopeFactory);

            return Ok(
                purchaseSubscriptionFacade.lauchExtendFromApiExtend(_httpContextAccessor.HttpContext.Request.Host.Value, extendOfPurchaseSubscriptionDTO)
                ? new JsonAnswerStatus("success", null)
                : new JsonAnswerStatus("error", "unknown")
            );
        }


        [HttpPost]
        [Authorize(AuthenticationSchemes = "AdminCookie")]
        [Route("user_purchases_subscription")]
        public IActionResult listAllUserPurchasesSubscription([FromForm] UserIdDTO userIdDTO)
        {
            if (!ModelState.IsValid) return Ok(new JsonAdminAnswer("error", null));

            PurchaseSubscriptionFacade purchaseSubscriptionFacade = new PurchaseSubscriptionFacade(_dbc, _serviceScopeFactory);
            return Ok(purchaseSubscriptionFacade.listAllUserPurchasesSubscription(userIdDTO.id));
        }


        [HttpPost]
        [Authorize(AuthenticationSchemes = "AdminCookie")]
        [Route("user_get")]
        public IActionResult getOfUser([FromForm] PurchaseSubscriptionIdDTO purchaseSubscriptionIdDTO)
        {
            if (!ModelState.IsValid) return Ok(new JsonAdminAnswer("error", null));
            PurchaseSubscriptionFacade purchaseSubscriptionFacade = new PurchaseSubscriptionFacade(_dbc, _serviceScopeFactory);
            return Ok(purchaseSubscriptionFacade.getOfUser(purchaseSubscriptionIdDTO.id));
        }


        [HttpPost]
        [Authorize(AuthenticationSchemes = "AdminCookie")]
        [Route("user_save")]
        public IActionResult saveOfUser([FromForm] PurchaseSubscriptionDTO purchaseSubscriptionDTO)
        {
            if (!ModelState.IsValid) return Ok(new JsonAdminAnswer("error", null));
            PurchaseSubscriptionFacade purchaseSubscriptionFacade = new PurchaseSubscriptionFacade(_dbc, _serviceScopeFactory);
            return Ok(purchaseSubscriptionFacade.saveOfUser(purchaseSubscriptionDTO));
        }


        [HttpPost]
        [Authorize(AuthenticationSchemes = "AdminCookie")]
        [Route("admin_add")]
        public IActionResult adminAdd([FromForm] PurchaseSubscriptionNewByAdminDTO purchaseSubscriptionNewByAdminDTO)
        {
            if (!ModelState.IsValid) return Ok(new JsonAdminAnswer("error", null));
            PurchaseSubscriptionFacade purchaseSubscriptionFacade = new PurchaseSubscriptionFacade(_dbc, _serviceScopeFactory);
            return Ok(purchaseSubscriptionFacade.addByAdmin(purchaseSubscriptionNewByAdminDTO));
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = "AdminCookie")]
        [Route("admin_delete")]
        public IActionResult adminDelete([FromForm] PurchaseSubscriptionIdDTO purchaseSubscriptionIdDTO)
        {
            if (!ModelState.IsValid) return Ok(new JsonAdminAnswer("error", null));
            PurchaseSubscriptionFacade purchaseSubscriptionFacade = new PurchaseSubscriptionFacade(_dbc, _serviceScopeFactory);
            return Ok(purchaseSubscriptionFacade.deleteByAdmin(purchaseSubscriptionIdDTO.id));
        }
    }
}
