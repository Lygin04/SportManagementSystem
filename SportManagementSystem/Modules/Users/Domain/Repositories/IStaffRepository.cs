using SportManagementSystem.Modules.Users.Domain.Entities;
using SportManagementSystem.Repositories.Abstract;

namespace SportManagementSystem.Modules.Users.Domain.Repositories;

public interface IStaffRepository : IBaseRepository<DbStaff>, IExistsByIdRepository<DbStaff>
{
    Task<bool> SetImageIdAsync(long staffId, Guid? imageId, CancellationToken ct);
}