using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SportManagementSystem.BuildingBlocks.Abstractions;
using SportManagementSystem.Modules.Clients.Application.Commands.CreateMembershipTemplate;
using SportManagementSystem.Modules.Clients.Application.Queries.GetMembershipTemplatesByBranch;
using SportManagementSystem.Modules.Clients.Contracts.Requests;

namespace SportManagementSystem.Controllers;

public class MembershipTemplatesController(IMediator mediator) : ApiControllerV1WithAuth
{
    [Authorize(Roles = "Admin, Manager")]
    [HttpPost]
    public async Task<IActionResult> Create(CreateMembershipTemplateRequest request, CancellationToken ct)
    {
        var result = await mediator.Send(new CreateMembershipTemplateMessage(UserId, Role, request), ct);
        return result.IsSuccess
            ? Created("", new { Id = result.Data })
            : ToActionResult(result);
    }

    [AllowAnonymous]
    [HttpGet("branch/{branchId:long}")]
    public async Task<IActionResult> GetByBranchId(long branchId, CancellationToken ct)
    {
        var result = await mediator.Send(new GetMembershipTemplatesByBranchMessage(branchId), ct);
        return ToActionResult(result);
    }
}
