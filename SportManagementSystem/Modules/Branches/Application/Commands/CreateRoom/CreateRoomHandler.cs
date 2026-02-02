using SportManagementSystem.BuildingBlocks;
using SportManagementSystem.BuildingBlocks.Abstractions;
using SportManagementSystem.Entities.Enums;
using SportManagementSystem.Modules.Branches.Domain.Entities;
using SportManagementSystem.Modules.Branches.Domain.Repositories;

namespace SportManagementSystem.Modules.Branches.Application.Commands.CreateRoom;

public class CreateRoomHandler(IRoomRepository roomRepository) : IMessageHandler<CreateRoomMessage, MbResult<long>>
{
    public async Task<MbResult<long>> Handle(CreateRoomMessage request, CancellationToken cancellationToken)
    {
        var room = new DbRoom
        {
            BranchId = request.Request.BranchId,
            Name = request.Request.Name,
            Capacity = request.Request.Capacity,
            Status = ERoomStatus.Available
        };
        
        var result = await roomRepository.CreateAsync(room, cancellationToken);
        return MbResult<long>.Success(result.Id);
    }
}