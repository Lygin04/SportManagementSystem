using SportManagementSystem.BuildingBlocks;
using SportManagementSystem.BuildingBlocks.Abstractions;
using SportManagementSystem.Modules.Analytics.Application.Repositories;

namespace SportManagementSystem.Modules.Analytics.Application.Queries.GetClientAnalytics;

public class GetClientAnalyticsHandler(IClientAnalyticsRepository analyticsRepository)
    : IMessageHandler<GetClientAnalyticsMessage, MbResult<ClientAnalyticsOverviewResponse>>
{
    public async Task<MbResult<ClientAnalyticsOverviewResponse>> Handle(
        GetClientAnalyticsMessage request,
        CancellationToken cancellationToken)
    {
        var analytics = await analyticsRepository.GetClientOverviewAsync(request.ClientId, cancellationToken);
        return MbResult<ClientAnalyticsOverviewResponse>.Success(analytics);
    }
}
