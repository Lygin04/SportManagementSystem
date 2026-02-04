using FluentValidation;

namespace SportManagementSystem.Modules.Scheduling.Application.Commands.CreateServiceSchedule;

public class CreateServiceScheduleValidator : AbstractValidator<CreateServiceScheduleMessage>
{
    public CreateServiceScheduleValidator()
    {
        RuleFor(x => x.Request.SportServiceId)
            .NotEmpty()
            .GreaterThan(0);
        
        RuleFor(x => x.Request.StaffId)
            .GreaterThan(0);
        
        RuleFor(x => x.Request.RoomId)
            .NotEmpty()
            .GreaterThan(0);

        RuleFor(x => x.Request.DayOfWeek)
            .NotEmpty()
            .InclusiveBetween(1, 7);

        RuleFor(x => x.Request.StartTime)
            .NotEmpty();

        RuleFor(x => x.Request.EndTime)
            .NotEmpty();
    }
}