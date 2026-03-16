using SportManagementSystem.Modules.Services.Domain.Entities;
using SportManagementSystem.Repositories.Abstract;

namespace SportManagementSystem.Modules.Services.Domain.Repositories;

public interface ISportServiceRepository : IBaseRepository<DbSportService>, IExistsByIdRepository<DbSportService>
{
    Task<List<DbSportService>> GetByBranchAsync(long branchId, CancellationToken ct);
    Task<bool> UpdateActive(long id, bool isActive, CancellationToken ct);
}
