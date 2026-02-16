using MediatR;
using SportManagementSystem.BuildingBlocks;
using SportManagementSystem.BuildingBlocks.Abstractions;
using SportManagementSystem.Modules.Assets.Domain.Repositories;
using SportManagementSystem.Modules.Images.Application.Commands.DeleteImage;

namespace SportManagementSystem.Modules.Assets.Application.Commands.DeleteImageBranch;

public class DeleteImageBranchHandler(
    IBranchRepository branchRepository,
    IMediator mediator) : IMessageHandler<DeleteImageBranchMessage, MbResult<Unit>>
{
    public async Task<MbResult<Unit>> Handle(DeleteImageBranchMessage request, CancellationToken cancellationToken)
    {
        var branch = await branchRepository.GetByIdAsync(request.BranchId, cancellationToken);
        if (branch is null)
        {
            return MbResult<Unit>.Failure(new MbError(
                title: "Branch not found",
                status: StatusCodes.Status404NotFound,
                detail: "Branch was not found."));
        }

        if (branch.Images.All(i => i.Id != request.ImageId))
        {
            return MbResult<Unit>.Failure(new MbError(
                title: "Image not found",
                status: StatusCodes.Status404NotFound,
                detail: "Image was not found for this branch."));
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
