using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SportManagementSystem.BuildingBlocks.Abstractions;
using SportManagementSystem.Modules.Services.Application.Commands.CreateServicePrice;
using SportManagementSystem.Modules.Services.Application.Queries.GetServicePrice;
using SportManagementSystem.Modules.Services.Contracts.Requests;

namespace SportManagementSystem.Controllers;

public class ServicePricesController(IMediator mediator) : ApiControllerV1WithAuth
{
    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> Create(CreateServicePriceRequest request, CancellationToken ct)
    {
        var result = await mediator.Send(new CreateServicePriceMessage(request), ct);
        return result.IsSuccess
            ? Created(nameof(GetById), new { Id = result.Data})
            : ToActionResult(result);
    }

    [HttpGet("{id:long}")]
    public async Task<IActionResult> GetById(long id, CancellationToken ct)
    {
        var result = await mediator.Send(new GetServicePriceMessage(id), ct);
        return ToActionResult(result);
    }
}