using SportManagementSystem.Data;
using SportManagementSystem.Modules.Clients.Domain.Entities;
using SportManagementSystem.Modules.Clients.Domain.Repositories;

namespace SportManagementSystem.Modules.Clients.Infrastructure.Repositories;

public class MembershipRepository(ApplicationDbContext db) : IMembershipRepository
{
    public async Task<DbMembership> CreateAsync(DbMembership entity, CancellationToken ct)
    {
        var membership = await db.Memberships.AddAsync(entity, ct);
        await db.SaveChangesAsync(ct);
        return membership.Entity;
    }

    public async Task DeleteAsync(long id, CancellationToken ct)
    {
        var membership = new  DbMembership { Id = id };
        db.Memberships.Attach(membership);
        db.Memberships.Remove(membership);
        await db.SaveChangesAsync(ct);
    }

    public async Task<DbMembership?> GetByIdAsync(long id, CancellationToken ct)
    {
        return await db.Memberships.FindAsync(id, ct);
    }
}