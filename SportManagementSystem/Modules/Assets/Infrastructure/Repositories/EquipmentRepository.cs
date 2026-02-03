using Microsoft.EntityFrameworkCore;
using SportManagementSystem.Data;
using SportManagementSystem.Modules.Assets.Domain.Entities;
using SportManagementSystem.Modules.Assets.Domain.Repositories;

namespace SportManagementSystem.Modules.Assets.Infrastructure.Repositories;

public class EquipmentRepository(ApplicationDbContext db) : IEquipmentRepository
{
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
        return await db.Equipments.FindAsync(id, ct);
    }

    public async Task<bool> ExistsAsync(long id, CancellationToken ct)
    {
        return await db.Equipments.AnyAsync(e => e.Id == id, ct);
    }
}