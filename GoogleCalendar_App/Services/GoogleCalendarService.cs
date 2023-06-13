using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Auth.OAuth2.Responses;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Services;
using GoogleCalendar_App.DTO;
using GoogleCalendar_App.IService;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Text;

namespace GoogleCalendar_App.Services
{
    public class GoogleCalendarService : IGoogleCalendarService
    {
        private readonly HttpClient _httpClient;

        public GoogleCalendarService()
        {
            _httpClient = new HttpClient();
        }

        public string AddToGoogleCalendar(GoogleCalendarReqDTO googleCalendarReqDTO)
        {
            try
            {
                var token = new TokenResponse { RefreshToken = googleCalendarReqDTO.refreshToken };
                var credentials = new UserCredential(new GoogleAuthorizationCodeFlow(
                    new GoogleAuthorizationCodeFlow.Initializer
                    {
                        ClientSecrets = new ClientSecrets { ClientId = "798723233176-5s05agfpq2p5jamqs2rglkuu444ohfef.apps.googleusercontent.com", ClientSecret = "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" }
                    }), "user", token);


                var service = new CalendarService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = credentials,
                });

                Event newEvent = new Event()
                {
                    Summary = googleCalendarReqDTO.Summary,
                    Location = googleCalendarReqDTO.Location,
                    Description = googleCalendarReqDTO.Description,
                    Start = new EventDateTime()
                    {
                        DateTime = googleCalendarReqDTO.StartTime,
                    },
                    End = new EventDateTime()
                    {
                        DateTime = googleCalendarReqDTO.EndTime,
                    },
                    Reminders = new Event.RemindersData()
                    {
                        UseDefault = false,
                        Overrides = new EventReminder[] {
                                    new EventReminder() { Method = "email", Minutes = 30 },
                                    new EventReminder() { Method = "popup", Minutes = 15 },
                                    new EventReminder() { Method = "popup", Minutes = 1 },
                                }
                    }
                };

                EventsResource.InsertRequest insertRequest = service.Events.Insert(newEvent, googleCalendarReqDTO.CalendarId);
                Event createdEvent = insertRequest.Execute();
                return createdEvent.Id;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return string.Empty;
            }

        }

        public async Task<string> Authenticate(string code, string redirectUri, string clientId, string clientSecret)
        {
            var tokenEndpoint = "https://accounts.google.com/o/oauth2/token";

            var content = new StringContent($"code={code}&redirect_uri={Uri.EscapeDataString(redirectUri)}&client_id={clientId}&client_secret={clientSecret}&grant_type=authorization_code", Encoding.UTF8, "application/x-www-form-urlencoded");

            var response = await _httpClient.PostAsync(tokenEndpoint, content);

            var responseContent = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                // Extract the access token from the response
                var accessToken = JObject.Parse(responseContent)["access_token"].ToString();
                return accessToken;
            }
            else
            {
                // Handle the error case when authentication fails
                throw new Exception($"Failed to authenticate: {responseContent}");
            }
        }


    }
}
