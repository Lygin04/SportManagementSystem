namespace SportManagementSystem.BuildingBlocks.Time;

public interface IAppClock
{
    DateTimeOffset UtcNow { get; }
    DateOnly TodayInDefaultTimeZone { get; }
    TimeZoneInfo DefaultTimeZone { get; }
}
