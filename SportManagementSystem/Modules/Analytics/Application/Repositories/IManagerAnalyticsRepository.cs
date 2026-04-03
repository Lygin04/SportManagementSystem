using SportManagementSystem.Modules.Analytics.Application.Queries.GetManagerBranchAnalytics;

namespace SportManagementSystem.Modules.Analytics.Application.Repositories;

public interface IManagerAnalyticsRepository
{
    Task<ManagerBranchAnalyticsOverviewResponse> GetManagerOverviewAsync(
        IReadOnlyCollection<long> branchIds,
        string trendGranularity,
        DateOnly? selectedDate,
        long? selectedBranchId,
        CancellationToken cancellationToken);
}
