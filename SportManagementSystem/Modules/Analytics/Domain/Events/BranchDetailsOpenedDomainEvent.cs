using MediatR;

namespace SportManagementSystem.Modules.Analytics.Domain.Events;

public sealed record BranchDetailsOpenedDomainEvent(
    long BranchId,
    long? ClientId,
    string UserRole,
    DateTimeOffset OccurredOnUtc) : INotification;
