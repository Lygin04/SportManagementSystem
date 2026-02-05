using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SportManagementSystem.BuildingBlocks.Abstractions;
using SportManagementSystem.Modules.Assets.Application.Commands.CreateRoom;
using SportManagementSystem.Modules.Assets.Application.Queries.GetRoom;
using SportManagementSystem.Modules.Assets.Application.Queries.GetRoomByBranch;
using SportManagementSystem.Modules.Assets.Contracts.Request;

namespace SportManagementSystem.Controllers;

public class RoomsController(IMediator mediator) : ApiControllerV1WithAuth
{
    /// <summary>
    /// Создать помещение в спорт. орг..
    /// </summary>
    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> Create(CreateRoomRequest request, CancellationToken ct)
    {
        var result = await mediator.Send(new CreateRoomMessage(request), ct);
        return result.IsSuccess
            ? Created(nameof(GetById), new { Id = result.Data})
            : ToActionResult(result);    
    }
    
    /// <summary>
    /// Получить помещение в спорт. орг. по идентификатору.
    /// </summary>
    [HttpGet("{id:long}")]
    public async Task<IActionResult> GetById(long id, CancellationToken ct)
    {
        var result = await mediator.Send(new GetRoomMessage(id), ct);
        return ToActionResult(result);
    }

    /// <summary>
    /// Получить все помещения в филиале спорт. орг.
    /// </summary>
    [HttpGet("branch/{branchId:long}")]
    public async Task<IActionResult> GetByBranchId(long branchId, CancellationToken ct)
    {
        var result = await mediator.Send(new GetRoomByBranchMessage(branchId), ct);
        return ToActionResult(result);
    }
}