using SportManagementSystem.Entities.Enums;

namespace SportManagementSystem.Modules.Services.Domain.Entities;

/// <summary>
/// Цена за услугу.
/// </summary>
public class DbServicePrice
{
    public long Id { get; set; }
    public long SportServiceId { get; set; }
    public DbSportService SportService { get; set; }

    /// <summary>
    /// Итоговая стоимость.
    /// </summary>
    public decimal Amount { get; set; }
    
    /// <summary>
    /// Валюта.
    /// </summary>
    public EIsoCurrency Currency { get; set; }
    
    /// <summary>
    /// Действует с этой даты.
    /// </summary>
    public DateOnly ValidFrom { get; set; }
    
    /// <summary>
    /// Действует по эту дату.
    /// </summary>
    public DateOnly? ValidTo { get; set; }
}