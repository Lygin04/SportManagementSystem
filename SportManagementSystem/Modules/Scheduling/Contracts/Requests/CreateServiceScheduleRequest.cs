namespace SportManagementSystem.Modules.Scheduling.Contracts.Requests;

public class CreateServiceScheduleRequest
{
    public long SportServiceId { get; set; }

    public long? StaffId { get; set; }

    public long RoomId { get; set; }

    public int DayOfWeek { get; set; }
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }
    public string? Note { get; set; }
}