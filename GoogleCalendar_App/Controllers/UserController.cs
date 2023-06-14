using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Util.Store;
using GoogleCalendar_App.Common;
using GoogleCalendar_App.DTO;
using GoogleCalendar_App.IService;
using Microsoft.AspNetCore.Authentication;
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
            string scopeURL = "https://accounts.google.com/o/oauth2/auth?scope={0}&redirect_uri={1}&response_type={2}&client_id={3}&state={4}&access_type={5}&approval_prompt={6}";

            string state = string.Empty;
            var clientID = "798723233176-5s05agfpq2p5jamqs2rglkuu444ohfef.apps.googleusercontent.com";
            var redirectURL = "https://localhost:7272/auth/google";
            string scope = Method.urlEncodeForGoogle("https://www.googleapis.com/auth/calendar https://www.googleapis.com/auth/calendar.readonly").Replace("%20", "+");
            string redirect_uri_encode = Method.urlEncodeForGoogle(redirectURL);
            string response_type = "token";
            state = Method.urlEncodeForGoogle(state);
            string access_type = "online";
            string approval_prompt = "force";

            var mainURL = string.Format(scopeURL, scope, redirect_uri_encode, response_type, clientID, state, access_type, approval_prompt);
            //var accessToken = _googleCalendarService.Authenticate(string.Empty, redirectURL, clientID, "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");
            //var script = $@"<script>
            //        var popup = window.open('{mainURL}', 'googleAuthPopup', 'width=500,height=600');
            //        if (window.focus) {{ popup.focus(); }}
            //        </script>";

            var Scopes = new string[] { CalendarService.Scope.Calendar };


            var path = @"E:\\clientsecret.json";
            UserCredential credential;
            string filePath = Path.Combine(Environment.CurrentDirectory, "clientsecret.json");
            using (FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                string credPath = "E:\\";

                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.FromStream(stream).Secrets,
                    Scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(credPath, true)).Result;
            }

            return Content(string.Empty, "text/html");
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
