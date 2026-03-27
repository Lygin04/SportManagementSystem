using Microsoft.EntityFrameworkCore;
using SportManagementSystem.Data;
using SportManagementSystem.Modules.Clients.Domain.Entities;
using SportManagementSystem.Modules.Clients.Domain.Repositories;

namespace SportManagementSystem.Modules.Clients.Infrastructure.Repositories;

public class MembershipTemplateRepository(ApplicationDbContext db) : IMembershipTemplateRepository
{
    public async Task<DbMembershipTemplate> CreateAsync(DbMembershipTemplate entity, CancellationToken ct)
    {
        var created = await db.Set<DbMembershipTemplate>().AddAsync(entity, ct);
        await db.SaveChangesAsync(ct);
        return created.Entity;
    }

    public async Task DeleteAsync(long id, CancellationToken ct)
    {
        var template = new DbMembershipTemplate { Id = id };
        db.Set<DbMembershipTemplate>().Attach(template);
        db.Set<DbMembershipTemplate>().Remove(template);
        await db.SaveChangesAsync(ct);
    }

    public async Task<DbMembershipTemplate?> GetByIdAsync(long id, CancellationToken ct)
    {
        return await db.Set<DbMembershipTemplate>()
            .AsNoTracking()
            .Include(x => x.SportService)
            .Include(x => x.ServicePrice)
            .FirstOrDefaultAsync(x => x.Id == id, ct);
    }

    public async Task<bool> ExistsAsync(long id, CancellationToken ct)
    {
        return await db.Set<DbMembershipTemplate>().AnyAsync(x => x.Id == id, ct);
    }

    public async Task<List<DbMembershipTemplate>> GetByBranchAsync(long branchId, CancellationToken ct)
    {
        return await db.Set<DbMembershipTemplate>()
            .AsNoTracking()
            .Include(x => x.SportService)
            .Include(x => x.ServicePrice)
            .Where(x => x.BranchId == branchId && x.IsActive)
            .OrderBy(x => x.Name)
            .ToListAsync(ct);
    }
}
