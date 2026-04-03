using SportManagementSystem.BuildingBlocks;
using SportManagementSystem.BuildingBlocks.Abstractions;
using SportManagementSystem.Modules.Clients.Contracts.Response;
using SportManagementSystem.Modules.Clients.Domain.Repositories;

namespace SportManagementSystem.Modules.Clients.Application.Queries.GetMyMemberships;

public class GetMyMembershipsHandler(IMembershipRepository membershipRepository)
    : IMessageHandler<GetMyMembershipsMessage, MbResult<List<ClientMembershipResponse>>>
{
    public async Task<MbResult<List<ClientMembershipResponse>>> Handle(
        GetMyMembershipsMessage request,
        CancellationToken cancellationToken)
    {
        var memberships = await membershipRepository.GetByClientIdAsync(request.ClientId, cancellationToken);

        var response = memberships
            .Select(membership => new ClientMembershipResponse
            {
                Id = membership.Id,
                SportServiceId = membership.SportServiceId,
                SportServiceName = membership.SportService?.Name ?? string.Empty,
                BranchId = membership.SportService?.BranchId ?? 0,
                BranchName = membership.SportService?.Branch?.Name ?? string.Empty,
                MembershipTemplateId = membership.MembershipTemplateId,
                MembershipTemplateName = membership.MembershipTemplate?.Name,
                StartDate = membership.StartDate,
                EndDate = membership.EndDate,
                TotalVisits = membership.TotalVisits,
                RemainingVisits = membership.RemainingVisits,
                Status = membership.Status.ToString()
            })
            .ToList();

        return MbResult<List<ClientMembershipResponse>>.Success(response);
    }
}
