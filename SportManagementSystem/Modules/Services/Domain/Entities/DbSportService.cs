using SportManagementSystem.Modules.Services.Domain.Enums;
using SportManagementSystem.Modules.Assets.Domain.Entities;

namespace SportManagementSystem.Modules.Services.Domain.Entities;

/// <summary>
/// Спортивная услуга.
/// </summary>
public class DbSportService
{
    public long Id { get; set; }
    
    /// <summary>
    /// Код услуги.
    /// </summary>
    public string Code { get; set; }
    
    /// <summary>
    /// Название услуги.
    /// </summary>
    public string Name { get; set; }
    
    /// <summary>
    /// Описание услуги.
    /// </summary>
    public string? Description { get; set; }
    
    /// <summary>
    /// Категория услуги.
    /// </summary>
    public EServiceCategory? Category { get; set; }
    
    /// <summary>
    /// Активна ли услуга.
    /// </summary>
    public bool IsActive { get; set; }
    
    /// <summary>
    /// Дата и время создание записи.
    /// </summary>
    public DateTimeOffset Created { get; set; }
    
    /// <summary>
    /// Дата и время последнего изменения данных.
    /// </summary>
    public DateTimeOffset? Modified { get; set; }

    public long BranchId { get; set; }
    public DbBranch Branch { get; set; } = null!;
    
    public ICollection<DbServicePrice> Prices { get; set; } = new List<DbServicePrice>();
}
