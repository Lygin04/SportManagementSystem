using MediatR;
using SportManagementSystem.Modules.Analytics.Domain.Events;

namespace SportManagementSystem.Modules.Analytics.Application.EventHandlers;

public class ClientBookedTrainingSessionDomainEventHandler(IAnalyticsEventWriter analyticsEventWriter)
    : INotificationHandler<ClientBookedTrainingSessionDomainEvent>
{
    public Task Handle(ClientBookedTrainingSessionDomainEvent notification, CancellationToken cancellationToken)
    {
        var analyticsEvent = AnalyticsEventRecord.Create(
            AnalyticsEventTypes.TrainingSessionBooked,
            notification.ClientId,
            notification.BookingId,
            notification.SportServiceId,
            notification.BranchId,
            notification.OccurredOnUtc,
            new Dictionary<string, object?>
            {
                ["sessionId"] = notification.SessionId,
                ["timeZoneId"] = notification.TimeZoneId
            });

        return analyticsEventWriter.WriteAsync(analyticsEvent, cancellationToken);
    }
}
