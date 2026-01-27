using System.Text.Json.Serialization;

namespace SportManagementSystem.Entities.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ENotificationStatus
{
    Pending,
    Send,
    Failed
}