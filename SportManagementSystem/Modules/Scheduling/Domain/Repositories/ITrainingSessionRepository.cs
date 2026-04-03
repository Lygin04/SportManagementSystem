using SportManagementSystem.Modules.Scheduling.Domain.Entities;
using SportManagementSystem.Repositories.Abstract;

namespace SportManagementSystem.Modules.Scheduling.Domain.Repositories;

public interface ITrainingSessionRepository : IBaseRepository<DbTrainingSession>,
    IExistsByIdRepository<DbTrainingSession>
{
    Task<List<DbTrainingSession>> GetByBranchIdAsync(long branchId, CancellationToken ct);
    Task<int> MarkDoneAsync(CancellationToken ct);
}
