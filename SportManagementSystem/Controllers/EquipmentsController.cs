using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SportManagementSystem.BuildingBlocks.Abstractions;
using SportManagementSystem.Modules.Assets.Application.Commands.CreateEquipment;
using SportManagementSystem.Modules.Assets.Application.Commands.DeleteEquipment;
using SportManagementSystem.Modules.Assets.Application.Commands.DeleteImageEquipment;
using SportManagementSystem.Modules.Assets.Application.Commands.UploadImageEquipment;
using SportManagementSystem.Modules.Assets.Application.Queries.GetEquipment;
using SportManagementSystem.Modules.Assets.Application.Queries.GetEquipmentByRoom;
using SportManagementSystem.Modules.Assets.Contracts.Request;
using SportManagementSystem.Modules.Images.Contracts.Requests;

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

    /// <summary>
    /// Получить оборудование по помещению.
    /// </summary>
    [HttpGet("room/{roomId:long}")]
    public async Task<IActionResult> GetByRoom(long roomId, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetEquipmentByRoomMessage(roomId), cancellationToken);
        return ToActionResult(result);
    }

    /// <summary>
    /// Загрузить изображение оборудования.
    /// </summary>
    [Authorize(Roles = "Admin")]
    [HttpPost("{id:long}/image")]
    public async Task<IActionResult> UploadImageEquipment([FromRoute] long id, [FromForm] UploadImageRequest request, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new UploadImageEquipmentMessage(id, request), cancellationToken);
        return ToActionResult(result);
    }

    /// <summary>
    /// Удаление изображения оборудования.
    /// </summary>
    [Authorize(Roles = "Admin")]
    [HttpDelete("{id:long}/image/{imageId:guid}")]
    public async Task<IActionResult> DeleteImageEquipment([FromRoute] long id, [FromRoute] Guid imageId, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new DeleteImageEquipmentMessage(id, imageId), cancellationToken);
        return ToActionResult(result);
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id:long}")]
    public async Task<IActionResult> Delete([FromRoute] long id, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new DeleteEquipmentMessage(id), cancellationToken);
        return ToActionResult(result);
    }
}
