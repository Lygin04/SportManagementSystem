using MediatR;
using Microsoft.EntityFrameworkCore;
using SportManagementSystem.BuildingBlocks;
using SportManagementSystem.BuildingBlocks.Abstractions;
using SportManagementSystem.Data;

namespace SportManagementSystem.Modules.Scheduling.Application.Commands.DeleteServiceSchedule;

public class DeleteServiceScheduleHandler(ApplicationDbContext db)
    : IMessageHandler<DeleteServiceScheduleMessage, MbResult<Unit>>
{
    public async Task<MbResult<Unit>> Handle(DeleteServiceScheduleMessage request, CancellationToken cancellationToken)
    {
        var scheduleExists = await db.ServiceSchedules.AnyAsync(schedule => schedule.Id == request.Id, cancellationToken);
        if (!scheduleExists)
        {
            return MbResult<Unit>.Failure(new MbError(
                title: "Service schedule not found",
                status: StatusCodes.Status404NotFound,
                detail: "Слот расписания не найден."));
        }

        await using var transaction = await db.Database.BeginTransactionAsync(cancellationToken);

        await db.TrainingSessions
            .Where(session => session.ScheduleId == request.Id)
            .ExecuteUpdateAsync(setters => setters
                .SetProperty(session => session.ScheduleId, (long?)null), cancellationToken);

        await db.ServiceSchedules
            .Where(schedule => schedule.Id == request.Id)
            .ExecuteDeleteAsync(cancellationToken);

        await transaction.CommitAsync(cancellationToken);
        db.ChangeTracker.Clear();

        return MbResult<Unit>.Success(Unit.Value);
    }
}
