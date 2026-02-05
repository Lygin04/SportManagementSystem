using SportManagementSystem.BuildingBlocks;
using SportManagementSystem.BuildingBlocks.Abstractions;
using SportManagementSystem.Modules.Users.Contracts.Response;
using SportManagementSystem.Modules.Users.Domain.Enums;
using SportManagementSystem.Modules.Users.Domain.Repositories;

namespace SportManagementSystem.Modules.Users.Application.Queries.GetUser;

public class GetUserHandler(
    IUserAccountRepository userAccountRepository,
    IClientRepository clientRepository,
    IStaffRepository staffRepository) : IMessageHandler<GetUserMessage, MbResult<GetUserResponse>>
{
    public async Task<MbResult<GetUserResponse>> Handle(GetUserMessage request, CancellationToken cancellationToken)
    {
        var userAccount = await userAccountRepository.GetByIdAsync(request.Id, cancellationToken);
        if (userAccount == null)
        {
            return MbResult<GetUserResponse>.Failure(new MbError(
                title: "User not found",
                status: StatusCodes.Status404NotFound,
                detail: "Пользователя не существует"));
        }

        var user = new GetUserResponse
        {
            Email = userAccount.Email
        };
        
        if (userAccount.Role == EUserRole.Client)
        {
            var client = await clientRepository.GetByIdAsync(userAccount.ClientId!.Value, cancellationToken);
            user.FirstName = client!.FirstName;
            user.LastName = client.LastName;
            user.BirthDate = client.BirthDate;
            user.Patronymic = client.Patronymic;
            user.Phone = client.Phone;
        }
        else
        {
            var staff = await staffRepository.GetByIdAsync(userAccount.ClientId!.Value, cancellationToken);
            user.FirstName = staff!.FirstName;
            user.LastName = staff.LastName;
            user.BirthDate = staff.BirthDate;
            user.Patronymic = staff.Patronymic;
            user.Phone = staff.Phone;
        }
        
        return MbResult<GetUserResponse>.Success(user);
    }
}