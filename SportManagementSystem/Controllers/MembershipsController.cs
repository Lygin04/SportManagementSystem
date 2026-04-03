using MediatR;
using Microsoft.AspNetCore.Mvc;
using SportManagementSystem.BuildingBlocks.Abstractions;
using SportManagementSystem.Modules.Clients.Application.Commands.CreateMembership;
using SportManagementSystem.Modules.Clients.Application.Queries.GetMembership;
using SportManagementSystem.Modules.Clients.Application.Queries.GetMyMemberships;
using SportManagementSystem.Modules.Clients.Contracts.Requests;

using Microsoft.AspNetCore.Authorization;

namespace SportManagementSystem.Controllers;

public class MembershipsController(IMediator mediator) : ApiControllerV1WithAuth
{
    [HttpPost]
    public async Task<IActionResult> Create(CreateMembershipRequest request, CancellationToken ct)
    {
        var result = await mediator.Send(new CreateMembershipMessage(UserId, Role, request), ct);
        return result.IsSuccess
            ? Created(nameof(GetById), new { Id = result.Data })
            : ToActionResult(result);
    }

    [HttpGet("{id:long}")]
    public async Task<IActionResult> GetById(long id, CancellationToken ct)
    {
        var result = await mediator.Send(new GetMembershipMessage(id), ct);
        return ToActionResult(result);
    }

    [Authorize(Roles = "Client")]
    [HttpGet("me")]
    public async Task<IActionResult> GetMyMemberships(CancellationToken ct)
    {
        var result = await mediator.Send(new GetMyMembershipsMessage(UserId), ct);
        return ToActionResult(result);
    }
}
