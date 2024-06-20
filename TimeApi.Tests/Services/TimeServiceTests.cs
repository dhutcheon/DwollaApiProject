using TimeApi.Models;
using TimeApi.Services;

namespace TimeApi.Tests.Services;

public class TimeServiceTests
{
    private readonly TimeService _timeService = new();

    [Fact]
    public void GetTime_ShouldReturnCurrentTimeInUtc()
    {
        var currentTime = DateTime.UtcNow.ToString(Constants.TimeFormat);
        var result = _timeService.GetTime();

        Assert.NotNull(result);
        Assert.NotNull(result.currentTime);
        Assert.EndsWith("Z", result.currentTime);
        Assert.Equal(currentTime, result.currentTime);
        Assert.Null(result.adjustedTime);
    }

    [Fact]
    public void GetTime_WithInvalidTimeZone_ShouldThrowArgumentException()
    {
        Assert.Throws<ArgumentException>(() => _timeService.GetTime("Some Bad Timezone"));
    }

    [Fact]
    public void GetTime_WithValidTimeZone_ShouldReturnAdjustedTime()
    {
        const string timeZoneId = "Pacific Standard Time";
        var adjustedTime = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, timeZoneId).ToString(Constants.TimeFormat);
        var result = _timeService.GetTime(timeZoneId);

        Assert.NotNull(result);
        Assert.NotNull(result.currentTime);
        Assert.NotNull(result.adjustedTime);
        Assert.False(adjustedTime.EndsWith("Z"));
        Assert.Equal(adjustedTime, result.adjustedTime);
    }
}