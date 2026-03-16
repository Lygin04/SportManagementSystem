using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SportManagementSystem.BuildingBlocks.Abstractions;
using SportManagementSystem.Modules.Scheduling.Application.Commands.CreateServiceSchedule;
using SportManagementSystem.Modules.Scheduling.Application.Queries.GetServiceSchedule;
using SportManagementSystem.Modules.Scheduling.Application.Queries.GetServiceSchedulesByBranch;
using SportManagementSystem.Modules.Scheduling.Contracts.Requests;

namespace SportManagementSystem.Controllers;

public class ServiceSchedulesController(IMediator mediator) : ApiControllerV1WithAuth
{
    /// <summary>
    /// Создать расписание тренировки.
    /// </summary>
    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> Create(CreateServiceScheduleRequest request, CancellationToken ct)
    {
        var result = await mediator.Send(new CreateServiceScheduleMessage(request), ct);
        return result.IsSuccess
            ? Created(nameof(GetById), new { Id = result.Data})
            : ToActionResult(result);
    }

    /// <summary>
    /// Получить расписание тренировки по идентификатору.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetById(long id, CancellationToken ct)
    {
        var result = await mediator.Send(new GetServiceScheduleMessage(id), ct);
        return ToActionResult(result);
    }

    /// <summary>
    /// Получить расписания услуг по филиалу.
    /// </summary>
    [HttpGet("branch/{branchId:long}")]
    public async Task<IActionResult> GetByBranchId(long branchId, CancellationToken ct)
    {
        var result = await mediator.Send(new GetServiceSchedulesByBranchMessage(branchId), ct);
        return ToActionResult(result);
    }
}
