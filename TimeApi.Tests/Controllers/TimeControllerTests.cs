using Microsoft.AspNetCore.Mvc;
using Moq;
using TimeApi.Controllers;
using TimeApi.Models;
using TimeApi.Services;

namespace TimeApi.Tests
{
    public class TimeControllerTests
    {
        private readonly Mock<ITimeService> _timeServiceMock;
        private readonly TimeController _timeController;

        public TimeControllerTests()
        {
            _timeServiceMock = new Mock<ITimeService>();
            _timeController = new TimeController(_timeServiceMock.Object);
        }

        [Fact]
        public void GetTime_ShouldReturnOkObjectResult_WithCorrectTimeResponse()
        {
            var expectedResponse = new TimeResponse
            {
                CurrentTime = "2050-01-24T15:06:26Z",
                AdjustedTime = "2050-01-24T07:06:26-08:00"
            };

            _timeServiceMock.Setup(s => s.GetTime(null)).Returns(expectedResponse);

            var result = _timeController.GetTime();

            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<TimeResponse>(okResult.Value);
            Assert.Equal(expectedResponse.CurrentTime, returnValue.CurrentTime);
            Assert.Equal(expectedResponse.AdjustedTime, returnValue.AdjustedTime);
        }

        [Fact]
        public void GetTime_WithTimeZone_ShouldReturnOkObjectResult_WithAdjustedTime()
        {
            var timeZone = "Pacific Standard Time";
            var expectedResponse = new TimeResponse
            {
                CurrentTime = "2050-01-24T15:06:26Z",
                AdjustedTime = "2050-01-24T07:06:26-08:00"
            };

            _timeServiceMock.Setup(s => s.GetTime(timeZone)).Returns(expectedResponse);

            var result = _timeController.GetTime(timeZone);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<TimeResponse>(okResult.Value);
            Assert.Equal(expectedResponse.CurrentTime, returnValue.CurrentTime);
            Assert.Equal(expectedResponse.AdjustedTime, returnValue.AdjustedTime);
        }

        [Fact]
        public void GetTime_WithInvalidTimeZone_ShouldReturnBadRequest()
        {
            const string errMsg = "Invalid time zone";
            var invalidTimeZone = "Invalid/Timezone";

            _timeServiceMock.Setup(s => s.GetTime(invalidTimeZone)).Throws(new ArgumentException(errMsg));

            var result = _timeController.GetTime(invalidTimeZone);
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.NotNull(badRequestResult.Value);
            var expected = "{ Error = " + errMsg + " }";
            Assert.Equal(expected, badRequestResult.Value.ToString());
        }
    }
}
