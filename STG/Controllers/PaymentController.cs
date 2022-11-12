using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using STG.Data;
using STG.Facade;
using STG.Entities;
using STG.ViewModels.Payment;
using STG.Models.Robokassa;

namespace STG.Controllers
{
    [Route("/payment")]
    [Authorize(AuthenticationSchemes = "UserCookie")]
    public class PaymentController : Controller
    {
        ApplicationDbContext _dbc;
        public PaymentController(ApplicationDbContext dbc)
        {
            this._dbc = dbc;
        }


        [HttpGet]
        [Route("success/{id_of_payment}")]
        public IActionResult Success(int id_of_payment)
        {
            ViewData["idMenuActive"] = 0;
            PaymentFacade paymentFacade = new PaymentFacade(_dbc);
            PaymentLiteViewModel paymentLiteViewModel = paymentFacade.findPayed(id_of_payment);
            if (paymentLiteViewModel == null) return Redirect("error");

            return View(paymentLiteViewModel);
        }

        
        [Route("robokassa/success")]
        public IActionResult robokassaSuccess([FromForm] RobokassaSuccessResponse robokassaSuccessResponse)
        {
            ViewData["idMenuActive"] = 0;
            PaymentFacade paymentFacade = new PaymentFacade(_dbc);
            PaymentLiteViewModel paymentLiteViewModel = paymentFacade.findPayed(robokassaSuccessResponse.InvId);
            if (paymentLiteViewModel == null) return Redirect("/payment/error");

            return View("success", paymentLiteViewModel);
        }




        [HttpGet]
        [Route("error")]
        [AllowAnonymous]
        public IActionResult Error()
        {
            ViewData["idMenuActive"] = 0;
            return View();
        }


        [HttpGet]
        [Route("result_tinkoff/{id_of_payment}")]
        [AllowAnonymous]
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> resultTinkoff(int id_of_payment)
        {
            PaymentFacade paymentFacade = new PaymentFacade(_dbc);
            Payment payment = await paymentFacade.isConfirmedByTinkoffAndUpdateIfNeed(id_of_payment);
            if(payment == null) return RedirectToAction("Error");
            if (payment.user != null && payment.preUserWithAppointment != null) await signIn(payment.user);
            return Redirect("/payment/success/" + id_of_payment);
        }


        private async Task signIn(User user)
        {
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, "User")
            };
            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "UserCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
            ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

            await HttpContext.SignInAsync("UserCookie", claimsPrincipal);
        }
    }
}
