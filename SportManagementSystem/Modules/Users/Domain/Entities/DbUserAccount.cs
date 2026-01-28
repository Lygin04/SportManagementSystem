using SportManagementSystem.Entities.Enums;

namespace SportManagementSystem.Modules.Users.Domain.Entities;

public class DbUserAccount
{
    public long Id { get; set; }
    public string Email { get; set; }
    public string PasswordHash { get; set; }
    public EUserRole Role { get; set; }
    public EAccountStatus Status { get; set; }

    public long? StaffId { get; set; }
    public DbStaff? Staff { get; set; }
    
    public long? ClientId { get; set; }
    public DbClient? Client { get; set; }
    
    public DateTime Created { get; set; }
    public DateTime? Modified { get; set; }
    public DateTime? LastLogin { get; set; }
}