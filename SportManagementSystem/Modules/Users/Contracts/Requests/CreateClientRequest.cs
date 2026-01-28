namespace SportManagementSystem.Modules.Users.Contracts.Requests;

public class CreateClientRequest
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string? Patronymic { get; set; }
    public DateOnly? BirthDate { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
}