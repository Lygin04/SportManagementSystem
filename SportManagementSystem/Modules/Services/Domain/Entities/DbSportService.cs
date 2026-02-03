using SportManagementSystem.Entities.Enums;

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
    public DateTime Created { get; set; }
    
    /// <summary>
    /// Дата и время последнего изменения данных.
    /// </summary>
    public DateTime? Modified { get; set; }
    
    public ICollection<DbServicePrice> Prices { get; set; }
}