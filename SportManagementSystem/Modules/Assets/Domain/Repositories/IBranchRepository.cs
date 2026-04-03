using SportManagementSystem.Modules.Assets.Domain.Entities;
using SportManagementSystem.Repositories.Abstract;

namespace SportManagementSystem.Modules.Assets.Domain.Repositories;

public interface IBranchRepository : IBaseRepository<DbBranch>, IExistsByIdRepository<DbBranch>, IGetAllRepository<DbBranch>
{
    Task AddImage(DbBranch branch, Guid imageId, CancellationToken ct);
    Task<List<DbBranch>> GetByAdmin(long adminId, CancellationToken ct);
    Task<List<DbBranch>> GetManagedByStaffAsync(long staffId, CancellationToken ct);
    Task<bool> AddStaffMemberAsync(long branchId, long staffId, bool asAdmin, CancellationToken ct);
    Task<bool> HasManagementAccessAsync(long branchId, long staffId, CancellationToken ct);
}
