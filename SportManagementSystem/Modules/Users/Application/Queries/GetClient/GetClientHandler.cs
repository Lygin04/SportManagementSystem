using SportManagementSystem.BuildingBlocks;
using SportManagementSystem.BuildingBlocks.Abstractions;
using SportManagementSystem.Modules.Users.Contracts.Response;
using SportManagementSystem.Modules.Users.Domain.Enums;
using SportManagementSystem.Modules.Users.Domain.Repositories;

namespace SportManagementSystem.Modules.Users.Application.Queries.GetClient;

public class GetClientHandler(
    IUserAccountRepository userAccountRepository,
    IClientRepository clientRepository,
    IStaffRepository staffRepository) : IMessageHandler<GetClientMessage, MbResult<GetUserResponse>>
{
    public async Task<MbResult<GetUserResponse>> Handle(GetClientMessage request, CancellationToken cancellationToken)
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
            
            if(client.AvatarId != null)
                user.AvatarId = client.AvatarId.Value;
        }
        else
        {
            var staff = await staffRepository.GetByIdAsync(userAccount.ClientId!.Value, cancellationToken);
            user.FirstName = staff!.FirstName;
            user.LastName = staff.LastName;
            user.BirthDate = staff.BirthDate;
            user.Patronymic = staff.Patronymic;
            user.Phone = staff.Phone;
            
            if(staff.AvatarId != null)
                user.AvatarId = staff.AvatarId.Value;
        }
        
        return MbResult<GetUserResponse>.Success(user);
    }
}