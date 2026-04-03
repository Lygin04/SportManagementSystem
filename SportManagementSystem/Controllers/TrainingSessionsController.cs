using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SportManagementSystem.BuildingBlocks.Abstractions;
using SportManagementSystem.Modules.Scheduling.Application.Commands.CreateTrainingSession;
using SportManagementSystem.Modules.Scheduling.Application.Queries.GetTrainingSession;
using SportManagementSystem.Modules.Scheduling.Application.Queries.GetTrainingSessionsByBranch;
using SportManagementSystem.Modules.Scheduling.Contracts.Requests;

namespace SportManagementSystem.Controllers;

public class TrainingSessionsController(IMediator mediator) : ApiControllerV1WithAuth
{
    /// <summary>
    /// Создать тренировку.
    /// </summary>
    [Authorize(Roles = "Admin,Manager")]
    [HttpPost]
    public async Task<IActionResult> Create(CreateTrainingSessionRequest request, CancellationToken ct)
    {
        var result = await mediator.Send(new CreateTrainingSessionMessage(request), ct);
        return result.IsSuccess
            ? Created(nameof(GetById), new { Id = result.Data})
            : ToActionResult(result);
    }

    /// <summary>
    /// Получить тренировку по идентификатору.
    /// </summary>
    [HttpGet("{id:long}")]
    public async Task<IActionResult> GetById(long id, CancellationToken ct)
    {
        var result = await mediator.Send(new GetTrainingSessionMessage(id), ct);
        return ToActionResult(result);
    }

    [Authorize(Roles = "Admin,Manager,Client")]
    [HttpGet("branch/{branchId:long}")]
    public async Task<IActionResult> GetByBranchId(long branchId, CancellationToken ct)
    {
        var result = await mediator.Send(new GetTrainingSessionsByBranchMessage(branchId), ct);
        return ToActionResult(result);
    }
}
