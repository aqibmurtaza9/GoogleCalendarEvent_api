using GoogleCalendar_App.DTO;

namespace GoogleCalendar_App.IService
{
    public interface IGoogleCalendarService
    {
        string GetAuthCode();
        Task<GoogleTokenResponse> GetTokens(string code);
        string AddToGoogleCalendar(GoogleCalendarReqDTO googleCalendarReqDTO);
    }
}
