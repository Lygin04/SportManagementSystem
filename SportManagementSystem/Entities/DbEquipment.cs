using SportManagementSystem.Entities.Enums;

namespace SportManagementSystem.Entities;

/// <summary>
/// Оборудование спортивной организации.
/// </summary>
public class DbEquipment
{
    /// <summary>
    /// Уникальный индентификатор оборудования.
    /// </summary>
    public long Id { get; set; }
    public long RoomId { get; set; }
    public DbRoom Room { get; set; }
    
    /// <summary>
    /// Наименование оборудования.
    /// </summary>
    public string Name { get; set; }
    
    /// <summary>
    /// Количество едениц.
    /// </summary>
    public int Quantity { get; set; }
    
    /// <summary>
    /// Техническое состояние.
    /// </summary>
    public EEquipmentCondition? Condition { get; set; }
}