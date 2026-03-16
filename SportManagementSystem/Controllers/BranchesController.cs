using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SportManagementSystem.BuildingBlocks.Abstractions;
using SportManagementSystem.Modules.Assets.Application.Commands.CreateBranch;
using SportManagementSystem.Modules.Assets.Application.Commands.DeleteImageBranch;
using SportManagementSystem.Modules.Assets.Application.Commands.UploadImageBranch;
using SportManagementSystem.Modules.Assets.Application.Queries.GetBranch;
using SportManagementSystem.Modules.Assets.Application.Queries.GetBranches;
using SportManagementSystem.Modules.Assets.Application.Queries.GetBranchesByAdmin;
using SportManagementSystem.Modules.Assets.Contracts.Request;
using SportManagementSystem.Modules.Images.Contracts.Requests;

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
        var result = await mediator.Send(new CreateBranchMessage(UserId, request), cancellationToken);
        return result.IsSuccess
            ? Created(nameof(GetById), new { Id = result.Data })
            : ToActionResult(result);
    }
    
    /// <summary>
    /// Получить филиал по идентификатору.
    /// </summary>
    [AllowAnonymous]
    [HttpGet("{id:long}")]
    public async Task<IActionResult> GetById(long id, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetBranchMessage(id), cancellationToken);
        return ToActionResult(result);
    }

    /// <summary>
    /// Получить все филиалы.
    /// </summary>
    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetBranchesMessage(), cancellationToken);
        return ToActionResult(result);
    }

    /// <summary>
    /// Администратор получает список филиалов которые создал.
    /// </summary>
    [Authorize(Roles = "Admin")]
    [HttpGet("me")]
    public async Task<IActionResult> GetByAdmin(CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetBranchesByAdminMessage(UserId), cancellationToken);
        return ToActionResult(result);
    }

    /// <summary>
    /// Загрузить изображение филиала.
    /// </summary>
    [Authorize(Roles = "Admin")]
    [HttpPost("{id:long}/image")]
    public async Task<IActionResult> UploadImageBranch([FromRoute] long id, [FromForm] UploadImageRequest request,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new UploadImageBranchMessage(id, request), cancellationToken);
        return ToActionResult(result);
    }

    /// <summary>
    /// Удаление изображения филиала.
    /// </summary>
    [Authorize(Roles = "Admin")]
    [HttpDelete("{id:long}/image/{imageId:guid}")]
    public async Task<IActionResult> DeleteImageBranch([FromRoute] long id, [FromRoute] Guid imageId,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new DeleteImageBranchMessage(id, imageId), cancellationToken);
        return ToActionResult(result);
    }
}
