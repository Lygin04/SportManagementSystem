using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SportManagementSystem.BuildingBlocks.Abstractions;
using SportManagementSystem.Modules.Scheduling.Application.Commands.CreateServiceSchedule;
using SportManagementSystem.Modules.Scheduling.Application.Queries.GetServiceSchedule;
using SportManagementSystem.Modules.Scheduling.Application.Queries.GetServiceSchedulesByBranch;
using SportManagementSystem.Modules.Scheduling.Contracts.Response;
using SportManagementSystem.Modules.Scheduling.Contracts.Requests;
using SportManagementSystem.Modules.Scheduling.Domain.Entities;

namespace SportManagementSystem.Controllers;

public class ServiceSchedulesController(IMediator mediator) : ApiControllerV1WithAuth
{
    /// <summary>
    /// Создать расписание тренировки.
    /// </summary>
    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> Create(CreateServiceScheduleRequest request, CancellationToken ct)
    {
        var result = await mediator.Send(new CreateServiceScheduleMessage(request), ct);
        return result.IsSuccess
            ? Created(nameof(GetById), new { Id = result.Data})
            : ToActionResult(result);
    }

    /// <summary>
    /// Получить расписание тренировки по идентификатору.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetById(long id, CancellationToken ct)
    {
        var result = await mediator.Send(new GetServiceScheduleMessage(id), ct);
        return ToActionResult(result);
    }

    /// <summary>
    /// Получить расписания услуг по филиалу.
    /// </summary>
    [AllowAnonymous]
    [HttpGet("branch/{branchId:long}")]
    public async Task<IActionResult> GetByBranchId(long branchId, CancellationToken ct)
    {
        var result = await mediator.Send(new GetServiceSchedulesByBranchMessage(branchId), ct);
        if (!result.IsSuccess)
        {
            return ToActionResult(result);
        }

        return Ok(result.Data.Select(MapServiceSchedule).ToList());
    }

    private static ServiceScheduleResponse MapServiceSchedule(DbServiceSchedule schedule)
    {
        return new ServiceScheduleResponse
        {
            Id = schedule.Id,
            SportServiceId = schedule.SportServiceId,
            SportService = schedule.SportService is null
                ? null
                : new SportServiceShortResponse
                {
                    Id = schedule.SportService.Id,
                    Name = schedule.SportService.Name,
                    Category = schedule.SportService.Category?.ToString(),
                    Description = schedule.SportService.Description,
                },
            StaffId = schedule.StaffId,
            Staff = schedule.Staff is null
                ? null
                : new BranchStaffShortResponse
                {
                    Id = schedule.Staff.Id,
                    FirstName = schedule.Staff.FirstName,
                    LastName = schedule.Staff.LastName,
                    Patronymic = schedule.Staff.Patronymic,
                },
            RoomId = schedule.RoomId,
            Room = schedule.Room is null
                ? null
                : new RoomShortResponse
                {
                    Id = schedule.Room.Id,
                    Name = schedule.Room.Name,
                    Capacity = schedule.Room.Capacity,
                },
            DayOfWeek = schedule.DayOfWeek,
            StartTime = schedule.StartTime,
            EndTime = schedule.EndTime,
            Note = schedule.Note,
        };
    }
}
