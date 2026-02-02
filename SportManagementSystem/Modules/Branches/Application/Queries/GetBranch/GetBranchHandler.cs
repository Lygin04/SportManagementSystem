using SportManagementSystem.BuildingBlocks;
using SportManagementSystem.BuildingBlocks.Abstractions;
using SportManagementSystem.Modules.Branches.Domain.Entities;
using SportManagementSystem.Modules.Branches.Domain.Repositories;

namespace SportManagementSystem.Modules.Branches.Application.Queries.GetBranch;

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