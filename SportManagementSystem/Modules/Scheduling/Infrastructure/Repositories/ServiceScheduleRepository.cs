using Microsoft.EntityFrameworkCore;
using SportManagementSystem.Data;
using SportManagementSystem.Modules.Scheduling.Domain.Entities;
using SportManagementSystem.Modules.Scheduling.Domain.Repositories;

namespace SportManagementSystem.Modules.Scheduling.Infrastructure.Repositories;

public class ServiceScheduleRepository(ApplicationDbContext db) : IServiceScheduleRepository
{
    public async Task<DbServiceSchedule> CreateAsync(DbServiceSchedule entity, CancellationToken ct)
    {
        var serviceSchedule = await db.ServiceSchedules.AddAsync(entity, ct);
        await db.SaveChangesAsync(ct);
        return serviceSchedule.Entity;
    }

    public async Task DeleteAsync(long id, CancellationToken ct)
    {
        var serviceSchedule = new DbServiceSchedule { Id = id };
        db.ServiceSchedules.Attach(serviceSchedule);
        db.ServiceSchedules.Remove(serviceSchedule);
        await db.SaveChangesAsync(ct);
    }

    public async Task<DbServiceSchedule?> GetByIdAsync(long id, CancellationToken ct)
    {
        return await db.ServiceSchedules.FindAsync(id, ct);
    }

    public async Task<bool> ExistsAsync(long id, CancellationToken ct)
    {
        return await db.ServiceSchedules.AnyAsync(c => c.Id == id, ct);
    }
}