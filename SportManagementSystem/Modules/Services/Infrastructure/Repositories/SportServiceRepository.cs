using Microsoft.EntityFrameworkCore;
using SportManagementSystem.Data;
using SportManagementSystem.Modules.Services.Domain.Entities;
using SportManagementSystem.Modules.Services.Domain.Repositories;

namespace SportManagementSystem.Modules.Services.Infrastructure.Repositories;

public class SportServiceRepository(ApplicationDbContext db) : ISportServiceRepository
{
    public async Task<DbSportService> CreateAsync(DbSportService entity, CancellationToken ct)
    {
        var sportService = await db.SportServices.AddAsync(entity, ct);
        await db.SaveChangesAsync(ct);
        return sportService.Entity;
    }

    public async Task DeleteAsync(long id, CancellationToken ct)
    {
        var sportService = new DbSportService { Id = id };
        db.SportServices.Attach(sportService);
        db.SportServices.Remove(sportService);
        await db.SaveChangesAsync(ct);
    }

    public async Task<DbSportService?> GetByIdAsync(long id, CancellationToken ct)
    {
        return await db.SportServices.FindAsync(id, ct);
    }

    public async Task<bool> ExistsAsync(long id, CancellationToken ct)
    {
        return await db.SportServices.AnyAsync(s => s.Id == id, ct);
    }
}