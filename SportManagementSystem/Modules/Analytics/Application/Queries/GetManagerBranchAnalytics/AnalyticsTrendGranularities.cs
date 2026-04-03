namespace SportManagementSystem.Modules.Analytics.Application.Queries.GetManagerBranchAnalytics;

public static class AnalyticsTrendGranularities
{
    public const string Day = "day";
    public const string Hour = "hour";
    public const string TenMinutes = "ten_minutes";

    public static string Normalize(string? value)
    {
        if (string.Equals(value, TenMinutes, StringComparison.OrdinalIgnoreCase))
        {
            return TenMinutes;
        }

        return string.Equals(value, Hour, StringComparison.OrdinalIgnoreCase)
            ? Hour
            : Day;
    }
}
