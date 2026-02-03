using SportManagementSystem.Entities.Enums;

namespace SportManagementSystem.Modules.Assets.Domain.Entities;

/// <summary>
/// Помещение (зал, баня, массажный кабинет).
/// </summary>
public class DbRoom
{
    /// <summary>
    /// Уникальный индентификатор помещения.
    /// </summary>
    public long Id { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    public long BranchId { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    public DbBranch Branch { get; set; }
    
    /// <summary>
    /// Название помещения.
    /// </summary>
    public string Name { get; set; } = string.Empty;
    
    /// <summary>
    /// Вместимость помещения.
    /// </summary>
    public int? Capacity { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    public string? Type { get; set; }
    
    /// <summary>
    /// Текущее состояние помещения.
    /// </summary>
    public ERoomStatus? Status { get; set; }
    
    /// <summary>
    /// Оборудование, размещенное в помещении.
    /// </summary>
    public ICollection<DbEquipment> Equipments { get; set; }
}