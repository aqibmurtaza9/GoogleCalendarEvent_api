namespace GoogleCalendar_App.DTO
{
    public class GoogleCalendarReqDTO
    {
        public long EventId { get; set; }
        public long UserID { get; set; }
        public string Summary { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string CalendarId { get; set; }
        public string refreshToken { get; set; }
    }
}
