using SportManagementSystem.BuildingBlocks;
using SportManagementSystem.BuildingBlocks.Abstractions;
using SportManagementSystem.Modules.Scheduling.Domain.Entities;
using SportManagementSystem.Modules.Scheduling.Domain.Repositories;

namespace SportManagementSystem.Modules.Scheduling.Application.Queries.GetServiceSchedulesByBranch;

public class GetServiceSchedulesByBranchHandler(
    IServiceScheduleRepository serviceScheduleRepository)
    : IMessageHandler<GetServiceSchedulesByBranchMessage, MbResult<List<DbServiceSchedule>>>
{
    public async Task<MbResult<List<DbServiceSchedule>>> Handle(
        GetServiceSchedulesByBranchMessage request,
        CancellationToken cancellationToken)
    {
        var schedules = await serviceScheduleRepository.GetByBranchIdAsync(request.BranchId, cancellationToken);
        return MbResult<List<DbServiceSchedule>>.Success(schedules);
    }
}
