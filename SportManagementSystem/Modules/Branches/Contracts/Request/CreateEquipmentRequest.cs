using SportManagementSystem.Entities.Enums;

namespace SportManagementSystem.Modules.Branches.Contracts.Request;

public class CreateEquipmentRequest
{
    public long RoomId { get; set; }
    
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