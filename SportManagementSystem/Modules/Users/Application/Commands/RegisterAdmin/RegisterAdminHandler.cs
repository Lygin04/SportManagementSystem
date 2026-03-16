using MediatR;
using SportManagementSystem.BuildingBlocks;
using SportManagementSystem.BuildingBlocks.Abstractions;
using SportManagementSystem.BuildingBlocks.Authentication.Hash.Interfaces;
using SportManagementSystem.Modules.Users.Domain.Entities;
using SportManagementSystem.Modules.Users.Domain.Enums;
using SportManagementSystem.Modules.Users.Domain.Repositories;

namespace SportManagementSystem.Modules.Users.Application.Commands.RegisterAdmin;

public class RegisterAdminHandler(
    IUserAccountRepository userAccountRepository,
    IStaffRepository staffRepository,
    IPasswordHasher passwordHasher) : IMessageHandler<RegisterAdminMessage, MbResult<Unit>>
{
    public async Task<MbResult<Unit>> Handle(RegisterAdminMessage request, CancellationToken cancellationToken)
    {
        if (await userAccountRepository.ExistsEmailAsync(request.Request.Email, cancellationToken))
        {
            return MbResult<Unit>.Failure(new MbError(
                title: "Email already exists",
                status: StatusCodes.Status409Conflict,
                detail: "Аккаунт с такой почтой уже существует."));
        }
        
        var staff = new DbStaff
        {
            FirstName = request.Request.FirstName,
            LastName = request.Request.LastName,
            Patronymic = request.Request.Patronymic,
            BirthDate = request.Request.BirthDate,
            Phone = request.Request.Phone,
        };
        
        staff = await staffRepository.CreateAsync(staff, cancellationToken);
        
        var userAccount = new DbUserAccount
        {
            Email = request.Request.Email,
            PasswordHash = passwordHasher.Hash(request.Request.Password),
            Role = EUserRole.Admin,
            Status = EAccountStatus.Active,
            StaffId = staff.Id,
            Created = DateTime.UtcNow,
        };

        await userAccountRepository.CreateAsync(userAccount, cancellationToken);

        return MbResult<Unit>.Success(Unit.Value);
    }
}