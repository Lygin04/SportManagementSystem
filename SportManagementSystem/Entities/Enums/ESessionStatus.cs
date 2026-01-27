using System.Text.Json.Serialization;

namespace SportManagementSystem.Entities.Enums;

/// <summary>
/// Статус проведения занятия.
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ESessionStatus
{
    /// <summary>
    /// Запланирован.
    /// </summary>
    Planned,
    
    /// <summary>
    /// Проведено.
    /// </summary>
    Done,
    
    /// <summary>
    /// Отменено.
    /// </summary>
    Canceled
}