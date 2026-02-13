using System.Text.Json.Serialization;

namespace SportManagementSystem.Modules.Assets.Domain.Enums;

/// <summary>
/// Состояние помещения.
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ERoomStatus
{
    /// <summary>
    /// Допустимый.
    /// </summary>
    Available,
    
    /// <summary>
    /// Поддержаное.
    /// </summary>
    Maintenance,
    
    /// <summary>
    /// Закрытое.
    /// </summary>
    Closed
}