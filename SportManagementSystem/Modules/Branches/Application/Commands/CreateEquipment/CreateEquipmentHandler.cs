using SportManagementSystem.BuildingBlocks;
using SportManagementSystem.BuildingBlocks.Abstractions;
using SportManagementSystem.Modules.Branches.Domain.Entities;
using SportManagementSystem.Modules.Branches.Domain.Repositories;

namespace SportManagementSystem.Modules.Branches.Application.Commands.CreateEquipment;

public class CreateEquipmentHandler(
    IEquipmentRepository equipmentRepository) : IMessageHandler<CreateEquipmentMessage, MbResult<long>>
{
    public async Task<MbResult<long>> Handle(CreateEquipmentMessage request, CancellationToken cancellationToken)
    {
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