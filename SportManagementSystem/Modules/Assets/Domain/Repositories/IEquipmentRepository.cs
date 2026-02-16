using SportManagementSystem.Modules.Assets.Domain.Entities;
using SportManagementSystem.Repositories.Abstract;

namespace SportManagementSystem.Modules.Assets.Domain.Repositories;

public interface IEquipmentRepository : IBaseRepository<DbEquipment>
{
    Task AddImage(DbEquipment branch, Guid imageId);
}