using Microsoft.EntityFrameworkCore;
using SportManagementSystem.BuildingBlocks;
using SportManagementSystem.BuildingBlocks.Abstractions;
using SportManagementSystem.Data;
using SportManagementSystem.Modules.Users.Contracts.Response;

namespace SportManagementSystem.Modules.Users.Application.Queries.GetBranchStaffs;

public class GetBranchStaffsHandler(ApplicationDbContext db)
    : IMessageHandler<GetBranchStaffsMessage, MbResult<List<BranchStaffResponse>>>
{
    public async Task<MbResult<List<BranchStaffResponse>>> Handle(
        GetBranchStaffsMessage request,
        CancellationToken cancellationToken)
    {
        var staffs = await db.UserAccounts
            .AsNoTracking()
            .Where(account =>
                account.StaffId != null &&
                account.Staff != null &&
                (account.Staff.AdminBranches.Any(branch => branch.Id == request.BranchId) ||
                    account.Staff.StaffBranches.Any(branch => branch.Id == request.BranchId)))
            .OrderBy(account => account.Staff!.LastName)
            .ThenBy(account => account.Staff!.FirstName)
            .Select(account => new BranchStaffResponse
            {
                StaffId = account.StaffId!.Value,
                Email = account.Email,
                Role = account.Role,
                FirstName = account.Staff!.FirstName,
                LastName = account.Staff.LastName,
                Patronymic = account.Staff.Patronymic,
                BirthDate = account.Staff.BirthDate,
                Phone = account.Staff.Phone,
                AvatarId = account.Staff.AvatarId,
            })
            .ToListAsync(cancellationToken);

        return MbResult<List<BranchStaffResponse>>.Success(staffs);
    }
}
