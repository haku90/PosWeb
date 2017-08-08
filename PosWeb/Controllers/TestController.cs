using System;
using Microsoft.AspNetCore.Mvc;
using PosWeb.Services;

namespace PosWeb.Controllers
{
    public class TestController : Controller
    {
        private TestService _testService;

        public TestController(TestService testService)
        {
            _testService = testService;
        }

        [HttpGet]
		[Route("api/ping")]
        public string Test()
        {
            try
            {
                _testService.AddTestObject();
                return "pong";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}
