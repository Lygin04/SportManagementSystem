using SportManagementSystem.Modules.Images.Domain.Entities;

namespace SportManagementSystem.Modules.Images.Domain.Repositories;

public interface IImageRepository
{
    Task<DbImage> CreateAsync(DbImage entity, CancellationToken ct);
    Task<DbImage?> GetByIdAsync(Guid id, CancellationToken ct);
    Task<bool> ExistsAsync(Guid id, CancellationToken ct);
    Task DeleteAsync(Guid id, CancellationToken ct);
}
