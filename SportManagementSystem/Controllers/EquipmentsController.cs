using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SportManagementSystem.BuildingBlocks.Abstractions;
using SportManagementSystem.Modules.Assets.Application.Commands.CreateEquipment;
using SportManagementSystem.Modules.Assets.Application.Queries.GetEquipment;
using SportManagementSystem.Modules.Assets.Contracts.Request;

namespace SportManagementSystem.Controllers;

public class EquipmentsController(IMediator mediator) : ApiControllerV1WithAuth
{
    /// <summary>
    /// Создать спортивное оборудование.
    /// </summary>
    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> Create(CreateEquipmentRequest request, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new CreateEquipmentMessage(request), cancellationToken);
        return result.IsSuccess
            ? Created(nameof(GetById), new { Id = result.Data})
            : ToActionResult(result);    
    }
    
    /// <summary>
    /// Получить спортивное оборудование по идентификатору.
    /// </summary>
    [HttpGet("{id:long}")]
    public async Task<IActionResult> GetById(long id, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetEquipmentMessage(id), cancellationToken);
        return ToActionResult(result);
    }
}