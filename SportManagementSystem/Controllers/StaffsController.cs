using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SportManagementSystem.BuildingBlocks.Abstractions;
using SportManagementSystem.Modules.Images.Contracts.Requests;
using SportManagementSystem.Modules.Users.Application.Commands.UploadAvatarStaff;
using SportManagementSystem.Modules.Users.Application.Queries.GetStaff;

namespace SportManagementSystem.Controllers;

public class StaffsController(IMediator mediator) : ApiControllerV1WithAuth
{
    /// <summary>
    /// Получить информацию о себе как сотруднике.
    /// </summary>
    [Authorize(Roles = "Admin, Manager, Trainer, Cashier")]
    [HttpGet("me")]
    public async Task<IActionResult> GetMe()
    {
        var result = await mediator.Send(new GetStaffMessage(UserId));
        return ToActionResult(result);
    }

    /// <summary>
    /// Получить информацию о сотруднике по идентификатору.
    /// </summary>
    [Authorize(Roles = "Admin, Manager, Trainer, Cashier")]
    [HttpGet("{id:long}")]
    public async Task<IActionResult> GetById(long id)
    {
        var result = await mediator.Send(new GetStaffMessage(id));
        return ToActionResult(result);
    }

    /// <summary>
    /// Загрузить аватар сотрудника.
    /// </summary>
    [Authorize(Roles = "Admin, Manager, Trainer, Cashier")]
    [HttpPost("avatar")]
    public async Task<IActionResult> UploadAvatar([FromForm] UploadImageRequest request, CancellationToken ct)
    {
        var result = await mediator.Send(new UploadAvatarStaffMessage(UserId, request), ct);
        return ToActionResult(result);
    }
}