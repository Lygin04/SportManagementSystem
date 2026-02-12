using Hangfire.Dashboard;

namespace SportManagementSystem.Infrastructure.Hangfire;

public sealed class AllowAllDashboardAuthorizationFilter : IDashboardAuthorizationFilter
{
    public bool Authorize(DashboardContext context) => true;
}
