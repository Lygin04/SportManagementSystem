using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SportManagementSystem.BuildingBlocks.Abstractions;
using SportManagementSystem.Modules.Images.Contracts.Requests;
using SportManagementSystem.Modules.Users.Application.Commands.RegisterBranchStaff;
using SportManagementSystem.Modules.Users.Application.Commands.UploadAvatarStaff;
using SportManagementSystem.Modules.Users.Application.Queries.GetBranchStaffs;
using SportManagementSystem.Modules.Users.Application.Queries.GetStaff;
using SportManagementSystem.Modules.Users.Contracts.Response;
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

    [AllowAnonymous]
    [HttpGet("branch/{branchId:long}/public")]
    public async Task<IActionResult> GetPublicByBranchId(long branchId, CancellationToken ct)
    {
        var result = await mediator.Send(new GetBranchStaffsMessage(branchId), ct);
        if (!result.IsSuccess)
        {
            return ToActionResult(result);
        }

        var staffs = result.Data.Select(staff => new PublicBranchStaffResponse
        {
            StaffId = staff.StaffId,
            Role = staff.Role,
            FirstName = staff.FirstName,
            LastName = staff.LastName,
            Patronymic = staff.Patronymic,
            AvatarId = staff.AvatarId,
        }).ToList();

        return Ok(staffs);
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
