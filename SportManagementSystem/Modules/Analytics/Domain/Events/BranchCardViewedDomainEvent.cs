using MediatR;

namespace SportManagementSystem.Modules.Analytics.Domain.Events;

public sealed record BranchCardViewedDomainEvent(
    long BranchId,
    long? ClientId,
    string UserRole,
    DateTimeOffset OccurredOnUtc) : INotification;
