using SportManagementSystem.Modules.Scheduling.Application.Commands.CreateServiceSchedule;
using SportManagementSystem.Modules.Scheduling.Application.Commands.CreateTrainingSession;
using SportManagementSystem.Modules.Scheduling.Contracts.Requests;

namespace SportManagementSystem.Tests.Modules.Scheduling.Unit;

public class SchedulingValidatorsTests
{
    [Fact]
    public void CreateServiceScheduleValidator_WhenRequestInvalid_ReturnsErrors()
    {
        var validator = new CreateServiceScheduleValidator();
        var message = new CreateServiceScheduleMessage(new CreateServiceScheduleRequest
        {
            SportServiceId = 0,
            StaffId = 0,
            RoomId = 0,
            DayOfWeek = 0,
            StartTime = default,
            EndTime = default
        });

        var result = validator.Validate(message);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "Request.SportServiceId");
        Assert.Contains(result.Errors, e => e.PropertyName == "Request.StaffId");
        Assert.Contains(result.Errors, e => e.PropertyName == "Request.RoomId");
        Assert.Contains(result.Errors, e => e.PropertyName == "Request.DayOfWeek");
        Assert.Contains(result.Errors, e => e.PropertyName == "Request.StartTime");
        Assert.Contains(result.Errors, e => e.PropertyName == "Request.EndTime");
    }

    [Fact]
    public void CreateTrainingSessionValidator_WhenRequestInvalid_ReturnsErrors()
    {
        var validator = new CreateTrainingSessionValidator();
        var message = new CreateTrainingSessionMessage(new CreateTrainingSessionRequest
        {
            SportServiceId = 0,
            ScheduleId = 0,
            TrainerId = 0,
            StartedDate = DateTime.UtcNow.AddMinutes(-10),
            EndedDate = DateTime.UtcNow.AddMinutes(-5)
        });

        var result = validator.Validate(message);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "Request.SportServiceId");
        Assert.Contains(result.Errors, e => e.PropertyName == "Request.ScheduleId");
        Assert.Contains(result.Errors, e => e.PropertyName == "Request.TrainerId");
        Assert.Contains(result.Errors, e => e.PropertyName == "Request.StartedDate");
        Assert.Contains(result.Errors, e => e.PropertyName == "Request.EndedDate");
    }

    [Fact]
    public void CreateTrainingSessionValidator_WhenRequestValid_HasNoErrors()
    {
        var validator = new CreateTrainingSessionValidator();
        var message = new CreateTrainingSessionMessage(new CreateTrainingSessionRequest
        {
            SportServiceId = 1,
            ScheduleId = 2,
            TrainerId = 3,
            StartedDate = DateTime.UtcNow.AddHours(1),
            EndedDate = DateTime.UtcNow.AddHours(2)
        });

        var result = validator.Validate(message);

        Assert.True(result.IsValid);
    }
}
