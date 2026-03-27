using SportManagementSystem.BuildingBlocks;
using SportManagementSystem.BuildingBlocks.Abstractions;
using SportManagementSystem.Modules.Users.Contracts.Response;
using SportManagementSystem.Modules.Users.Domain.Repositories;

namespace SportManagementSystem.Modules.Users.Application.Queries.GetClientLookup;

public class GetClientLookupHandler(
    IUserAccountRepository userAccountRepository,
    IClientRepository clientRepository) : IMessageHandler<GetClientLookupMessage, MbResult<GetUserResponse>>
{
    public async Task<MbResult<GetUserResponse>> Handle(GetClientLookupMessage request, CancellationToken cancellationToken)
    {
        var client = await clientRepository.GetByIdAsync(request.ClientId, cancellationToken);
        if (client == null)
        {
            return MbResult<GetUserResponse>.Failure(new MbError(
                title: "Client not found",
                status: StatusCodes.Status404NotFound,
                detail: "Клиента не существует"));
        }

        var userAccount = await userAccountRepository.GetByClientIdAsync(request.ClientId, cancellationToken);
        if (userAccount == null)
        {
            return MbResult<GetUserResponse>.Failure(new MbError(
                title: "User not found",
                status: StatusCodes.Status404NotFound,
                detail: "Учетная запись клиента не найдена"));
        }

        var user = new GetUserResponse
        {
            Email = userAccount.Email,
            FirstName = client.FirstName,
            LastName = client.LastName,
            BirthDate = client.BirthDate,
            Patronymic = client.Patronymic,
            Phone = client.Phone
        };

        if (client.AvatarId != null)
        {
            user.AvatarId = client.AvatarId.Value;
        }

        return MbResult<GetUserResponse>.Success(user);
    }
}
