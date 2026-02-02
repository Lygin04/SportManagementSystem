using SportManagementSystem.BuildingBlocks;
using SportManagementSystem.BuildingBlocks.Abstractions;
using SportManagementSystem.Modules.Branches.Domain.Entities;
using SportManagementSystem.Modules.Branches.Domain.Repositories;

namespace SportManagementSystem.Modules.Branches.Application.Queries.GetBranches;

public class GetBranchesHandler(IBranchRepository branchRepository) : IMessageHandler<GetBranchesMessage, MbResult<List<DbBranch>>>
{
    public async Task<MbResult<List<DbBranch>>> Handle(GetBranchesMessage request, CancellationToken cancellationToken)
    {
        var branches = await branchRepository.GetAllAsync(cancellationToken);
        return MbResult<List<DbBranch>>.Success(branches);
    }
}