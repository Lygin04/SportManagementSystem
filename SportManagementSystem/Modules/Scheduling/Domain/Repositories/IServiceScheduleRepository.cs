using SportManagementSystem.Modules.Scheduling.Domain.Entities;
using SportManagementSystem.Repositories.Abstract;

namespace SportManagementSystem.Modules.Scheduling.Domain.Repositories;

public interface IServiceScheduleRepository : 
    IBaseRepository<DbServiceSchedule>,
    IExistsByIdRepository<DbServiceSchedule>
{
    Task<List<DbServiceSchedule>> GetByBranchIdAsync(long branchId, CancellationToken ct);
}
