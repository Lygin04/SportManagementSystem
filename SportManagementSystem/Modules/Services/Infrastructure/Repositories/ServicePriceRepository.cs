using SportManagementSystem.Data;
using SportManagementSystem.Modules.Services.Domain.Entities;
using SportManagementSystem.Modules.Services.Domain.Repositories;

namespace SportManagementSystem.Modules.Services.Infrastructure.Repositories;

public class ServicePriceRepository(ApplicationDbContext db) : IServicePriceRepository
{
    public async Task<DbServicePrice> CreateAsync(DbServicePrice entity, CancellationToken ct)
    {
        var servicePrice = await db.ServicePrices.AddAsync(entity, ct);
        await db.SaveChangesAsync(ct);
        return servicePrice.Entity;
    }

    public async Task DeleteAsync(long id, CancellationToken ct)
    {
        var servicePrice = new DbServicePrice { Id = id };
        db.ServicePrices.Attach(servicePrice);
        db.ServicePrices.Remove(servicePrice);
        await db.SaveChangesAsync(ct);
    }

    public async Task<DbServicePrice?> GetByIdAsync(long id, CancellationToken ct)
    {
        return await db.ServicePrices.FindAsync(id, ct);
    }
}