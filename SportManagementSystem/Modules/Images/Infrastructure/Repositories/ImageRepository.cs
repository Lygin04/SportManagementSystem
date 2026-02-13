using Microsoft.EntityFrameworkCore;
using SportManagementSystem.Data;
using SportManagementSystem.Modules.Images.Domain.Entities;
using SportManagementSystem.Modules.Images.Domain.Repositories;

namespace SportManagementSystem.Modules.Images.Infrastructure.Repositories;

public class ImageRepository(ApplicationDbContext db) : IImageRepository
{
    public async Task<DbImage> CreateAsync(DbImage entity, CancellationToken ct)
    {
        var result = await db.Images.AddAsync(entity, ct);
        await db.SaveChangesAsync(ct);
        return result.Entity;
    }

    public async Task<DbImage?> GetByIdAsync(Guid id, CancellationToken ct)
    {
        return await db.Images.FirstOrDefaultAsync(x => x.Id == id, ct);
    }

    public async Task<bool> ExistsAsync(Guid id, CancellationToken ct)
    {
        return await db.Images.AnyAsync(x => x.Id == id, ct);
    }

    public async Task DeleteAsync(Guid id, CancellationToken ct)
    {
        var tracked = db.Images.Local.FirstOrDefault(x => x.Id == id);
        if (tracked is not null)
        {
            db.Images.Remove(tracked);
        }
        else
        {
            var entity = new DbImage { Id = id };
            db.Images.Attach(entity);
            db.Images.Remove(entity);
        }

        await db.SaveChangesAsync(ct);
    }
}
