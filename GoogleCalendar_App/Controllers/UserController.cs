using Google.Apis.Auth.AspNetCore3;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Auth.OAuth2.Responses;
using Google.Apis.Calendar.v3;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using GoogleCalendar_App.Common;
using GoogleCalendar_App.DTO;
using GoogleCalendar_App.IService;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading;

namespace GoogleCalendar_App.Controllers
{
    public class UserController : Controller
    {
        private IGoogleCalendarService _googleCalendarService;
        private IHttpContextAccessor _httpContextAccessor;

        public UserController(IGoogleCalendarService googleCalendarService, IHttpContextAccessor httpContextAccessor)
        {
            _googleCalendarService = googleCalendarService;
            _httpContextAccessor = httpContextAccessor;
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
