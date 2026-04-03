using MediatR;
using SportManagementSystem.Modules.Analytics.Domain.Events;

namespace SportManagementSystem.Modules.Analytics.Application.EventHandlers;

public class BranchCardViewedDomainEventHandler(IAnalyticsEventWriter analyticsEventWriter)
    : INotificationHandler<BranchCardViewedDomainEvent>
{
    public Task Handle(BranchCardViewedDomainEvent notification, CancellationToken cancellationToken)
    {
        var analyticsEvent = AnalyticsEventRecord.Create(
            AnalyticsEventTypes.BranchCardViewed,
            notification.ClientId ?? 0,
            notification.BranchId,
            0,
            notification.BranchId,
            notification.OccurredOnUtc,
            new Dictionary<string, object?>
            {
                ["userRole"] = notification.UserRole,
                ["isAuthenticatedClient"] = notification.ClientId.HasValue
            });

        return analyticsEventWriter.WriteAsync(analyticsEvent, cancellationToken);
    }
}
