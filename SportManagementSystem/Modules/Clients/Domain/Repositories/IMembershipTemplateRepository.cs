using SportManagementSystem.Modules.Clients.Domain.Entities;
using SportManagementSystem.Repositories.Abstract;

namespace SportManagementSystem.Modules.Clients.Domain.Repositories;

public interface IMembershipTemplateRepository : IBaseRepository<DbMembershipTemplate>, IExistsByIdRepository<long>
{
    Task<List<DbMembershipTemplate>> GetByBranchAsync(long branchId, CancellationToken ct);
}
