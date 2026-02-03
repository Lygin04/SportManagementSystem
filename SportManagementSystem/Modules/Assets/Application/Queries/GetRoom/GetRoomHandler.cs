using SportManagementSystem.BuildingBlocks;
using SportManagementSystem.BuildingBlocks.Abstractions;
using SportManagementSystem.Modules.Assets.Domain.Entities;
using SportManagementSystem.Modules.Assets.Domain.Repositories;

namespace SportManagementSystem.Modules.Assets.Application.Queries.GetRoom;

public class GetRoomHandler(IRoomRepository roomRepository) : IMessageHandler<GetRoomMessage, MbResult<DbRoom>>
{
    public async Task<MbResult<DbRoom>> Handle(GetRoomMessage request, CancellationToken cancellationToken)
    {
        var room = await roomRepository.GetByIdAsync(request.Id, cancellationToken);

        if (room == null)
        {
            return MbResult<DbRoom>.Failure(new MbError(
                title: "Room not found",
                status: StatusCodes.Status404NotFound,
                detail: "Комната не существует"));
        }
        
        return MbResult<DbRoom>.Success(room);
    }
}