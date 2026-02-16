using SportManagementSystem.Modules.Assets.Domain.Entities;
using SportManagementSystem.Modules.Images.Domain.Entities;
using SportManagementSystem.Repositories.Abstract;

namespace SportManagementSystem.Modules.Assets.Domain.Repositories;

public interface IBranchRepository : IBaseRepository<DbBranch>, IExistsByIdRepository<DbBranch>, IGetAllRepository<DbBranch>
{
    Task AddImage(DbBranch branch, Guid imageId);
}