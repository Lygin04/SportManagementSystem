using MediatR;
using Microsoft.EntityFrameworkCore;
using SportManagementSystem.BuildingBlocks;
using SportManagementSystem.BuildingBlocks.Abstractions;
using SportManagementSystem.Data;

namespace SportManagementSystem.Modules.Services.Application.Commands.DeleteSportService;

public class DeleteSportServiceHandler(
    ApplicationDbContext db) : IMessageHandler<DeleteSportServiceMessage, MbResult<Unit>>
{
    public async Task<MbResult<Unit>> Handle(DeleteSportServiceMessage request, CancellationToken cancellationToken)
    {
        var sportServiceExists = await db.SportServices.AnyAsync(service => service.Id == request.Id, cancellationToken);
        if (!sportServiceExists)
        {
            return MbResult<Unit>.Failure(new MbError(
                title: "Sport service not found",
                status: StatusCodes.Status404NotFound,
                detail: "Спорт. услуга не найдена."));
        }

        await using var transaction = await db.Database.BeginTransactionAsync(cancellationToken);

        await db.MembershipTemplates
            .Where(template => template.SportServiceId == request.Id)
            .ExecuteDeleteAsync(cancellationToken);

        var trainingSessionIds = await db.TrainingSessions
            .Where(session => session.SportServiceId == request.Id)
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
            .Where(schedule => schedule.SportServiceId == request.Id)
            .ExecuteDeleteAsync(cancellationToken);

        await db.ServicePrices
            .Where(price => price.SportServiceId == request.Id)
            .ExecuteDeleteAsync(cancellationToken);

        await db.SportServices
            .Where(service => service.Id == request.Id)
            .ExecuteUpdateAsync(setters => setters
                .SetProperty(service => service.IsActive, false)
                .SetProperty(service => service.Modified, (DateTimeOffset?)DateTimeOffset.UtcNow), cancellationToken);

        await transaction.CommitAsync(cancellationToken);
        db.ChangeTracker.Clear();

        return MbResult<Unit>.Success(Unit.Value);
    }
}
