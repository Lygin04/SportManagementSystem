using MediatR;
using Microsoft.EntityFrameworkCore;
using SportManagementSystem.BuildingBlocks;
using SportManagementSystem.BuildingBlocks.Abstractions;
using SportManagementSystem.Data;

namespace SportManagementSystem.Modules.Scheduling.Application.Commands.DeleteTrainingSession;

public class DeleteTrainingSessionHandler(ApplicationDbContext db)
    : IMessageHandler<DeleteTrainingSessionMessage, MbResult<Unit>>
{
    public async Task<MbResult<Unit>> Handle(DeleteTrainingSessionMessage request, CancellationToken cancellationToken)
    {
        var sessionExists = await db.TrainingSessions.AnyAsync(session => session.Id == request.Id, cancellationToken);
        if (!sessionExists)
        {
            return MbResult<Unit>.Failure(new MbError(
                title: "Training session not found",
                status: StatusCodes.Status404NotFound,
                detail: "Занятие не найдено."));
        }

        await using var transaction = await db.Database.BeginTransactionAsync(cancellationToken);

        await db.Bookings
            .Where(booking => booking.SessionId == request.Id)
            .ExecuteDeleteAsync(cancellationToken);

        await db.TrainingSessions
            .Where(session => session.Id == request.Id)
            .ExecuteDeleteAsync(cancellationToken);

        await transaction.CommitAsync(cancellationToken);
        db.ChangeTracker.Clear();

        return MbResult<Unit>.Success(Unit.Value);
    }
}
