using SportManagementSystem.BuildingBlocks;
using SportManagementSystem.BuildingBlocks.Abstractions;

namespace SportManagementSystem.Modules.Analytics.Application.Queries.GetClientAnalytics;

public record GetClientAnalyticsMessage(long ClientId) : IMessage<MbResult<ClientAnalyticsOverviewResponse>>;
