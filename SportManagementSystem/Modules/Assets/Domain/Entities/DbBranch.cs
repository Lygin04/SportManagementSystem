using SportManagementSystem.Modules.Images.Domain.Entities;
using SportManagementSystem.Modules.Users.Domain.Entities;
using SportManagementSystem.Modules.Services.Domain.Entities;

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
    
    // Main administrator who created this branch.
    public long AdminId { get; set; }
    public DbStaff Admin { get; set; } = null!;

    // Additional administrators assigned to this branch.
    public ICollection<DbStaff> BranchAdmins { get; set; } = new List<DbStaff>();

    // Employees working in this branch.
    public ICollection<DbStaff> BranchStaffs { get; set; } = new List<DbStaff>();
    
    /// <summary>
    /// Помещения, принадлежащие филиалу.
    /// </summary>
    public ICollection<DbRoom> Rooms { get; set; } = new List<DbRoom>();

    public ICollection<DbSportService> SportServices { get; set; } = new List<DbSportService>();
    
    /// <summary>
    /// Фотографии спортивной организации.
    /// </summary>
    public ICollection<DbImage> Images { get; set; } = new List<DbImage>();
}
