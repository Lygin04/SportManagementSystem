using SportManagementSystem.BuildingBlocks;
using SportManagementSystem.BuildingBlocks.Abstractions;
using SportManagementSystem.Modules.Scheduling.Domain.Entities;
using SportManagementSystem.Modules.Scheduling.Domain.Repositories;

namespace SportManagementSystem.Modules.Scheduling.Application.Queries.GetTrainingSession;

public class GetTrainingSessionHandler(
    ITrainingSessionRepository trainingSessionRepository) : IMessageHandler<GetTrainingSessionMessage, MbResult<DbTrainingSession>>
{
    public async Task<MbResult<DbTrainingSession>> Handle(GetTrainingSessionMessage request, CancellationToken cancellationToken)
    {
        var result = await trainingSessionRepository.GetByIdAsync(request.Id, cancellationToken);

        if (result == null)
        {
            return MbResult<DbTrainingSession>.Failure(new MbError(
                title: "Training Session not found",
                status: StatusCodes.Status404NotFound,
                detail: "Тренировка не найдена."));
        }
        
        return MbResult<DbTrainingSession>.Success(result);
    }
}