using SportManagementSystem.Modules.Services.Domain.Enums;

namespace SportManagementSystem.Modules.Services.Contracts.Requests;

public class CreateSportServiceRequest
{
    // <summary>
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
}