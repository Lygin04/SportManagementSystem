using SportManagementSystem.BuildingBlocks;
using SportManagementSystem.BuildingBlocks.Abstractions;
using SportManagementSystem.Modules.Branches.Domain.Entities;
using SportManagementSystem.Modules.Branches.Domain.Repositories;

namespace SportManagementSystem.Modules.Branches.Application.Queries.GetRoomByBranch;

public class GetRoomByBranchHandler(IRoomRepository roomRepository) : IMessageHandler<GetRoomByBranchMessage, MbResult<List<DbRoom>>>
{
    public async Task<MbResult<List<DbRoom>>> Handle(GetRoomByBranchMessage request, CancellationToken cancellationToken)
    {
        var rooms = await roomRepository.GetByBranchAsync(request.BranchId, cancellationToken);

        if (rooms == null || rooms.Count == 0)
        {
            return MbResult<List<DbRoom>>.Failure(new MbError(
                title: "Rooms not found",
                status: StatusCodes.Status404NotFound,
                detail: "Не существует комнат в таком филиале"));
        }
        
        return MbResult<List<DbRoom>>.Success(rooms);
    }
}