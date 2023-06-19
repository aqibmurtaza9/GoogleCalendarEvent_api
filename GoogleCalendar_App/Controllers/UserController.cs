using GoogleCalendar_App.DTO;
using GoogleCalendar_App.IService;
using Microsoft.AspNetCore.Mvc;

namespace GoogleCalendar_App.Controllers
{
    public class UserController : Controller
    {
        private IGoogleCalendarService _googleCalendarService;
        public UserController(IGoogleCalendarService googleCalendarService)
        {
            _googleCalendarService = googleCalendarService;
        }

        [HttpGet]
        [Route("/user/index")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Route("/auth/google")]
        public async Task<IActionResult> GoogleAuth()
        {
            return Redirect(_googleCalendarService.GetAuthCode());
        }

        [HttpGet]
        [Route("/auth/callback")]
        public async Task<IActionResult> Callback()
        {
            string code = HttpContext.Request.Query["code"];
            string scope = HttpContext.Request.Query["scope"];

            //get token method
            var token = await _googleCalendarService.GetTokens(code);
            return Ok(token);
        }

        [HttpPost]
        [Route("/user/calendarevent")]
        public async Task<IActionResult> AddCalendarEvent([FromBody] GoogleCalendarReqDTO calendarEventReqDTO)
        {
            var data = _googleCalendarService.AddToGoogleCalendar(calendarEventReqDTO);
            return Ok();
        }

    }
}
