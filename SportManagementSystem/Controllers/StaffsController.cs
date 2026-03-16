using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SportManagementSystem.BuildingBlocks.Abstractions;
using SportManagementSystem.Modules.Images.Contracts.Requests;
using SportManagementSystem.Modules.Users.Application.Commands.RegisterBranchStaff;
using SportManagementSystem.Modules.Users.Application.Commands.UploadAvatarStaff;
using SportManagementSystem.Modules.Users.Application.Queries.GetBranchStaffs;
using SportManagementSystem.Modules.Users.Application.Queries.GetStaff;
using SportManagementSystem.Modules.Users.Contracts.Requests;

namespace SportManagementSystem.Controllers;

public class StaffsController(IMediator mediator) : ApiControllerV1WithAuth
{
    [Authorize(Roles = "Admin, Manager, Trainer, Cashier")]
    [HttpGet("me")]
    public async Task<IActionResult> GetMe()
    {
        var result = await mediator.Send(new GetStaffMessage(AccountId));
        return ToActionResult(result);
    }

    [Authorize(Roles = "Admin, Manager, Trainer, Cashier")]
    [HttpGet("{id:long}")]
    public async Task<IActionResult> GetById(long id)
    {
        var result = await mediator.Send(new GetStaffMessage(id));
        return ToActionResult(result);
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("branch/{branchId:long}")]
    public async Task<IActionResult> GetByBranchId(long branchId, CancellationToken ct)
    {
        var result = await mediator.Send(new GetBranchStaffsMessage(branchId), ct);
        return ToActionResult(result);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost("branch/{branchId:long}")]
    public async Task<IActionResult> RegisterByBranchId(long branchId, RegisterStaffRequest request, CancellationToken ct)
    {
        var result = await mediator.Send(new RegisterBranchStaffMessage(branchId, request), ct);
        return result.IsSuccess
            ? Created("", new { result.IsSuccess, result.Data, result.Error })
            : ToActionResult(result);
    }

    [Authorize(Roles = "Admin, Manager, Trainer, Cashier")]
    [HttpPost("avatar")]
    public async Task<IActionResult> UploadAvatar([FromForm] UploadImageRequest request, CancellationToken ct)
    {
        var result = await mediator.Send(new UploadAvatarStaffMessage(UserId, request), ct);
        return ToActionResult(result);
    }
}
