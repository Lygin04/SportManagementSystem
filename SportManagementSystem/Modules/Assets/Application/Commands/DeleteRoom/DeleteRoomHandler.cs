using MediatR;
using Microsoft.EntityFrameworkCore;
using SportManagementSystem.BuildingBlocks;
using SportManagementSystem.BuildingBlocks.Abstractions;
using SportManagementSystem.Data;

namespace SportManagementSystem.Modules.Assets.Application.Commands.DeleteRoom;

public class DeleteRoomHandler(ApplicationDbContext db) : IMessageHandler<DeleteRoomMessage, MbResult<Unit>>
{
    public async Task<MbResult<Unit>> Handle(DeleteRoomMessage request, CancellationToken cancellationToken)
    {
        var roomExists = await db.Rooms.AnyAsync(room => room.Id == request.Id, cancellationToken);
        if (!roomExists)
        {
            return MbResult<Unit>.Failure(new MbError(
                title: "Room not found",
                status: StatusCodes.Status404NotFound,
                detail: "Помещение не найдено."));
        }

        await using var transaction = await db.Database.BeginTransactionAsync(cancellationToken);

        await db.ServiceSchedules
            .Where(schedule => schedule.RoomId == request.Id)
            .ExecuteUpdateAsync(setters => setters
                .SetProperty(schedule => schedule.RoomId, (long?)null), cancellationToken);

        await db.Equipments
            .Where(equipment => equipment.RoomId == request.Id)
            .ExecuteDeleteAsync(cancellationToken);

        await db.Rooms
            .Where(room => room.Id == request.Id)
            .ExecuteDeleteAsync(cancellationToken);

        await transaction.CommitAsync(cancellationToken);
        db.ChangeTracker.Clear();

        return MbResult<Unit>.Success(Unit.Value);
    }
}
