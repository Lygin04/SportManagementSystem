using SportManagementSystem.Entities.Enums;

namespace SportManagementSystem.Entities;

public class DbTrainingSession
{
    public long Id { get; set; }
    public long SportServiceId { get; set; }
    public DbSportService SportService { get; set; }

    public long? ScheduleId { get; set; }
    public DbServiceSchedule? Schedule { get; set; }

    public DateOnly Date { get; set; }
    public long? TrainerId { get; set; }
    public DbStaff? Trainer { get; set; }

    public ESessionStatus Status { get; set; } = ESessionStatus.Planned;
}