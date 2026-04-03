using SportManagementSystem.Modules.Analytics.Application.Queries.GetClientAnalytics;

namespace SportManagementSystem.Modules.Analytics.Application.Repositories;

public interface IClientAnalyticsRepository
{
    Task<ClientAnalyticsOverviewResponse> GetClientOverviewAsync(long clientId, CancellationToken cancellationToken);
}
