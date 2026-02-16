using SportManagementSystem.BuildingBlocks;
using SportManagementSystem.BuildingBlocks.Abstractions;
using SportManagementSystem.Modules.Users.Domain.Entities;
using SportManagementSystem.Modules.Users.Domain.Repositories;

namespace SportManagementSystem.Modules.Users.Application.Queries.GetStaff;

public class GetStaffHandler(
    IStaffRepository staffRepository) : IMessageHandler<GetStaffMessage, MbResult<DbStaff>>
{
    public async Task<MbResult<DbStaff>> Handle(GetStaffMessage request, CancellationToken cancellationToken)
    {
        var staff = await staffRepository.GetByIdAsync(request.Id, cancellationToken);

        if (staff == null)
        {
            return MbResult<DbStaff>.Failure(new MbError(
                title: "Staff not found",
                status: StatusCodes.Status404NotFound,
                detail: "Сотрудник не найден"));
        }
        
        return MbResult<DbStaff>.Success(staff);
    }
}