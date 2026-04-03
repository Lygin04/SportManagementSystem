using MediatR;
using Microsoft.EntityFrameworkCore;
using SportManagementSystem.BuildingBlocks;
using SportManagementSystem.BuildingBlocks.Abstractions;
using SportManagementSystem.Data;

namespace SportManagementSystem.Modules.Users.Application.Commands.DeleteStaff;

public class DeleteStaffHandler(ApplicationDbContext db) : IMessageHandler<DeleteStaffMessage, MbResult<Unit>>
{
    public async Task<MbResult<Unit>> Handle(DeleteStaffMessage request, CancellationToken cancellationToken)
    {
        if (request.StaffId == request.CurrentUserId)
        {
            return MbResult<Unit>.Failure(new MbError(
                title: "Delete current user is forbidden",
                status: StatusCodes.Status400BadRequest,
                detail: "Нельзя удалить текущего пользователя."));
        }

        var staffExists = await db.Staffs.AnyAsync(staff => staff.Id == request.StaffId, cancellationToken);
        if (!staffExists)
        {
            return MbResult<Unit>.Failure(new MbError(
                title: "Staff not found",
                status: StatusCodes.Status404NotFound,
                detail: "Сотрудник не найден."));
        }

        var ownsBranches = await db.Branches.AnyAsync(branch => branch.AdminId == request.StaffId, cancellationToken);
        if (ownsBranches)
        {
            return MbResult<Unit>.Failure(new MbError(
                title: "Delete branch owner is forbidden",
                status: StatusCodes.Status409Conflict,
                detail: "Нельзя удалить сотрудника, который является владельцем филиала."));
        }

        await using var transaction = await db.Database.BeginTransactionAsync(cancellationToken);

        var trainingSessionIds = await db.TrainingSessions
            .Where(session => session.TrainerId == request.StaffId)
            .Select(session => session.Id)
            .ToListAsync(cancellationToken);

        if (trainingSessionIds.Count > 0)
        {
            await db.Bookings
                .Where(booking => trainingSessionIds.Contains(booking.SessionId))
                .ExecuteDeleteAsync(cancellationToken);

            await db.TrainingSessions
                .Where(session => trainingSessionIds.Contains(session.Id))
                .ExecuteDeleteAsync(cancellationToken);
        }

        await db.ServiceSchedules
            .Where(schedule => schedule.StaffId == request.StaffId)
            .ExecuteUpdateAsync(setters => setters
                .SetProperty(schedule => schedule.StaffId, (long?)null), cancellationToken);

        var staff = await db.Staffs
            .Include(entity => entity.AdminBranches)
            .Include(entity => entity.StaffBranches)
            .FirstAsync(entity => entity.Id == request.StaffId, cancellationToken);

        staff.AdminBranches.Clear();
        staff.StaffBranches.Clear();
        await db.SaveChangesAsync(cancellationToken);

        await db.UserAccounts
            .Where(account => account.StaffId == request.StaffId)
            .ExecuteDeleteAsync(cancellationToken);

        await db.Staffs
            .Where(entity => entity.Id == request.StaffId)
            .ExecuteDeleteAsync(cancellationToken);

        await transaction.CommitAsync(cancellationToken);
        db.ChangeTracker.Clear();

        return MbResult<Unit>.Success(Unit.Value);
    }
}
