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

    public async Task<bool> ExistsAsync(long id, CancellationToken ct)
    {
        return await db.Clients.AnyAsync(c => c.Id == id, ct);
    }

    public async Task<bool> SetImageIdAsync(long clientId, Guid? imageId, CancellationToken ct)
    {
        var updatedRows = await db.Clients
            .Where(c => c.Id == clientId)
            .ExecuteUpdateAsync(setters => setters
                .SetProperty(c => c.AvatarId, imageId), ct);

        db.ChangeTracker.Clear();

        return updatedRows > 0;
    }
}
