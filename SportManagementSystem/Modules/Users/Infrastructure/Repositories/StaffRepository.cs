using Microsoft.EntityFrameworkCore;
using SportManagementSystem.Data;
using SportManagementSystem.Modules.Users.Domain.Entities;
using SportManagementSystem.Modules.Users.Domain.Repositories;

namespace SportManagementSystem.Modules.Users.Infrastructure.Repositories;

public class StaffRepository(ApplicationDbContext db) : IStaffRepository
{
    public async Task<DbStaff> CreateAsync(DbStaff entity, CancellationToken ct)
    {
        var staff = await db.Staffs.AddAsync(entity, ct);
        await db.SaveChangesAsync(ct);
        return staff.Entity;
    }

    public async Task DeleteAsync(long id, CancellationToken ct)
    {
        var staff = new  DbStaff { Id = id };
        db.Staffs.Attach(staff);
        db.Staffs.Remove(staff);
        await db.SaveChangesAsync(ct);
    }

    public async Task<DbStaff?> GetByIdAsync(long id, CancellationToken ct)
    {
        return await db.Staffs.FindAsync(id, ct);
    }

    public async Task<bool> ExistsAsync(long id, CancellationToken ct)
    {
        return await db.Staffs.AnyAsync(staff => staff.Id == id, ct);
    }

    public async Task<bool> SetImageIdAsync(long staffId, Guid? imageId, CancellationToken ct)
    {
        var updatedRows = await db.Staffs
            .Where(c => c.Id == staffId)
            .ExecuteUpdateAsync(setters => setters
                .SetProperty(c => c.AvatarId, imageId), ct);

        db.ChangeTracker.Clear();

        return updatedRows > 0;
    }
}
