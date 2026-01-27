namespace SportManagementSystem.Entities;

public class DbServiceSchedule
{
    public long Id { get; set; }
    public long SportServiceId { get; set; }
    public DbSportService SportService { get; set; }

    public long? StaffId { get; set; }
    public DbStaff? Staff { get; set; }

    public long? RoomId { get; set; }
    public DbRoom? Room { get; set; }

    public int DayOfWeek { get; set; }
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }
    public string? Note { get; set; }
}