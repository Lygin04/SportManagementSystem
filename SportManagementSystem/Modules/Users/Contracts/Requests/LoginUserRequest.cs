namespace SportManagementSystem.Modules.Users.Contracts.Requests;

public class LoginUserRequest
{
    public string Email { get; set; }
    public string Password { get; set; }
}