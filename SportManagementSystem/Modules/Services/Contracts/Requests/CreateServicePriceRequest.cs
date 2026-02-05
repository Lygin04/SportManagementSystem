using SportManagementSystem.Modules.Services.Domain.Enums;

namespace SportManagementSystem.Modules.Services.Contracts.Requests;

public class CreateServicePriceRequest
{
    public long SportServiceId { get; set; }

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