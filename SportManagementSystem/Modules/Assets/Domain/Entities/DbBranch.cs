using SportManagementSystem.Modules.Images.Domain.Entities;

namespace SportManagementSystem.Modules.Assets.Domain.Entities;

/// <summary>
/// Филиал спортивной организации.
/// </summary>
public class DbBranch
{
    /// <summary>
    /// Уникальный индетификатор филиала.
    /// </summary>
    public long Id { get; set; }
    
    /// <summary>
    /// Название Филиала.
    /// </summary>
    public string Name { get; set; } = string.Empty;
    
    /// <summary>
    /// Физический адрес.
    /// </summary>
    public string? Address { get; set; }
    
    /// <summary>
    /// Географическая широта.
    /// </summary>
    public double? Latitude { get; set; }
    
    /// <summary>
    /// Географическая долгота.
    /// </summary>
    public double? Longitude { get; set; }
    
    /// <summary>
    /// Помещения, принадлежащие филиалу.
    /// </summary>
    public ICollection<DbRoom> Rooms { get; set; } = new List<DbRoom>();
    
    /// <summary>
    /// Фотографии спортивной организации.
    /// </summary>
    public ICollection<DbImage> Images { get; set; } = new List<DbImage>();
}