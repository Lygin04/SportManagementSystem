using SportManagementSystem.BuildingBlocks;
using SportManagementSystem.BuildingBlocks.Abstractions;
using SportManagementSystem.Modules.Clients.Contracts.Response;
using SportManagementSystem.Modules.Clients.Domain.Repositories;

namespace SportManagementSystem.Modules.Clients.Application.Queries.GetMembershipTemplatesByBranch;

public class GetMembershipTemplatesByBranchHandler(
    IMembershipTemplateRepository membershipTemplateRepository)
    : IMessageHandler<GetMembershipTemplatesByBranchMessage, MbResult<List<MembershipTemplateResponse>>>
{
    public async Task<MbResult<List<MembershipTemplateResponse>>> Handle(
        GetMembershipTemplatesByBranchMessage request,
        CancellationToken cancellationToken)
    {
        var templates = await membershipTemplateRepository.GetByBranchAsync(request.BranchId, cancellationToken);

        return MbResult<List<MembershipTemplateResponse>>.Success(
            templates.Select(template => new MembershipTemplateResponse
            {
                Id = template.Id,
                BranchId = template.BranchId,
                SportServiceId = template.SportServiceId,
                ServicePriceId = template.ServicePriceId,
                Name = template.Name,
                Description = template.Description,
                PriceAmount = template.ServicePrice.Amount,
                Currency = template.ServicePrice.Currency.ToString(),
                DurationDays = template.DurationDays,
                VisitLimit = template.VisitLimit,
                IsActive = template.IsActive,
                SportServiceName = template.SportService.Name,
            }).ToList());
    }
}
