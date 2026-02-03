using SportManagementSystem.Modules.Assets.Domain.Entities;
using SportManagementSystem.Repositories.Abstract;

namespace SportManagementSystem.Modules.Assets.Domain.Repositories;

public interface IRoomRepository : IBaseRepository<DbRoom>, IExistsByIdRepository<DbRoom>
{
    Task<List<DbRoom>?> GetByBranchAsync(long branchId, CancellationToken cancellationToken);
}