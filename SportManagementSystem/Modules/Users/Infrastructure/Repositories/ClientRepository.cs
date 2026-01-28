using Microsoft.EntityFrameworkCore;
using SportManagementSystem.Data;
using SportManagementSystem.Modules.Users.Domain.Entities;
using SportManagementSystem.Modules.Users.Domain.Repositories;

namespace SportManagementSystem.Modules.Users.Infrastructure.Repositories;

public class ClientRepository(ApplicationDbContext db) : IClientRepository
{
    public async Task<DbClient> CreateAsync(DbClient entity, CancellationToken ct)
    {
        var client = await db.Clients.AddAsync(entity, ct);
        await db.SaveChangesAsync(ct);
        return client.Entity;
    }

    public async Task UpdateAsync(DbClient entity, CancellationToken ct)
    {
        var existingEntity = db.Clients.Local.FirstOrDefault(e => e.Id == entity.Id)
                             ?? await db.Clients.FindAsync(entity.Id, ct);

        if (existingEntity != null)
        {
            db.Entry(existingEntity).CurrentValues.SetValues(entity);
            existingEntity.Modified = DateTime.UtcNow;
        }
        else
        {
            db.Entry(entity).State = EntityState.Modified;
        }
        
        await db.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(long id, CancellationToken ct)
    {
        var client = new  DbClient { Id = id };
        db.Clients.Attach(client);
        db.Clients.Remove(client);
        await db.SaveChangesAsync(ct);
    }

    public async Task<DbClient?> GetByIdAsync(long id, CancellationToken ct)
    {
        return await db.Clients.FindAsync(id, ct);
    }
}