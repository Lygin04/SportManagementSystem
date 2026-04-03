using MediatR;

namespace SportManagementSystem.Modules.Analytics.Domain.Events;

public sealed record ClientPurchasedMembershipDomainEvent(
    long ClientId,
    long MembershipId,
    long? MembershipTemplateId,
    long SportServiceId,
    long BranchId,
    int TotalVisits,
    int? RemainingVisits,
    long PurchasedByUserId,
    string PurchasedByRole,
    DateTimeOffset OccurredOnUtc) : INotification;
