using SportManagementSystem.BuildingBlocks;
using SportManagementSystem.BuildingBlocks.Abstractions;

namespace SportManagementSystem.Modules.Analytics.Application.Queries.GetManagerBranchAnalytics;

public record GetManagerBranchAnalyticsMessage(
    long UserId,
    string Role,
    string TrendGranularity,
    DateOnly? SelectedDate,
    long? BranchId)
    : IMessage<MbResult<ManagerBranchAnalyticsOverviewResponse>>;
