using SportManagementSystem.Modules.Users.Domain.Entities;
using SportManagementSystem.Repositories.Abstract;

namespace SportManagementSystem.Modules.Users.Domain.Repositories;

public interface IUserAccountRepository : IBaseRepository<DbUserAccount>
{
    Task<DbUserAccount?> GetByEmailAsync(string email, CancellationToken ct);
    Task<DbUserAccount?> GetByClientIdAsync(long clientId, CancellationToken ct);
    Task<bool> ExistsEmailAsync(string email, CancellationToken ct);
    Task UpdateLastLoginDateAsync(long id, CancellationToken ct);
}
