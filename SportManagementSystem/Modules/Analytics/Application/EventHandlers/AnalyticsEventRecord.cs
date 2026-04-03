using System.Text.Json;

namespace SportManagementSystem.Modules.Analytics.Application.EventHandlers;

public sealed record AnalyticsEventRecord(
    Guid EventId,
    string EventType,
    long ClientId,
    long SourceEntityId,
    long SportServiceId,
    long BranchId,
    DateTimeOffset OccurredOnUtc,
    string MetadataJson)
{
    public static AnalyticsEventRecord Create(
        string eventType,
        long clientId,
        long sourceEntityId,
        long sportServiceId,
        long branchId,
        DateTimeOffset occurredOnUtc,
        IReadOnlyDictionary<string, object?> metadata)
    {
        return new AnalyticsEventRecord(
            Guid.NewGuid(),
            eventType,
            clientId,
            sourceEntityId,
            sportServiceId,
            branchId,
            occurredOnUtc.ToUniversalTime(),
            JsonSerializer.Serialize(metadata));
    }
}
