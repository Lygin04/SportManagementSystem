using MediatR;
using SportManagementSystem.BuildingBlocks;
using SportManagementSystem.BuildingBlocks.Abstractions;
using SportManagementSystem.BuildingBlocks.Authentication.Hash.Interfaces;
using SportManagementSystem.BuildingBlocks.Time;
using SportManagementSystem.Modules.Assets.Domain.Repositories;
using SportManagementSystem.Modules.Users.Domain.Entities;
using SportManagementSystem.Modules.Users.Domain.Enums;
using SportManagementSystem.Modules.Users.Domain.Repositories;

namespace SportManagementSystem.Modules.Users.Application.Commands.RegisterBranchStaff;

public class RegisterBranchStaffHandler(
    IAppClock clock,
    IUserAccountRepository userAccountRepository,
    IStaffRepository staffRepository,
    IBranchRepository branchRepository,
    IPasswordHasher passwordHasher) : IMessageHandler<RegisterBranchStaffMessage, MbResult<Unit>>
{
    public async Task<MbResult<Unit>> Handle(RegisterBranchStaffMessage request, CancellationToken cancellationToken)
    {
        if (!await branchRepository.ExistsAsync(request.BranchId, cancellationToken))
        {
            return MbResult<Unit>.Failure(new MbError(
                title: "Branch not found",
                status: StatusCodes.Status404NotFound,
                detail: "Филиал не найден."));
        }

        if (request.Request.Role == EUserRole.Client)
        {
            return MbResult<Unit>.Failure(new MbError(
                title: "Invalid role",
                status: StatusCodes.Status400BadRequest,
                detail: "Клиента нельзя регистрировать как сотрудника филиала."));
        }

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
            Role = request.Request.Role,
            Status = EAccountStatus.Active,
            StaffId = staff.Id,
            Created = clock.UtcNow,
        };

        await userAccountRepository.CreateAsync(userAccount, cancellationToken);

        var attached = await branchRepository.AddStaffMemberAsync(
            request.BranchId,
            staff.Id,
            request.Request.Role == EUserRole.Admin,
            cancellationToken);

        if (!attached)
        {
            return MbResult<Unit>.Failure(new MbError(
                title: "Unable to assign staff",
                status: StatusCodes.Status500InternalServerError,
                detail: "Не удалось привязать сотрудника к филиалу."));
        }

        return MbResult<Unit>.Success(Unit.Value);
    }
}
