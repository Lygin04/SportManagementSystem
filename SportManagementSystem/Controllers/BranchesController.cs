using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SportManagementSystem.BuildingBlocks.Abstractions;
using SportManagementSystem.Modules.Assets.Application.Commands.CreateBranch;
using SportManagementSystem.Modules.Assets.Application.Queries.GetBranch;
using SportManagementSystem.Modules.Assets.Application.Queries.GetBranches;
using SportManagementSystem.Modules.Assets.Contracts.Request;

namespace SportManagementSystem.Controllers;

public class BranchesController(IMediator mediator) : ApiControllerV1WithAuth
{
    /// <summary>
    /// Создать филиал спортивной организации.
    /// </summary>
    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> Create(CreateBranchRequest request, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new CreateBranchMessage(request), cancellationToken);
        return result.IsSuccess
            ? Created(nameof(GetById), new { Id = result.Data})
            : ToActionResult(result);
    }

    /// <summary>
    /// Получить филиал спортивной организации по идентификатору.
    /// </summary>
    [HttpGet("{id:long}")]
    public async Task<IActionResult> GetById(long id, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetBranchMessage(id), cancellationToken);
        return ToActionResult(result);
    }

    /// <summary>
    /// Получить все филиалы спортивных организаций.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetBranchesMessage(), cancellationToken);
        return ToActionResult(result);
    }
}