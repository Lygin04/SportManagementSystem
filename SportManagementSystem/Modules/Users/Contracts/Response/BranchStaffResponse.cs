using SportManagementSystem.Modules.Users.Domain.Enums;

namespace SportManagementSystem.Modules.Users.Contracts.Response;

public class BranchStaffResponse
{
    public long StaffId { get; set; }
    public string Email { get; set; } = string.Empty;
    public EUserRole Role { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string? Patronymic { get; set; }
    public DateOnly? BirthDate { get; set; }
    public string? Phone { get; set; }
    public Guid? AvatarId { get; set; }
}
