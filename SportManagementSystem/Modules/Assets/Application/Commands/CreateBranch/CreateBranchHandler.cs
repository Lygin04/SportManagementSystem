using SportManagementSystem.BuildingBlocks;
using SportManagementSystem.BuildingBlocks.Abstractions;
using SportManagementSystem.Modules.Assets.Domain.Entities;
using SportManagementSystem.Modules.Assets.Domain.Repositories;

namespace SportManagementSystem.Modules.Assets.Application.Commands.CreateBranch;

public class CreateBranchHandler(IBranchRepository branchRepository) : IMessageHandler<CreateBranchMessage, MbResult<long>>
{
    public async Task<MbResult<long>> Handle(CreateBranchMessage request, CancellationToken cancellationToken)
    {
        var branch = new DbBranch
        {
            AdminId = request.AdminId,
            Name = request.Request.Name,
            Address = request.Request.Address,
            Latitude = request.Request.Latitude,
            Longitude = request.Request.Longitude
        };
            
        var result = await branchRepository.CreateAsync(branch, cancellationToken);

        return MbResult<long>.Success(result.Id);
    }
}
