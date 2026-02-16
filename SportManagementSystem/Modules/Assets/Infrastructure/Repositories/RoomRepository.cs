using Microsoft.EntityFrameworkCore;
using SportManagementSystem.Data;
using SportManagementSystem.Modules.Assets.Domain.Entities;
using SportManagementSystem.Modules.Assets.Domain.Repositories;
using SportManagementSystem.Modules.Images.Domain.Entities;

namespace SportManagementSystem.Modules.Assets.Infrastructure.Repositories;

public class RoomRepository(ApplicationDbContext db) : IRoomRepository
{
    public async Task<DbRoom> CreateAsync(DbRoom entity, CancellationToken ct)
    {
        var room = await db.Rooms.AddAsync(entity, ct);
        await db.SaveChangesAsync(ct);
        return room.Entity;
    }

    public async Task DeleteAsync(long id, CancellationToken ct)
    {
        var room = new  DbRoom { Id = id };
        db.Rooms.Attach(room);
        db.Rooms.Remove(room);
        await db.SaveChangesAsync(ct);
    }

    public async Task<DbRoom?> GetByIdAsync(long id, CancellationToken ct)
    {
        return await db.Rooms
            .Include(r => r.Images)
            .FirstOrDefaultAsync(r => r.Id == id, ct);
    }

    public async Task AddImage(DbRoom room, Guid imageId)
    {
        var trackedImage = db.Images.Local.FirstOrDefault(i => i.Id == imageId);
        if (trackedImage is null)
        {
            trackedImage = new DbImage { Id = imageId };
            db.Images.Attach(trackedImage);
        }

        room.Images.Add(trackedImage);
        await db.SaveChangesAsync();
    }

    public async Task<List<DbRoom>?> GetByBranchAsync(long branchId, CancellationToken cancellationToken)
    {
        return await db.Rooms
            .Where(r => r.BranchId == branchId)
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> ExistsAsync(long id, CancellationToken ct)
    {
        return await db.Rooms.AnyAsync(e => e.Id == id);
    }
}
