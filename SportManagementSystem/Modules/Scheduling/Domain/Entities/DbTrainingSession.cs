using SportManagementSystem.Modules.Scheduling.Domain.Enums;
using SportManagementSystem.Modules.Services.Domain.Entities;
using SportManagementSystem.Modules.Users.Domain.Entities;

namespace SportManagementSystem.Modules.Scheduling.Domain.Entities;

/// <summary>
/// Конкретное занятие (фактическое событие).
/// </summary>
public class DbTrainingSession
{
    public long Id { get; set; }
    public long SportServiceId { get; set; }
    public DbSportService SportService { get; set; }

    public long? ScheduleId { get; set; }
    public DbServiceSchedule? Schedule { get; set; }

    public string TimeZoneId { get; set; } = "UTC";

    /// <summary>
    /// Дата и время начала занятия.
    /// </summary>
    public DateTimeOffset StartedDate { get; set; }
    
    /// <summary>
    /// Дата и время конца занятия.
    /// </summary>
    public DateTimeOffset EndedDate { get; set; }
    
    public long TrainerId { get; set; }
    public DbStaff Trainer { get; set; }

    /// <summary>
    /// Статус занятия.
    /// </summary>
    public ESessionStatus Status { get; set; } = ESessionStatus.Planned;
}
