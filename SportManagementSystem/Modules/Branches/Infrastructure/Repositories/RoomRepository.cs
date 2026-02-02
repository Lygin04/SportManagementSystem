using Microsoft.EntityFrameworkCore;
using SportManagementSystem.Data;
using SportManagementSystem.Modules.Branches.Domain.Entities;
using SportManagementSystem.Modules.Branches.Domain.Repositories;

namespace SportManagementSystem.Modules.Branches.Infrastructure.Repositories;

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
        return await db.Rooms.FindAsync(id, ct);
    }

    public async Task<List<DbRoom>?> GetByBranchAsync(long branchId, CancellationToken cancellationToken)
    {
        return await db.Rooms
            .Where(r => r.BranchId == branchId)
            .ToListAsync(cancellationToken);
    }
}