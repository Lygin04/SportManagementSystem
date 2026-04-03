using SportManagementSystem.Modules.Clients.Domain.Entities;
using SportManagementSystem.Repositories.Abstract;

namespace SportManagementSystem.Modules.Clients.Domain.Repositories;

public interface IMembershipRepository : IBaseRepository<DbMembership>
{
    Task<List<DbMembership>> GetByClientIdAsync(long clientId, CancellationToken ct);
}
