using Hangfire.Dashboard;

namespace SportManagementSystem.BuildingBlocks.Hangfire;

public sealed class AllowAllDashboardAuthorizationFilter : IDashboardAuthorizationFilter
{
    public bool Authorize(DashboardContext context) => true;
}
