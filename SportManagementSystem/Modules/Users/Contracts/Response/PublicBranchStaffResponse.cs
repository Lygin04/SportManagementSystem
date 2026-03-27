using SportManagementSystem.Modules.Users.Domain.Enums;

namespace SportManagementSystem.Modules.Users.Contracts.Response;

public class PublicBranchStaffResponse
{
    public long StaffId { get; set; }
    public EUserRole Role { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string? Patronymic { get; set; }
    public Guid? AvatarId { get; set; }
}
