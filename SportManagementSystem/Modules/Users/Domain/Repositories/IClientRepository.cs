using SportManagementSystem.Modules.Users.Domain.Entities;
using SportManagementSystem.Repositories.Abstract;

namespace SportManagementSystem.Modules.Users.Domain.Repositories;

public interface IClientRepository : IBaseRepository<DbClient>, IExistsByIdRepository<long>
{
}