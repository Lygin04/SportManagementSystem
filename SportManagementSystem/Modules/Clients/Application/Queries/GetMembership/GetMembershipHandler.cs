using SportManagementSystem.BuildingBlocks;
using SportManagementSystem.BuildingBlocks.Abstractions;
using SportManagementSystem.Modules.Clients.Domain.Entities;
using SportManagementSystem.Modules.Clients.Domain.Repositories;

namespace SportManagementSystem.Modules.Clients.Application.Queries.GetMembership;

public class GetMembershipHandler(
    IMembershipRepository membershipRepository) : IMessageHandler<GetMembershipMessage, MbResult<DbMembership>>
{
    public async Task<MbResult<DbMembership>> Handle(GetMembershipMessage request, CancellationToken cancellationToken)
    {
        var membership = await membershipRepository.GetByIdAsync(request.Id, cancellationToken);

        if (membership is null)
        {
            return MbResult<DbMembership>.Failure(new MbError(
                title: "Membership not found",
                status: StatusCodes.Status404NotFound,
                detail: "Абонемент не найден"));
        }
        
        return MbResult<DbMembership>.Success(membership);
    }
}