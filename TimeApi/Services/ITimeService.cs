using TimeApi.Models;

namespace TimeApi.Services;

public interface ITimeService
{
    public TimeResponse GetTime(string? timeZone = null);
}