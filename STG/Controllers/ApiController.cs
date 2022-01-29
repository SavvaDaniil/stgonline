using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace STG.Controllers
{
    [Route("api0/[controller]")]
    [ApiController]
    public class ApiController : ControllerBase
    {
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Registration()
        {
            Console.WriteLine("---------- Не сюда");

            return null;
        }
    }
}