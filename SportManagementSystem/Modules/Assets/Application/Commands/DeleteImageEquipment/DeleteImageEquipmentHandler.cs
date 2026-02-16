using MediatR;
using SportManagementSystem.BuildingBlocks;
using SportManagementSystem.BuildingBlocks.Abstractions;
using SportManagementSystem.Modules.Assets.Domain.Repositories;
using SportManagementSystem.Modules.Images.Application.Commands.DeleteImage;

namespace SportManagementSystem.Modules.Assets.Application.Commands.DeleteImageEquipment;

public class DeleteImageEquipmentHandler(
    IEquipmentRepository equipmentRepository,
    IMediator mediator) : IMessageHandler<DeleteImageEquipmentMessage, MbResult<Unit>>
{
    public async Task<MbResult<Unit>> Handle(DeleteImageEquipmentMessage request, CancellationToken cancellationToken)
    {
        var equipment = await equipmentRepository.GetByIdAsync(request.EquipmentId, cancellationToken);
        if (equipment is null)
        {
            return MbResult<Unit>.Failure(new MbError(
                title: "Equipment not found",
                status: StatusCodes.Status404NotFound,
                detail: "Equipment was not found."));
        }

        if (equipment.Images.All(i => i.Id != request.ImageId))
        {
            return MbResult<Unit>.Failure(new MbError(
                title: "Image not found",
                status: StatusCodes.Status404NotFound,
                detail: "Image was not found for this equipment."));
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
