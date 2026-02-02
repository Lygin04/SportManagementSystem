using SportManagementSystem.Modules.Branches.Domain.Entities;
using SportManagementSystem.Repositories.Abstract;

namespace SportManagementSystem.Modules.Branches.Domain.Repositories;

public interface IBranchRepository : IBaseRepository<DbBranch>
{
    Task<List<DbBranch>> GetAllAsync(CancellationToken ct);
}