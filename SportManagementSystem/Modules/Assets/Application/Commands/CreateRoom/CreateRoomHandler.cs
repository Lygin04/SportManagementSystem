using SportManagementSystem.BuildingBlocks;
using SportManagementSystem.BuildingBlocks.Abstractions;
using SportManagementSystem.Entities.Enums;
using SportManagementSystem.Modules.Assets.Domain.Entities;
using SportManagementSystem.Modules.Assets.Domain.Repositories;

namespace SportManagementSystem.Modules.Assets.Application.Commands.CreateRoom;

public class CreateRoomHandler(
    IRoomRepository roomRepository,
    IBranchRepository branchRepository) : IMessageHandler<CreateRoomMessage, MbResult<long>>
{
    public async Task<MbResult<long>> Handle(CreateRoomMessage request, CancellationToken cancellationToken)
    {
        var exists = await branchRepository.ExistsAsync(request.Request.BranchId, cancellationToken);
        if (!exists)
        {
            return MbResult<long>.Failure(new MbError(
                title: "Branch doesn't exist",
                status: StatusCodes.Status409Conflict,
                detail: "Филиала не существует"));
        }
        
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