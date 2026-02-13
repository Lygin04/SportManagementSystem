using MediatR;
using Microsoft.AspNetCore.Mvc;
using SportManagementSystem.BuildingBlocks.Abstractions;
using SportManagementSystem.Modules.Users.Application.Queries.GetUser;

namespace SportManagementSystem.Controllers;

[ApiController]
[Route("v1/[controller]")]
public class UsersController(IMediator mediator) : ApiControllerV1WithAuth
{
    /// <summary>
    /// Получить пользователя по идентификатору.
    /// </summary>
    [HttpGet("{id:long}")]
    public async Task<IActionResult> GetById(long id, CancellationToken ct)
    {
        var result = await mediator.Send(new GetUserMessage(id), ct);
        return ToActionResult(result);
    }

    /// <summary>
    /// Получить свои данные.
    /// </summary>
    [HttpGet("me")]
    public async Task<IActionResult> GetMe(CancellationToken ct)
    {
        var result = await mediator.Send(new GetUserMessage(UserId), ct);
        return ToActionResult(result);
    }
}