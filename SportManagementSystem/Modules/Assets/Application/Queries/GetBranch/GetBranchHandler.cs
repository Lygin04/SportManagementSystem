using SportManagementSystem.BuildingBlocks;
using SportManagementSystem.BuildingBlocks.Abstractions;
using SportManagementSystem.Modules.Assets.Domain.Entities;
using SportManagementSystem.Modules.Assets.Domain.Repositories;

namespace SportManagementSystem.Modules.Assets.Application.Queries.GetBranch;

public class GetBranchHandler(IBranchRepository branchRepository) : IMessageHandler<GetBranchMessage, MbResult<DbBranch>>
{
    public async Task<MbResult<DbBranch>> Handle(GetBranchMessage request, CancellationToken cancellationToken)
    {
        var branch = await branchRepository.GetByIdAsync(request.Id, cancellationToken);

        if (branch == null)
        {
            return MbResult<DbBranch>.Failure(new MbError(
                title: "Branch not found",
                status: StatusCodes.Status404NotFound,
                detail: "Филиала не существует"));
        }
        
        return MbResult<DbBranch>.Success(branch);
    }
}