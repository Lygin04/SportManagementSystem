using Microsoft.EntityFrameworkCore;
using SportManagementSystem.Data;
using SportManagementSystem.Modules.Assets.Domain.Entities;
using SportManagementSystem.Modules.Assets.Domain.Repositories;
using SportManagementSystem.Modules.Images.Domain.Entities;

namespace SportManagementSystem.Modules.Assets.Infrastructure.Repositories;

public class BranchRepository(ApplicationDbContext db) : IBranchRepository
{
    public async Task<DbBranch> CreateAsync(DbBranch entity, CancellationToken ct)
    {
        var creator = await db.Staffs.FirstOrDefaultAsync(s => s.Id == entity.AdminId, ct);
        if (creator is not null)
        {
            entity.BranchAdmins.Add(creator);
        }

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
        return await db.Branches
            .Include(b => b.Images)
            .Include(b => b.Rooms)
                .ThenInclude(room => room.Images)
            .Include(b => b.Rooms)
                .ThenInclude(room => room.Equipments)
                    .ThenInclude(equipment => equipment.Images)
            .FirstOrDefaultAsync(b => b.Id == id, ct);
    }

    public async Task<List<DbBranch>> GetAllAsync(CancellationToken ct)
    {
        return await db.Branches
            .Include(b => b.Images)
            .Include(r => r.Rooms)
            .ToListAsync(ct);
    }

    public async Task AddImage(DbBranch branch, Guid imageId, CancellationToken ct)
    {
        var trackedImage = db.Images.Local.FirstOrDefault(i => i.Id == imageId);
        if (trackedImage is null)
        {
            trackedImage = new DbImage { Id = imageId };
            db.Images.Attach(trackedImage);
        }

        branch.Images.Add(trackedImage);
        await db.SaveChangesAsync(ct);
    }

    public async Task<List<DbBranch>> GetByAdmin(long adminId, CancellationToken ct)
    {
        return await db.Branches
            .Where(s => s.AdminId == adminId)
            .Include(b => b.Images)
            .Include(r => r.Rooms)
            .ToListAsync(ct);
    }

    public async Task<List<DbBranch>> GetManagedByStaffAsync(long staffId, CancellationToken ct)
    {
        return await db.Branches
            .Where(branch =>
                branch.AdminId == staffId ||
                branch.BranchAdmins.Any(admin => admin.Id == staffId) ||
                branch.BranchStaffs.Any(staff => staff.Id == staffId))
            .Include(b => b.Images)
            .Include(r => r.Rooms)
            .ToListAsync(ct);
    }

    public async Task<bool> ExistsAsync(long id, CancellationToken ct)
    {
        return db.Branches.Any(e => e.Id == id);
    }

    public async Task<bool> AddStaffMemberAsync(long branchId, long staffId, bool asAdmin, CancellationToken ct)
    {
        var branch = await db.Branches
            .Include(b => b.BranchAdmins)
            .Include(b => b.BranchStaffs)
            .FirstOrDefaultAsync(b => b.Id == branchId, ct);
        if (branch is null)
        {
            return false;
        }

        var staff = await db.Staffs.FirstOrDefaultAsync(s => s.Id == staffId, ct);
        if (staff is null)
        {
            return false;
        }

        if (asAdmin)
        {
            if (!branch.BranchAdmins.Any(member => member.Id == staffId))
            {
                branch.BranchAdmins.Add(staff);
            }
        }
        else if (!branch.BranchStaffs.Any(member => member.Id == staffId))
        {
            branch.BranchStaffs.Add(staff);
        }

        await db.SaveChangesAsync(ct);
        return true;
    }

    public async Task<bool> HasManagementAccessAsync(long branchId, long staffId, CancellationToken ct)
    {
        return await db.Branches
            .Where(branch => branch.Id == branchId)
            .AnyAsync(branch =>
                branch.AdminId == staffId ||
                branch.BranchAdmins.Any(admin => admin.Id == staffId) ||
                branch.BranchStaffs.Any(staff => staff.Id == staffId), ct);
    }
}
