using SportManagementSystem.BuildingBlocks;
using SportManagementSystem.BuildingBlocks.Abstractions;
using SportManagementSystem.Modules.Assets.Domain.Entities;
using SportManagementSystem.Modules.Assets.Domain.Repositories;

namespace SportManagementSystem.Modules.Assets.Application.Commands.CreateEquipment;

public class CreateEquipmentHandler(
    IEquipmentRepository equipmentRepository,
    IRoomRepository roomRepository) : IMessageHandler<CreateEquipmentMessage, MbResult<long>>
{
    public async Task<MbResult<long>> Handle(CreateEquipmentMessage request, CancellationToken cancellationToken)
    {
        var exist = await roomRepository.ExistsAsync(request.Request.RoomId, cancellationToken);
        if (!exist)
        {
            return MbResult<long>.Failure(new MbError(
                title: "Room Not Found",
                status: StatusCodes.Status409Conflict,
                detail: "Комната не найдена"));
        }
        
        var equipment = new DbEquipment
        {
            RoomId = request.Request.RoomId,
            Name = request.Request.Name,
            Quantity = request.Request.Quantity,
            Condition = request.Request.Condition
        };

        var result = await equipmentRepository.CreateAsync(equipment, cancellationToken);
        return MbResult<long>.Success(result.Id);
    }
}