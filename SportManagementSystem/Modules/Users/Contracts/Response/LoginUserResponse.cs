namespace SportManagementSystem.Modules.Users.Contracts.Response;

public class LoginUserResponse
{
    /// <summary>
    /// Токен аутентификации.
    /// </summary>
    public string Token { get; set; }

    /// <summary>
    /// Время истечения токена.
    /// </summary>
    public DateTime Expires { get; set; }
}