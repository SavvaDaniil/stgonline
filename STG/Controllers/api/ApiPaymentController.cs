using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using STG.Component;
using STG.Data;
using STG.DTO.Payment;
using STG.Facade;
using STG.Models.Robokassa;
using STG.Observer;
using STG.ViewModels;

namespace STG.Controllers.api
{
    [Route("api/payment")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "UserCookie")]
    public class ApiPaymentController : ControllerBase
    {
        ApplicationDbContext _dbc;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public ApiPaymentController(IHttpContextAccessor httpContextAccessor, ApplicationDbContext dbc)
        {
            this._dbc = dbc;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpPost]
        [Route("new")]
        public async Task<IActionResult> newPayment([FromForm]PaymentNewDTO paymentNewDTO)
        {
            if (!ModelState.IsValid) return Ok(new JsonAnswerStatus("error", "no_data"));

            PaymentFacade paymentFacade = new PaymentFacade(_dbc);

            return Ok(new JsonAnswerStatus(
                "success",
                null,
                await paymentFacade.createNewForBuyingByUser(HttpContext, _httpContextAccessor.HttpContext.Request.Host.Value, paymentNewDTO)
            ));
        }

        [HttpPost]
        [Route("robokassa/result")]
        [AllowAnonymous]
        [IgnoreAntiforgeryToken]
        public IActionResult robokassaResult([FromForm] RobokassaResultResponse robokassaResultResponse)
        {
            PaymentFacade paymentFacade = new PaymentFacade(_dbc);

            return Ok(
                paymentFacade.resultRobokassa(robokassaResultResponse) != null
                ? string.Format("OK{0}", robokassaResultResponse.InvId)
                : "bad sign"
            );
        }

        /*
        [HttpPost]
        [Route("result")]
        [AllowAnonymous]
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> result([FromForm] PaymentResultDTO paymentResultDTO)
        {
            if (!ModelState.IsValid) return Ok(new JsonAnswerStatus("error", "no_data"));

            PaymentFacade paymentFacade = new PaymentFacade(_dbc);

            return Ok(
                await paymentFacade.result(paymentResultDTO) != null
                ? new JsonAnswerStatus("success", null)
                : new JsonAnswerStatus("error", null)
            );
        }
        */


        [HttpGet]
        [Route("get_state/{id_of_payment}")]
        [AllowAnonymous]
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> getState(int id_of_payment)
        {
            PaymentFacade paymentFacade = new PaymentFacade(_dbc);

            return Ok(new JsonAnswerStatus("success", null, await paymentFacade.getState(id_of_payment)));
        }


        [HttpGet]
        [Route("get_state_and_check_receipt/{id_of_payment}")]
        [AllowAnonymous]
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> getStateAndCheckReceipt(int id_of_payment)
        {
            PaymentFacade paymentFacade = new PaymentFacade(_dbc);

            return Ok(new JsonAnswerStatus("success", null, await paymentFacade.getState(id_of_payment, true)));
        }


        [Route("notification")]
        [AllowAnonymous]
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> notification()
        {
            using (var reader = new StreamReader(Request.Body))
            {
                string body = reader.ReadToEnd();
                
                LoggerComponent.writeToLog("<p>На страницу notification прошли</p>"
                    + HttpContext.Connection.RemoteIpAddress + " - "
                    + body);
                

                PaymentFacade paymentFacade = new PaymentFacade(_dbc);
                await paymentFacade.updatePaymentDataFromNotification(body);

                body = null;
                reader.Dispose();
            }

            return Ok();
        }



    }
}
