namespace SportManagementSystem.Modules.Scheduling.Contracts.Requests;

public class CreateTrainingSessionRequest
{
    public long SportServiceId { get; set; }

    public long? ScheduleId { get; set; }

    public string? TimeZoneId { get; set; }

    /// <summary>
    /// Дата и время начала занятия с часовым поясом или смещением.
    /// </summary>
    public DateTimeOffset StartedDate { get; set; }
    
    /// <summary>
    /// Дата и время конца занятия с часовым поясом или смещением.
    /// </summary>
    public DateTimeOffset EndedDate { get; set; }
    
    public long TrainerId { get; set; }
}
