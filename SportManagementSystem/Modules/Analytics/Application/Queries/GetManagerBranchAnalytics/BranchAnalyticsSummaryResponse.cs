namespace SportManagementSystem.Modules.Analytics.Application.Queries.GetManagerBranchAnalytics;

public sealed class BranchAnalyticsSummaryResponse
{
    public long BranchId { get; init; }
    public string BranchName { get; set; } = string.Empty;
    public int BranchCardViews { get; set; }
    public int BranchDetailsOpens { get; set; }
    public int TrainingSessionBookings { get; set; }
    public int MembershipPurchases { get; set; }
}
