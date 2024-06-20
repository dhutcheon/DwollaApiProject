using Microsoft.AspNetCore.Mvc;
using TimeApi.Services;

namespace TimeApi.Controllers;

[ApiController]
[Route("api/time")]
public class TimeController : ControllerBase
{
    private readonly ITimeService _timeService;

    public TimeController(ITimeService timeService)
    {
        _timeService = timeService;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult GetTime([FromQuery] string? timezone = null)
    {
        try
        {
            var response = _timeService.GetTime(timezone);
            return Ok(response);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { Error = ex.Message });
        }
    }
}