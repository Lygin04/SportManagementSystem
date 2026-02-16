using MediatR;
using SportManagementSystem.BuildingBlocks;
using SportManagementSystem.BuildingBlocks.Abstractions;
using SportManagementSystem.Modules.Assets.Domain.Repositories;
using SportManagementSystem.Modules.Images.Application.Commands.DeleteImage;

namespace SportManagementSystem.Modules.Assets.Application.Commands.DeleteImageRoom;

public class DeleteImageRoomHandler(
    IRoomRepository roomRepository,
    IMediator mediator) : IMessageHandler<DeleteImageRoomMessage, MbResult<Unit>>
{
    public async Task<MbResult<Unit>> Handle(DeleteImageRoomMessage request, CancellationToken cancellationToken)
    {
        var room = await roomRepository.GetByIdAsync(request.RoomId, cancellationToken);
        if (room is null)
        {
            return MbResult<Unit>.Failure(new MbError(
                title: "Room not found",
                status: StatusCodes.Status404NotFound,
                detail: "Room was not found."));
        }

        if (room.Images.All(i => i.Id != request.ImageId))
        {
            return MbResult<Unit>.Failure(new MbError(
                title: "Image not found",
                status: StatusCodes.Status404NotFound,
                detail: "Image was not found for this room."));
        }

        var deleteResult = await mediator.Send(new DeleteImageMessage(request.ImageId), cancellationToken);
        return deleteResult.IsSuccess
            ? MbResult<Unit>.Success(Unit.Value)
            : MbResult<Unit>.Failure(deleteResult.Error ?? new MbError(
                title: "Delete Failed",
                status: StatusCodes.Status500InternalServerError,
                detail: "Image delete failed."));
    }
}
