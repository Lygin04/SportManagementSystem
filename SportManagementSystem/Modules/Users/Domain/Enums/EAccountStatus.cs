using System.Text.Json.Serialization;

namespace SportManagementSystem.Modules.Users.Domain.Enums;

/// <summary>
/// Статус клиента спортивной организации.
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum EAccountStatus
{
    /// <summary>
    /// Активный клиент.
    /// </summary>
    Active,
    
    /// <summary>
    /// Временно заблокирован.
    /// </summary>
    Suspended,
    
    /// <summary>
    /// Неактивный клиент.
    /// </summary>
    Inactive,
}