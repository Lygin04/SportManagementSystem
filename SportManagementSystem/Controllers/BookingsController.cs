using MediatR;
using Microsoft.AspNetCore.Mvc;
using SportManagementSystem.BuildingBlocks.Abstractions;
using SportManagementSystem.Modules.Clients.Application.Commands.CreateBooking;
using SportManagementSystem.Modules.Clients.Application.Queries.GetBooking;
using SportManagementSystem.Modules.Clients.Contracts.Requests;

namespace SportManagementSystem.Controllers;

public class BookingsController(IMediator mediator) : ApiControllerV1WithAuth
{
    [HttpPost]
    public async Task<IActionResult> Create(CreateBookingRequest request, CancellationToken ct)
    {
        var result = await mediator.Send(new CreateBookingMessage(UserId, request), ct);
        return result.IsSuccess
            ? Created(nameof(GetById), new { Id = result.Data})
            : ToActionResult(result);
    }

    [HttpGet("{id:long}")]
    public async Task<IActionResult> GetById(long id, CancellationToken ct)
    {
        var result = await mediator.Send(new GetBookingMessage(id), ct);
        return ToActionResult(result);
    }
}