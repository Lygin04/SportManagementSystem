using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SportManagementSystem.BuildingBlocks.Abstractions;
using SportManagementSystem.Modules.Images.Contracts.Requests;
using SportManagementSystem.Modules.Users.Application.Commands.UploadAvatarClient;
using SportManagementSystem.Modules.Users.Application.Queries.GetClient;

namespace SportManagementSystem.Controllers;

[ApiController]
[Route("v1/[controller]")]
public class ClientsController(IMediator mediator) : ApiControllerV1WithAuth
{
    /// <summary>
    /// Получить пользователя по идентификатору.
    /// </summary>
    [HttpGet("{id:long}")]
    public async Task<IActionResult> GetById(long id, CancellationToken ct)
    {
        var result = await mediator.Send(new GetClientMessage(id), ct);
        return ToActionResult(result);
    }

    /// <summary>
    /// Получить свои данные.
    /// </summary>
    [Authorize(Roles = "Client")]
    [HttpGet("me")]
    public async Task<IActionResult> GetMe(CancellationToken ct)
    {
        var result = await mediator.Send(new GetClientMessage(AccountId), ct);
        return ToActionResult(result);
    }

    /// <summary>
    /// Добавляет или обновляет аватар клиента.
    /// </summary>
    [Authorize(Roles = "Client")]
    [HttpPost("avatar")]
    public async Task<IActionResult> UploadAvatar([FromForm] UploadImageRequest request, CancellationToken ct)
    {
        var result = await mediator.Send(new UploadAvatarClientMessage(UserId, request), ct);
        return ToActionResult(result);
    }
}