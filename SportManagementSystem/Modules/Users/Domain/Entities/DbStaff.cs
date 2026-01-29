namespace SportManagementSystem.Modules.Users.Domain.Entities;

public class DbStaff
{
    public long Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string? Patronymic { get; set; }
    public string Phone { get; set; }
    public DateOnly BirthDate { get; set; }
    public string? Role { get; set; }
}