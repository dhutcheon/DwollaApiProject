using TimeApi.Models;

namespace TimeApi.Services;

public class TimeService : ITimeService
{
    public TimeResponse GetTime(string? timezone = null)
    {
        var response = new TimeResponse
        {
            currentTime = DateTime.UtcNow.ToString(Constants.TimeFormat)
        };

        if (string.IsNullOrEmpty(timezone))
        {
            return response;
        }

        timezone = timezone.Trim();
        if (!TimeZoneInfo.TryFindSystemTimeZoneById(timezone, out var timeZoneInfo))
        {
            throw new ArgumentException($"Invalid {nameof(timezone)} parameter: {timezone}");
        }

        response.adjustedTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, timeZoneInfo).ToString(Constants.TimeFormat);
        return response;
    }
}