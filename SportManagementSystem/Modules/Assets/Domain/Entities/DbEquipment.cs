using SportManagementSystem.Modules.Assets.Domain.Enums;
using SportManagementSystem.Modules.Images.Domain.Entities;

namespace SportManagementSystem.Modules.Assets.Domain.Entities;

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
    
    /// <summary>
    /// Фотографии оборудования.
    /// </summary>
    public ICollection<DbImage> Images { get; set; } = new List<DbImage>();
}