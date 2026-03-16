using SportManagementSystem.BuildingBlocks;
using SportManagementSystem.BuildingBlocks.Abstractions;
using SportManagementSystem.Modules.Assets.Domain.Entities;
using SportManagementSystem.Modules.Assets.Domain.Repositories;

namespace SportManagementSystem.Modules.Assets.Application.Queries.GetBranchesByAdmin;

public class GetBranchesByAdminHandler(
    IBranchRepository branchRepository) : IMessageHandler<GetBranchesByAdminMessage, MbResult<List<DbBranch>>>
{
    public async Task<MbResult<List<DbBranch>>> Handle(GetBranchesByAdminMessage request, CancellationToken cancellationToken)
    {
        var result = await branchRepository.GetByAdmin(request.UserId, cancellationToken);
        
        return MbResult<List<DbBranch>>.Success(result);
    }
}