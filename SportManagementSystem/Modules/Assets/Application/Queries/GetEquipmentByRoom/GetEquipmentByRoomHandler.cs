using SportManagementSystem.BuildingBlocks;
using SportManagementSystem.BuildingBlocks.Abstractions;
using SportManagementSystem.Modules.Assets.Domain.Entities;
using SportManagementSystem.Modules.Assets.Domain.Repositories;

namespace SportManagementSystem.Modules.Assets.Application.Queries.GetEquipmentByRoom;

public class GetEquipmentByRoomHandler(IEquipmentRepository equipmentRepository)
    : IMessageHandler<GetEquipmentByRoomMessage, MbResult<List<DbEquipment>>>
{
    public async Task<MbResult<List<DbEquipment>>> Handle(GetEquipmentByRoomMessage request, CancellationToken cancellationToken)
    {
        var equipments = await equipmentRepository.GetByRoomAsync(request.RoomId, cancellationToken);
        return MbResult<List<DbEquipment>>.Success(equipments);
    }
}

