using System.Security.Claims;
using SportManagementSystem.BuildingBlocks;
using SportManagementSystem.BuildingBlocks.Abstractions;
using SportManagementSystem.BuildingBlocks.Authentication.Hash.Interfaces;
using SportManagementSystem.Modules.Users.Contracts.Response;
using SportManagementSystem.Modules.Users.Domain.Enums;
using SportManagementSystem.Modules.Users.Domain.Repositories;
using SportManagementSystem.Modules.Users.Domain.Services;

namespace SportManagementSystem.Modules.Users.Application.Commands.LoginUser;

public class LoginUserHandler(
    IJwtTokenService jwtService,
    IPasswordHasher passwordHasher,
    IUserAccountRepository userAccountRepository) : IMessageHandler<LoginUserMessage, MbResult<LoginUserResponse>>
{
    public async Task<MbResult<LoginUserResponse>> Handle(LoginUserMessage request, CancellationToken cancellationToken)
    {
        var candidate = await userAccountRepository.GetByEmailAsync(request.Request.Email, cancellationToken);
        
        if (candidate == null || !passwordHasher.Verify(request.Request.Password, candidate.PasswordHash))
        {
            return MbResult<LoginUserResponse>.Failure(new MbError(
                title: "Invalid credentials",
                status: StatusCodes.Status401Unauthorized,
                detail: "Неверная почта или пароль."));;
        }

        if (candidate.Status != EAccountStatus.Active)
        {
            return MbResult<LoginUserResponse>.Failure(new MbError(
                title: "Account is not active",
                status: StatusCodes.Status409Conflict,
                detail: "Аккаунт удален или заблокирован"));
        }
        
        candidate.LastLogin = DateTime.UtcNow;
        await userAccountRepository.UpdateLastLoginDateAsync(candidate.Id, cancellationToken);

        var response = await jwtService.CreateAccessTokenAsync(new List<Claim>
        {
            new("email", request.Request.Email),
            new("role", candidate.Role.ToString()),
            new("id", candidate.Id.ToString())
        });

        return MbResult<LoginUserResponse>.Success(response);
    }
}