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
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Route("/auth/google")]
        public IActionResult GoogleAuth()
        {
            var script = @"<script>
                    var popup = window.open('https://accounts.google.com/o/oauth2/auth?scope=https://www.googleapis.com/auth/calendar&response_type=code&redirect_uri=https://your-redirect-url.com/callback&client_id=798723233176-5s05agfpq2p5jamqs2rglkuu444ohfef.apps.googleusercontent.com&prompt=consent', 'googleAuthPopup', 'width=500,height=600');
                    if (window.focus) { popup.focus(); }
                    </script>";

            return Content(script, "text/html");
        }

        public async Task<IActionResult> GoogleAuhtAccessToken()
        {
            return Ok();
        }

        public async Task<IActionResult> AddCalendarEvent([FromBody] GoogleCalendarReqDTO calendarEventReqDTO)
        {
            var data = _googleCalendarService.AddToGoogleCalendar(calendarEventReqDTO);
            return Ok();
        }

        
    }
}
