using Microsoft.EntityFrameworkCore;
using SportManagementSystem.Data;
using SportManagementSystem.Modules.Branches.Domain.Entities;
using SportManagementSystem.Modules.Branches.Domain.Repositories;

namespace SportManagementSystem.Modules.Branches.Infrastructure.Repositories;

public class BranchRepository(ApplicationDbContext db) : IBranchRepository
{
    public async Task<DbBranch> CreateAsync(DbBranch entity, CancellationToken ct)
    {
        var branch = await db.Branches.AddAsync(entity, ct);
        await db.SaveChangesAsync(ct);
        return branch.Entity;
    }

    public async Task DeleteAsync(long id, CancellationToken ct)
    {
        var branch = new DbBranch { Id = id };
        db.Branches.Attach(branch);
        db.Branches.Remove(branch);
        await db.SaveChangesAsync(ct);
    }

    public async Task<DbBranch?> GetByIdAsync(long id, CancellationToken ct)
    {
        return await db.Branches.FindAsync(id, ct);
    }

    public async Task<List<DbBranch>> GetAllAsync(CancellationToken ct)
    {
        return await db.Branches.ToListAsync(ct);
    }
}