using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using STG.Data;
using STG.DTO.Analytics;
using STG.Facade;
using STG.ViewModels;
using System.Threading.Tasks;

namespace STG.Controllers.api
{
    [Route("api/analytics")]
    [Authorize(AuthenticationSchemes = "AdminCookie")]
    [ApiController]
    public class ApiAnalyticsController : ControllerBase
    {
        private ApplicationDbContext _dbc;
        public ApiAnalyticsController(ApplicationDbContext dbc)
        {
            this._dbc = dbc;
        }

        [Route("payments_history")]
        public IActionResult update([FromForm] AnalyticsPaymentsHistoryDTO analyticsPaymentsHistoryDTO)
        {
            if (ModelState.IsValid)
            {
                AnalyticsFacade analyticsFacade = new AnalyticsFacade(_dbc);
                JsonAnswerStatus jsonAnswerStatus = new JsonAnswerStatus(
                    "success", 
                    null,
                    analyticsFacade.listAllByDates(analyticsPaymentsHistoryDTO.dateFrom, analyticsPaymentsHistoryDTO.dateTo)
                );
                return Ok(jsonAnswerStatus);
            }
            return Ok(new JsonAnswerStatus("error", null));
        }
    }
}
