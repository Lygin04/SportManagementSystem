using Microsoft.Extensions.Options;

namespace SportManagementSystem.BuildingBlocks.Time;

public class SystemAppClock(IOptions<AppTimeOptions> options) : IAppClock
{
    private readonly TimeZoneInfo _defaultTimeZone = AppTimeZoneResolver.Resolve(options.Value.DefaultTimeZoneId);

    public DateTimeOffset UtcNow => DateTimeOffset.UtcNow;

    public DateOnly TodayInDefaultTimeZone
    {
        get
        {
            var localTime = TimeZoneInfo.ConvertTime(UtcNow, _defaultTimeZone);
            return DateOnly.FromDateTime(localTime.DateTime);
        }
    }

    public TimeZoneInfo DefaultTimeZone => _defaultTimeZone;
}
