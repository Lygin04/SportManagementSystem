using System.Globalization;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SportManagementSystem.BuildingBlocks.Abstractions;
using SportManagementSystem.Modules.Analytics.Application.Commands.TrackBranchInteraction;
using SportManagementSystem.Modules.Analytics.Application.Queries.GetClientAnalytics;
using SportManagementSystem.Modules.Analytics.Application.Queries.GetManagerBranchAnalytics;
using SportManagementSystem.Modules.Analytics.Contracts.Requests;

namespace SportManagementSystem.Controllers;

public class AnalyticsController(IMediator mediator) : ApiControllerV1
{
    [Authorize]
    [HttpGet("clients/{clientId:long}")]
    public async Task<IActionResult> GetClientAnalytics(long clientId, CancellationToken ct)
    {
        var result = await mediator.Send(new GetClientAnalyticsMessage(clientId), ct);
        return ToActionResult(result);
    }

    [Authorize(Roles = "Admin,Manager")]
    [HttpGet("manager/branches")]
    public async Task<IActionResult> GetManagerBranchAnalytics(
        [FromQuery] string trendGranularity = AnalyticsTrendGranularities.Day,
        [FromQuery(Name = "date")] string? selectedDate = null,
        [FromQuery] long? branchId = null,
        CancellationToken ct = default)
    {
        var userId = TryGetCurrentUserId();
        var role = TryGetCurrentRole();
        if (!userId.HasValue || string.IsNullOrWhiteSpace(role))
        {
            return Unauthorized();
        }

        DateOnly? parsedDate = null;
        if (!string.IsNullOrWhiteSpace(selectedDate))
        {
            if (!DateOnly.TryParseExact(selectedDate, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var parsedDateValue))
            {
                return BadRequest(new
                {
                    title = "Invalid date",
                    detail = "Use yyyy-MM-dd format for date query parameter."
                });
            }

            parsedDate = parsedDateValue;
        }

        var result = await mediator.Send(
            new GetManagerBranchAnalyticsMessage(userId.Value, role, trendGranularity, parsedDate, branchId),
            ct);
        return ToActionResult(result);
    }

    [AllowAnonymous]
    [HttpPost("branches/interactions")]
    public async Task<IActionResult> TrackBranchInteraction(
        TrackBranchInteractionRequest request,
        CancellationToken ct)
    {
        var userId = TryGetCurrentUserId();
        var role = TryGetCurrentRole();

        var result = await mediator.Send(new TrackBranchInteractionMessage(userId, role, request), ct);
        return ToActionResult(result);
    }

    private long? TryGetCurrentUserId()
    {
        if (User?.Identity?.IsAuthenticated != true)
        {
            return null;
        }

        var idClaim = User.Claims.FirstOrDefault(c =>
            c.Type.EndsWith("/nameidentifier", StringComparison.OrdinalIgnoreCase) ||
            c.Type.EndsWith("/sid", StringComparison.OrdinalIgnoreCase) ||
            string.Equals(c.Type, "id", StringComparison.OrdinalIgnoreCase));

        return long.TryParse(idClaim?.Value, out var userId) ? userId : null;
    }

    private string? TryGetCurrentRole()
    {
        if (User?.Identity?.IsAuthenticated != true)
        {
            return null;
        }

        return User.Claims.FirstOrDefault(c =>
            c.Type.EndsWith("role", StringComparison.OrdinalIgnoreCase) ||
            string.Equals(c.Type, "role", StringComparison.OrdinalIgnoreCase))?.Value;
    }
}
