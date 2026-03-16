using Microsoft.EntityFrameworkCore;
using SportManagementSystem.Data;
using SportManagementSystem.Modules.Assets.Domain.Entities;
using SportManagementSystem.Modules.Assets.Domain.Repositories;
using SportManagementSystem.Modules.Images.Domain.Entities;

namespace SportManagementSystem.Modules.Assets.Infrastructure.Repositories;

public class EquipmentRepository(ApplicationDbContext db) : IEquipmentRepository
{
    public async Task<List<DbEquipment>> GetByRoomAsync(long roomId, CancellationToken ct)
    {
        return await db.Equipments
            .Where(e => e.RoomId == roomId)
            .Include(e => e.Images)
            .ToListAsync(ct);
    }

    public async Task<DbEquipment> CreateAsync(DbEquipment entity, CancellationToken ct)
    {
        var equipment = await db.Equipments.AddAsync(entity, ct);
        await db.SaveChangesAsync(ct);
        return equipment.Entity;
    }

    public async Task DeleteAsync(long id, CancellationToken ct)
    {
        var equipment = new  DbEquipment { Id = id };
        db.Equipments.Attach(equipment);
        db.Equipments.Remove(equipment);
        await db.SaveChangesAsync(ct);
    }

    public async Task<DbEquipment?> GetByIdAsync(long id, CancellationToken ct)
    {
        return await db.Equipments
            .Include(e => e.Images)
            .FirstOrDefaultAsync(e => e.Id == id, ct);
    }

    public async Task AddImage(DbEquipment equipment, Guid imageId)
    {
        var trackedImage = db.Images.Local.FirstOrDefault(i => i.Id == imageId);
        if (trackedImage is null)
        {
            trackedImage = new DbImage { Id = imageId };
            db.Images.Attach(trackedImage);
        }

        equipment.Images.Add(trackedImage);
        await db.SaveChangesAsync();
    }

    public async Task<bool> ExistsAsync(long id, CancellationToken ct)
    {
        return await db.Equipments.AnyAsync(e => e.Id == id, ct);
    }
}
