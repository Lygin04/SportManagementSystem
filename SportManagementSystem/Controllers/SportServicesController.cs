using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SportManagementSystem.BuildingBlocks.Abstractions;
using SportManagementSystem.Modules.Services.Application.Commands.CreateSportService;
using SportManagementSystem.Modules.Services.Application.Queries.GetSportService;
using SportManagementSystem.Modules.Services.Application.Queries.GetSportServicesByBranch;
using SportManagementSystem.Modules.Services.Contracts.Requests;

namespace SportManagementSystem.Controllers;

public class SportServicesController(IMediator mediator) : ApiControllerV1WithAuth
{
    /// <summary>
    /// Создать спорт. услугу.
    /// </summary>
    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> Create(CreateSportServiceRequest request, CancellationToken ct)
    {
        var result = await mediator.Send(new CreateSportServiceMessage(UserId, request), ct);
        return result.IsSuccess
            ? Created(nameof(GetById), new { Id = result.Data})
            : ToActionResult(result);
    }

    /// <summary>
    /// Получить спорт. услугу по идентификатору.
    /// </summary>
    [HttpGet("{id:long}")]
    public async Task<IActionResult> GetById(long id, CancellationToken ct)
    {
        var result = await mediator.Send(new GetSportServiceMessage(id), ct);
        return ToActionResult(result);
    }

    /// <summary>
    /// Получить услуги филиала.
    /// </summary>
    [HttpGet("branch/{branchId:long}")]
    public async Task<IActionResult> GetByBranchId(long branchId, CancellationToken ct)
    {
        var result = await mediator.Send(new GetSportServicesByBranchMessage(branchId), ct);
        return ToActionResult(result);
    }
}
