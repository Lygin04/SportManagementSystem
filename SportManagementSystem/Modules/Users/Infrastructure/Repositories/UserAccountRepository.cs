using Microsoft.EntityFrameworkCore;
using SportManagementSystem.Data;
using SportManagementSystem.Modules.Users.Domain.Entities;
using SportManagementSystem.Modules.Users.Domain.Repositories;

namespace SportManagementSystem.Modules.Users.Infrastructure.Repositories;

public class UserAccountRepository(ApplicationDbContext db) : IUserAccountRepository
{
    public async Task<DbUserAccount> CreateAsync(DbUserAccount entity, CancellationToken ct)
    {
        var userAccount = await db.UserAccounts.AddAsync(entity, ct);
        await db.SaveChangesAsync(ct);
        return userAccount.Entity;
    }

    public async Task DeleteAsync(long id, CancellationToken ct)
    {
        var userAccount = new  DbUserAccount { Id = id };
        db.UserAccounts.Attach(userAccount);
        db.UserAccounts.Remove(userAccount);
        await db.SaveChangesAsync(ct);
    }

    public async Task<DbUserAccount?> GetByIdAsync(long id, CancellationToken ct)
    {
        return await db.UserAccounts.FindAsync(id, ct);
    }

    public async Task<DbUserAccount?> GetByEmailAsync(string email, CancellationToken ct)
    {
        return await db.UserAccounts.FirstOrDefaultAsync(x => x.Email == email, ct);
    }

    public async Task<bool> ExistsEmailAsync(string email, CancellationToken ct)
    {
        return await db.UserAccounts.AnyAsync(e => e.Email == email, ct);
    }

    public async Task UpdateLastLoginDateAsync(long id, CancellationToken ct)
    {
        var userAccount = await db.UserAccounts.FindAsync(id, ct);
        userAccount!.LastLogin = DateTime.UtcNow;
        await db.SaveChangesAsync(ct);
    }
}