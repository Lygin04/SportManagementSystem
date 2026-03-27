namespace SportManagementSystem.Modules.Scheduling.Contracts.Response;

public class ServiceScheduleResponse
{
    public long Id { get; set; }
    public long SportServiceId { get; set; }
    public SportServiceShortResponse? SportService { get; set; }
    public long? StaffId { get; set; }
    public BranchStaffShortResponse? Staff { get; set; }
    public long? RoomId { get; set; }
    public RoomShortResponse? Room { get; set; }
    public int DayOfWeek { get; set; }
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }
    public string? Note { get; set; }
}

public class SportServiceShortResponse
{
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Category { get; set; }
    public string? Description { get; set; }
}

public class BranchStaffShortResponse
{
    public long Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string? Patronymic { get; set; }
}

public class RoomShortResponse
{
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int? Capacity { get; set; }
}
