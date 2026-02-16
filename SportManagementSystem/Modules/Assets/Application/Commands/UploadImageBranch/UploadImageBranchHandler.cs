using MediatR;
using SportManagementSystem.BuildingBlocks;
using SportManagementSystem.BuildingBlocks.Abstractions;
using SportManagementSystem.Modules.Assets.Domain.Repositories;
using SportManagementSystem.Modules.Images.Application.Commands.UploadImage;

namespace SportManagementSystem.Modules.Assets.Application.Commands.UploadImageBranch;

public class UploadImageBranchHandler(
    IBranchRepository branchRepository,
    IMediator mediator) : IMessageHandler<UploadImageBranchMessage, MbResult<Unit>>
{
    public async Task<MbResult<Unit>> Handle(UploadImageBranchMessage request, CancellationToken cancellationToken)
    {
        var branch = await branchRepository.GetByIdAsync(request.BranchId, cancellationToken);
        if (branch is null)
        {
            return MbResult<Unit>.Failure(new MbError(
                title: "Branch not found",
                status: StatusCodes.Status404NotFound,
                detail: "Branch was not found."));
        }

        var uploadResult = await mediator.Send(new UploadImageMessage(request.Request), cancellationToken);
        if (!uploadResult.IsSuccess)
        {
            return MbResult<Unit>.Failure(uploadResult.Error ?? new MbError(
                title: "Upload Failed",
                status: StatusCodes.Status500InternalServerError,
                detail: "Image upload failed."));
        }

        await branchRepository.AddImage(branch, uploadResult.Data);
        return MbResult<Unit>.Success(Unit.Value);
    }
}
