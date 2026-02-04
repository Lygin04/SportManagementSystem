namespace SportManagementSystem.Modules.Scheduling.Contracts.Requests;

public class CreateTrainingSessionRequest
{
    public long SportServiceId { get; set; }

    public long? ScheduleId { get; set; }

    /// <summary>
    /// Дата и время начала занятия.
    /// </summary>
    public DateTime StartedDate { get; set; }
    
    /// <summary>
    /// Дата и время конца занятия.
    /// </summary>
    public DateTime EndedDate { get; set; }
    
    public long TrainerId { get; set; }
}