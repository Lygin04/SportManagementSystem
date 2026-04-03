using MediatR;
using SportManagementSystem.BuildingBlocks;
using SportManagementSystem.BuildingBlocks.Abstractions;
using SportManagementSystem.BuildingBlocks.Authentication.Hash.Interfaces;
using SportManagementSystem.BuildingBlocks.Time;
using SportManagementSystem.Modules.Users.Domain.Entities;
using SportManagementSystem.Modules.Users.Domain.Enums;
using SportManagementSystem.Modules.Users.Domain.Repositories;

namespace SportManagementSystem.Modules.Users.Application.Commands.RegisterClient;

public class RegisterClientHandler(
    IAppClock clock,
    IUserAccountRepository userAccountRepository,
    IClientRepository clientRepository,
    IPasswordHasher passwordHasher) : IMessageHandler<RegisterClientMessage, MbResult<Unit>>
{
    public async Task<MbResult<Unit>> Handle(RegisterClientMessage request, CancellationToken cancellationToken)
    {
        if (await userAccountRepository.ExistsEmailAsync(request.Request.Email, cancellationToken))
        {
            return MbResult<Unit>.Failure(new MbError(
                title: "Email already exists",
                status: StatusCodes.Status409Conflict,
                detail: "Аккаунт с такой почтой уже существует."));
        }
        
        var client = new DbClient
        {
            FirstName = request.Request.FirstName,
            LastName = request.Request.LastName,
            Patronymic = request.Request.Patronymic,
            BirthDate = request.Request.BirthDate,
            Phone = request.Request.Phone,
            RegisterDate = clock.UtcNow,
        };
        
        client = await clientRepository.CreateAsync(client, cancellationToken);
       
        var userAccount = new DbUserAccount
        {
            Email = request.Request.Email,
            PasswordHash = passwordHasher.Hash(request.Request.Password),
            Role = EUserRole.Client,
            Status = EAccountStatus.Active,
            ClientId = client.Id,
            Created = clock.UtcNow,
        };

        await userAccountRepository.CreateAsync(userAccount, cancellationToken);
       
        return MbResult<Unit>.Success(Unit.Value);
    }
}
