using SportManagementSystem.BuildingBlocks;
using SportManagementSystem.BuildingBlocks.Abstractions;
using SportManagementSystem.Modules.Scheduling.Domain.Entities;
using SportManagementSystem.Modules.Scheduling.Domain.Repositories;

namespace SportManagementSystem.Modules.Scheduling.Application.Queries.GetTrainingSessionsByBranch;

public class GetTrainingSessionsByBranchHandler(
    ITrainingSessionRepository trainingSessionRepository)
    : IMessageHandler<GetTrainingSessionsByBranchMessage, MbResult<List<DbTrainingSession>>>
{
    public async Task<MbResult<List<DbTrainingSession>>> Handle(
        GetTrainingSessionsByBranchMessage request,
        CancellationToken cancellationToken)
    {
        var sessions = await trainingSessionRepository.GetByBranchIdAsync(request.BranchId, cancellationToken);
        return MbResult<List<DbTrainingSession>>.Success(sessions);
    }
}
