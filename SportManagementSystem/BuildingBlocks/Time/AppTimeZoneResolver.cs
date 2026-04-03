namespace SportManagementSystem.BuildingBlocks.Time;

public static class AppTimeZoneResolver
{
    public static TimeZoneInfo Resolve(string? timeZoneId)
    {
        if (string.IsNullOrWhiteSpace(timeZoneId))
        {
            return TimeZoneInfo.Utc;
        }

        try
        {
            return TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
        }
        catch (TimeZoneNotFoundException)
        {
        }

        if (OperatingSystem.IsWindows() &&
            TimeZoneInfo.TryConvertIanaIdToWindowsId(timeZoneId, out var windowsId))
        {
            return TimeZoneInfo.FindSystemTimeZoneById(windowsId);
        }

        if (!OperatingSystem.IsWindows() &&
            TimeZoneInfo.TryConvertWindowsIdToIanaId(timeZoneId, out var ianaId))
        {
            return TimeZoneInfo.FindSystemTimeZoneById(ianaId);
        }

        throw new InvalidOperationException($"Unknown time zone id: '{timeZoneId}'.");
    }
}
