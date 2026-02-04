using FluentValidation;

namespace SportManagementSystem.Modules.Scheduling.Application.Commands.CreateTrainingSession;

public class CreateTrainingSessionValidator : AbstractValidator<CreateTrainingSessionMessage>
{
    public CreateTrainingSessionValidator()
    {
        RuleFor(x => x.Request.SportServiceId)
            .NotEmpty()
            .GreaterThan(0);

        RuleFor(x => x.Request.ScheduleId)
            .GreaterThan(0);
        
        RuleFor(x => x.Request.TrainerId)
            .NotEmpty()
            .GreaterThan(0);
        
        RuleFor(x => x.Request.StartedDate)
            .NotEmpty()
            .GreaterThan(DateTime.Now);
        
        RuleFor(x => x.Request.EndedDate)
            .NotEmpty()
            .GreaterThan(DateTime.Now);
    }
}