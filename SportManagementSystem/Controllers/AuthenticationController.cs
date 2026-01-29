using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SportManagementSystem.BuildingBlocks.Abstractions;
using SportManagementSystem.Modules.Users.Application.Commands.LoginUser;
using SportManagementSystem.Modules.Users.Application.Commands.RegisterClient;
using SportManagementSystem.Modules.Users.Application.Commands.RegisterStaff;
using SportManagementSystem.Modules.Users.Contracts.Requests;

namespace SportManagementSystem.Controllers;

public class AuthenticationController(IMediator mediator) : ApiControllerV1
{
    [HttpPost("register")]
    public async Task<IActionResult> RegisterClient(RegisterClientRequest request, CancellationToken ct)
    {
        var result = await mediator.Send(new RegisterClientMessage(request), ct);
        
        return result.IsSuccess
            ? Created("", new { result.IsSuccess, result.Data, result.Error })
            : ToActionResult(result);
    }

    [HttpPost("register-staff")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> RegisterStaff(RegisterStaffRequest request, CancellationToken ct)
    {
        var result = await mediator.Send(new RegisterStaffMessage(request), ct);
        
        return result.IsSuccess
            ? Created("", new { result.IsSuccess, result.Data, result.Error })
            : ToActionResult(result);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginUserRequest request, CancellationToken ct)
    {
        var result = await mediator.Send(new LoginUserMessage(request), ct);
        return ToActionResult(result);
    }
}