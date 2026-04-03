using MediatR;

namespace SportManagementSystem.Modules.Analytics.Domain.Events;

public sealed record ClientBookedTrainingSessionDomainEvent(
    long ClientId,
    long BookingId,
    long SessionId,
    long SportServiceId,
    long BranchId,
    string TimeZoneId,
    DateTimeOffset OccurredOnUtc) : INotification;
