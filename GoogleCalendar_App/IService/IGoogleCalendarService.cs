using GoogleCalendar_App.DTO;

namespace GoogleCalendar_App.IService
{
    public interface IGoogleCalendarService
    {
        Task<string> Authenticate(string code, string redirectUri, string clientId, string clientSecret);
        string AddToGoogleCalendar(GoogleCalendarReqDTO googleCalendarReqDTO);
    }
}
