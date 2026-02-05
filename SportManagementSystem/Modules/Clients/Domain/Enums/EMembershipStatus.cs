using System.Text.Json.Serialization;

namespace SportManagementSystem.Modules.Clients.Domain.Enums;

/// <summary>
/// Статус абонемента.
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum EMembershipStatus
{
    /// <summary>
    /// Активно.
    /// </summary>
    Active,
    
    /// <summary>
    /// Истекший. 
    /// </summary>
    Expired,
    
    /// <summary>
    /// Заморожен.
    /// </summary>
    Frozen
}