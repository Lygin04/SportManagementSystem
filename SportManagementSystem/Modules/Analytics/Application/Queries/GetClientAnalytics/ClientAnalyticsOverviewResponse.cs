namespace SportManagementSystem.Modules.Analytics.Application.Queries.GetClientAnalytics;

public sealed class ClientAnalyticsOverviewResponse
{
    public long ClientId { get; init; }
    public int TrainingSessionBookings { get; init; }
    public int MembershipPurchases { get; init; }
}
