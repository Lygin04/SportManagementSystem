using SportManagementSystem.Entities.Enums;

namespace SportManagementSystem.Entities;

public class DbSportService
{
    public long Id { get; set; }
    public string Code { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public EServiceCategory? Category { get; set; }
    public bool IsActive { get; set; }
    
    public DateTime Created { get; set; }
    public DateTime? Modified { get; set; }
    
    public ICollection<DbServicePrice> Prices { get; set; }
}