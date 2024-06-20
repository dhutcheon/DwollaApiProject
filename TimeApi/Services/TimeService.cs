using TimeApi.Models;

namespace TimeApi.Services
{
    public class TimeService : ITimeService
    {
        public TimeResponse GetTime(string? timeZone = null)
        {
            var response = new TimeResponse
            {
                CurrentTime = DateTime.UtcNow.ToString(Constants.TimeFormat)
            };

            if (string.IsNullOrEmpty(timeZone))
            {
                return response;
            }

            if (!TimeZoneInfo.TryFindSystemTimeZoneById(timeZone, out var timeZoneInfo))
            {
                throw new ArgumentException($"Invalid {nameof(timeZone)} parameter: {timeZone}");
            }

            response.AdjustedTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, timeZoneInfo).ToString(Constants.TimeFormat);
            return response;
        }
    }
}