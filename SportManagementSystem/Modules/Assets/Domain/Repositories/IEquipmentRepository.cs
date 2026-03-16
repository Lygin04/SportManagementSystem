using SportManagementSystem.Modules.Assets.Domain.Entities;
using SportManagementSystem.Repositories.Abstract;

namespace SportManagementSystem.Modules.Assets.Domain.Repositories;

public interface IEquipmentRepository : IBaseRepository<DbEquipment>
{
    Task<List<DbEquipment>> GetByRoomAsync(long roomId, CancellationToken ct);
    Task AddImage(DbEquipment branch, Guid imageId);
}
