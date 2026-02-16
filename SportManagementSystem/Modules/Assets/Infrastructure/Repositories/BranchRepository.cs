using Microsoft.EntityFrameworkCore;
using SportManagementSystem.Data;
using SportManagementSystem.Modules.Assets.Domain.Entities;
using SportManagementSystem.Modules.Assets.Domain.Repositories;
using SportManagementSystem.Modules.Images.Domain.Entities;

namespace SportManagementSystem.Modules.Assets.Infrastructure.Repositories;

public class BranchRepository(ApplicationDbContext db) : IBranchRepository
{
    public async Task<DbBranch> CreateAsync(DbBranch entity, CancellationToken ct)
    {
        var branch = await db.Branches.AddAsync(entity, ct);
        await db.SaveChangesAsync(ct);
        return branch.Entity;
    }

    public async Task DeleteAsync(long id, CancellationToken ct)
    {
        var branch = new DbBranch { Id = id };
        db.Branches.Attach(branch);
        db.Branches.Remove(branch);
        await db.SaveChangesAsync(ct);
    }

    public async Task<DbBranch?> GetByIdAsync(long id, CancellationToken ct)
    {
        return await db.Branches
            .Include(b => b.Images)
            .Include(b => b.Rooms)
            .FirstOrDefaultAsync(b => b.Id == id, ct);
    }

    public async Task<List<DbBranch>> GetAllAsync(CancellationToken ct)
    {
        return await db.Branches
            .Include(b => b.Images)
            .Include(r => r.Rooms)
            .ToListAsync(ct);
    }

    public async Task AddImage(DbBranch branch, Guid imageId)
    {
        var trackedImage = db.Images.Local.FirstOrDefault(i => i.Id == imageId);
        if (trackedImage is null)
        {
            trackedImage = new DbImage { Id = imageId };
            db.Images.Attach(trackedImage);
        }

        branch.Images.Add(trackedImage);
        await db.SaveChangesAsync();
    }

    public async Task<bool> ExistsAsync(long id, CancellationToken ct)
    {
        return db.Branches.Any(e => e.Id == id);
    }
}
