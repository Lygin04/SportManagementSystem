using SportManagementSystem.Modules.Services.Domain.Entities;
using SportManagementSystem.Repositories.Abstract;

namespace SportManagementSystem.Modules.Services.Domain.Repositories;

public interface ISportServiceRepository : IBaseRepository<DbSportService>, IExistsByIdRepository<DbSportService>
{
    
}