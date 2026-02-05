using System.Text.Json.Serialization;

namespace SportManagementSystem.Modules.Assets.Domain.Enums;

/// <summary>
/// Техническое состояние оборудования.
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum EEquipmentCondition
{
    /// <summary>
    /// Только с завода.
    /// </summary>
    New,
    
    /// <summary>
    /// Хорошее.
    /// </summary>
    Good,
    
    /// <summary>
    /// Требуется ремонт.
    /// </summary>
    NeedRepair,
    
    /// <summary>
    /// Сломанный.
    /// </summary>
    Broken
}