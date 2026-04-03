using Microsoft.AspNetCore.Http;
using SportManagementSystem.BuildingBlocks;
using SportManagementSystem.BuildingBlocks.Abstractions;
using SportManagementSystem.Modules.Analytics.Application.Repositories;
using SportManagementSystem.Modules.Assets.Domain.Repositories;

namespace SportManagementSystem.Modules.Analytics.Application.Queries.GetManagerBranchAnalytics;

public class GetManagerBranchAnalyticsHandler(
    IBranchRepository branchRepository,
    IManagerAnalyticsRepository analyticsRepository)
    : IMessageHandler<GetManagerBranchAnalyticsMessage, MbResult<ManagerBranchAnalyticsOverviewResponse>>
{
    public async Task<MbResult<ManagerBranchAnalyticsOverviewResponse>> Handle(
        GetManagerBranchAnalyticsMessage request,
        CancellationToken cancellationToken)
    {
        var branches = await branchRepository.GetManagedByStaffAsync(request.UserId, cancellationToken);
        var normalizedGranularity = AnalyticsTrendGranularities.Normalize(request.TrendGranularity);
        var distinctBranches = branches
            .GroupBy(branch => branch.Id)
            .Select(group => group.First())
            .OrderBy(branch => branch.Name)
            .ToList();

        if (distinctBranches.Count == 0)
        {
            return MbResult<ManagerBranchAnalyticsOverviewResponse>.Success(new ManagerBranchAnalyticsOverviewResponse
            {
                TrendGranularity = normalizedGranularity,
                SelectedBranchId = request.BranchId
            });
        }

        if (request.BranchId.HasValue && distinctBranches.All(branch => branch.Id != request.BranchId.Value))
        {
            return MbResult<ManagerBranchAnalyticsOverviewResponse>.Failure(new MbError(
                title: "Branch not found",
                status: StatusCodes.Status404NotFound,
                detail: "Филиал не найден или недоступен управляющему"));
        }

        var analytics = await analyticsRepository.GetManagerOverviewAsync(
            distinctBranches.Select(branch => branch.Id).ToArray(),
            normalizedGranularity,
            request.SelectedDate,
            request.BranchId,
            cancellationToken);

        foreach (var summary in analytics.Branches)
        {
            summary.BranchName = distinctBranches.FirstOrDefault(branch => branch.Id == summary.BranchId)?.Name ?? summary.BranchName;
        }

        var existingIds = analytics.Branches.Select(branch => branch.BranchId).ToHashSet();
        foreach (var branch in distinctBranches.Where(branch => !existingIds.Contains(branch.Id)))
        {
            analytics.Branches.Add(new BranchAnalyticsSummaryResponse
            {
                BranchId = branch.Id,
                BranchName = branch.Name
            });
        }

        analytics.Branches = analytics.Branches
            .OrderByDescending(branch => branch.BranchDetailsOpens)
            .ThenBy(branch => branch.BranchName)
            .ToList();

        return MbResult<ManagerBranchAnalyticsOverviewResponse>.Success(analytics);
    }
}
