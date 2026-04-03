using MediatR;
using Microsoft.EntityFrameworkCore;
using SportManagementSystem.BuildingBlocks;
using SportManagementSystem.BuildingBlocks.Abstractions;
using SportManagementSystem.Data;

namespace SportManagementSystem.Modules.Assets.Application.Commands.DeleteEquipment;

public class DeleteEquipmentHandler(ApplicationDbContext db) : IMessageHandler<DeleteEquipmentMessage, MbResult<Unit>>
{
    public async Task<MbResult<Unit>> Handle(DeleteEquipmentMessage request, CancellationToken cancellationToken)
    {
        var equipmentExists = await db.Equipments.AnyAsync(equipment => equipment.Id == request.Id, cancellationToken);
        if (!equipmentExists)
        {
            return MbResult<Unit>.Failure(new MbError(
                title: "Equipment not found",
                status: StatusCodes.Status404NotFound,
                detail: "Оборудование не найдено."));
        }

        await db.Equipments
            .Where(equipment => equipment.Id == request.Id)
            .ExecuteDeleteAsync(cancellationToken);

        db.ChangeTracker.Clear();

        return MbResult<Unit>.Success(Unit.Value);
    }
}
