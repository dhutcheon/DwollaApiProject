using TimeApi.Models;
using TimeApi.Services;

namespace TimeApi.Tests
{
    public class TimeServiceTests
    {
        private readonly TimeService _timeService;

        public TimeServiceTests()
        {
            _timeService = new TimeService();
        }

        [Fact]
        public void GetTime_ShouldReturnCurrentTimeInUtc()
        {
            var currentTime = DateTime.UtcNow.ToString(Constants.TimeFormat);
            var result = _timeService.GetTime();

            Assert.NotNull(result);
            Assert.NotNull(result.CurrentTime);
            Assert.EndsWith("Z", result.CurrentTime);
            Assert.Equal(currentTime, result.CurrentTime);
            Assert.Null(result.AdjustedTime);
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
            Assert.NotNull(result.CurrentTime);
            Assert.NotNull(result.AdjustedTime);
            Assert.False(adjustedTime.EndsWith("Z"));
            Assert.Equal(adjustedTime, result.AdjustedTime);
        }
    }
}