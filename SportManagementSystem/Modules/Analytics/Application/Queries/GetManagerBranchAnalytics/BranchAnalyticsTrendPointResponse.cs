namespace SportManagementSystem.Modules.Analytics.Application.Queries.GetManagerBranchAnalytics;

public sealed class BranchAnalyticsTrendPointResponse
{
    public string Date { get; init; } = string.Empty;
    public string Time { get; init; } = string.Empty;
    public string Label { get; init; } = string.Empty;
    public int BranchCardViews { get; init; }
    public int BranchDetailsOpens { get; init; }
    public int TrainingSessionBookings { get; init; }
    public int MembershipPurchases { get; init; }
}
