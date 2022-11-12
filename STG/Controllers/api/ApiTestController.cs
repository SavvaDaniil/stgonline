using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using STG.Component;
using STG.Data;
using STG.DTO.Payment;
using STG.Facade;
using STG.Models;
using STG.Observer;
using STG.Service;
using STG.ViewModels;
using STG.ViewModels.User;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace STG.Controllers.api
{
    [Route("api/test")]
    [ApiController]
    public class ApiTestController : ControllerBase
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private ApplicationDbContext _dbc;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        public ApiTestController(IHttpContextAccessor httpContextAccessor, ApplicationDbContext dbc, IServiceScopeFactory serviceScopeFactory)
        {
            _httpContextAccessor = httpContextAccessor;
            _dbc = dbc;
            _serviceScopeFactory = serviceScopeFactory;
        }

        [Route("s")]
        public IActionResult test()
        {
            //System.Diagnostics.Debug.WriteLine("_httpContextAccessor.HttpContext.Request.Host.Value: " + _httpContextAccessor.HttpContext.Request.Host.Value);
            //System.Diagnostics.Debug.WriteLine("Вызов notification");

            //PaymentFacade paymentFacade = new PaymentFacade(_dbc);
            //PaymentService paymentService = new PaymentService(_dbc);

            //Payment payment = await paymentService.findById(123);
            //if (payment == null) return Ok("not_found");


            //PurchaseSubscriptionFacade purchaseSubscriptionFacade = new PurchaseSubscriptionFacade(_dbc, _serviceScopeFactory);

            //await purchaseSubscriptionFacade.getFirstActiveAndActivateIfNull(
            //    HttpContext,
            //    _httpContextAccessor.HttpContext.Request.Host.Value
            //);


            //await purchaseSubscriptionFacade.initExtend(_httpContextAccessor.HttpContext.Request.Host.Value, 63);
            //AmoCRMFacade amoCRMFacade = new AmoCRMFacade(_dbc);
            //await amoCRMFacade.test();

            return Ok();
        }

        [Route("amocrm_check_users")]
        public IActionResult amocrmCheckUsers()
        {
            UserFacade userFacade = new UserFacade(_dbc);
            List<UserAmoCRMData> userAmoCRMDatas = userFacade.checkUsersForIdOfAmocrm();

            return Ok(
                new JsonAnswerStatus(
                    "success",
                    null,
                    userAmoCRMDatas
                )    
            );
        }
    }
}
