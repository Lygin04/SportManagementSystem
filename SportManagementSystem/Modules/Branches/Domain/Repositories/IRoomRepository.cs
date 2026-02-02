using SportManagementSystem.Modules.Branches.Domain.Entities;
using SportManagementSystem.Repositories.Abstract;

namespace SportManagementSystem.Modules.Branches.Domain.Repositories;

public interface IRoomRepository : IBaseRepository<DbRoom>
{
    Task<List<DbRoom>?> GetByBranchAsync(long branchId, CancellationToken cancellationToken);
}