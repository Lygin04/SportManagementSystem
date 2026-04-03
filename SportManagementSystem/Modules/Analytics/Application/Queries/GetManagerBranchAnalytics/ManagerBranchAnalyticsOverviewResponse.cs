namespace SportManagementSystem.Modules.Analytics.Application.Queries.GetManagerBranchAnalytics;

public sealed class ManagerBranchAnalyticsOverviewResponse
{
    public List<BranchAnalyticsSummaryResponse> Branches { get; set; } = [];
    public List<BranchAnalyticsTrendPointResponse> Trend { get; set; } = [];
    public string TrendGranularity { get; set; } = AnalyticsTrendGranularities.Day;
    public string? SelectedDate { get; set; }
    public long? SelectedBranchId { get; set; }
    public List<string> AvailableDates { get; set; } = [];
}
