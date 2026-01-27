using System.Text.Json.Serialization;

namespace SportManagementSystem.Entities.Enums;

/// <summary>
/// Статус записи клиента.
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum EBookingStatus
{
    /// <summary>
    /// Забронированно.
    /// </summary>
    Booked,
    
    /// <summary>
    /// Посищено.
    /// </summary>
    Attended,
    
    /// <summary>
    /// Отменена.
    /// </summary>
    Canceled,
    
    /// <summary>
    /// 
    /// </summary>
    NoShow
}