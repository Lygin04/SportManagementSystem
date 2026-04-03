using MediatR;
using SportManagementSystem.Modules.Analytics.Domain.Events;

namespace SportManagementSystem.Modules.Analytics.Application.EventHandlers;

public class ClientPurchasedMembershipDomainEventHandler(IAnalyticsEventWriter analyticsEventWriter)
    : INotificationHandler<ClientPurchasedMembershipDomainEvent>
{
    public Task Handle(ClientPurchasedMembershipDomainEvent notification, CancellationToken cancellationToken)
    {
        var analyticsEvent = AnalyticsEventRecord.Create(
            AnalyticsEventTypes.MembershipPurchased,
            notification.ClientId,
            notification.MembershipId,
            notification.SportServiceId,
            notification.BranchId,
            notification.OccurredOnUtc,
            new Dictionary<string, object?>
            {
                ["membershipTemplateId"] = notification.MembershipTemplateId,
                ["totalVisits"] = notification.TotalVisits,
                ["remainingVisits"] = notification.RemainingVisits,
                ["purchasedByUserId"] = notification.PurchasedByUserId,
                ["purchasedByRole"] = notification.PurchasedByRole
            });

        return analyticsEventWriter.WriteAsync(analyticsEvent, cancellationToken);
    }
}
