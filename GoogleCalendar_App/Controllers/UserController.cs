using GoogleCalendar_App.Common;
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
        public IActionResult GoogleAuth()
        {
            string Url = "https://accounts.google.com/o/oauth2/auth?scope={0}&redirect_uri={1}&response_type={2}&client_id={3}&state={4}&access_type={5}&approval_prompt={6}";

            string state = string.Empty;
            var clientID = string.Empty;
            var redirectURL = string.Empty;
            string scope = Method.urlEncodeForGoogle("https://www.googleapis.com/auth/calendar https://www.googleapis.com/auth/calendar.readonly").Replace("%20", "+");
            string redirect_uri_encode = Method.urlEncodeForGoogle(redirectURL);
            string response_type = "code";
            state = Method.urlEncodeForGoogle(state);
            string access_type = "offline";
            string approval_prompt = "force";

            var mainURL = string.Format(Url, scope, redirect_uri_encode, response_type, clientID, state, access_type, approval_prompt);


            var script = @"<script>
                    var popup = window.open('mainURL', 'googleAuthPopup', 'width=500,height=600');
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
