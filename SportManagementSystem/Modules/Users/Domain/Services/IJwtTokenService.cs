using System.Security.Claims;
using SportManagementSystem.Modules.Users.Contracts.Response;

namespace SportManagementSystem.Modules.Users.Domain.Services;

/// <summary>
/// Интерфейс сервиса генерации JWT токенов.
/// </summary>
public interface IJwtTokenService
{
    /// <summary>
    /// Создает JWT токен на основе предоставленных утверждений (claims) асинхронно.
    /// </summary>
    /// <param name="claims">Коллекция утверждений, которые будут включены в токен.</param>
    /// <returns>DTO (Data Transfer Object) токена аутентификации.</returns>
    Task<LoginUserResponse> CreateAccessTokenAsync(ICollection<Claim> claims);
}