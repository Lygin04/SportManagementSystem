using SportManagementSystem.Modules.Scheduling.Domain.Repositories;

namespace SportManagementSystem.Modules.Scheduling.Application.Dispatchers;

public class TrainingSessionStatusDispatcher(ITrainingSessionRepository trainingSessionRepository)
{
    public async Task DispatchAsync()
    {
        await trainingSessionRepository.MarkDoneAsync(CancellationToken.None);
    }
}
