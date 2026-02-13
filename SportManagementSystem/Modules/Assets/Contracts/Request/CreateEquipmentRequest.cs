using SportManagementSystem.Modules.Assets.Domain.Enums;

namespace SportManagementSystem.Modules.Assets.Contracts.Request;

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