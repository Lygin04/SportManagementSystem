using FluentValidation;
using SportManagementSystem.BuildingBlocks.Time;

namespace SportManagementSystem.Modules.Scheduling.Application.Commands.CreateTrainingSession;

public class CreateTrainingSessionValidator : AbstractValidator<CreateTrainingSessionMessage>
{
    public CreateTrainingSessionValidator(IAppClock clock)
    {
        RuleFor(x => x.Request.SportServiceId)
            .NotEmpty()
            .GreaterThan(0);

        RuleFor(x => x.Request.ScheduleId)
            .GreaterThan(0);
        
        RuleFor(x => x.Request.TrainerId)
            .NotEmpty()
            .GreaterThan(0);

        RuleFor(x => x.Request.TimeZoneId)
            .Must(BeValidTimeZone)
            .When(x => !string.IsNullOrWhiteSpace(x.Request.TimeZoneId))
            .WithMessage("Укажите корректный идентификатор часового пояса.");
        
        RuleFor(x => x.Request.StartedDate)
            .NotEmpty()
            .Must(x => x.ToUniversalTime() > clock.UtcNow);
        
        RuleFor(x => x.Request.EndedDate)
            .NotEmpty()
            .Must(x => x.ToUniversalTime() > clock.UtcNow);

        RuleFor(x => x.Request)
            .Must(request => request.EndedDate.ToUniversalTime() > request.StartedDate.ToUniversalTime())
            .WithMessage("Время окончания занятия должно быть позже времени начала.");
    }

    private static bool BeValidTimeZone(string? timeZoneId)
    {
        try
        {
            AppTimeZoneResolver.Resolve(timeZoneId);
            return true;
        }
        catch
        {
            return false;
        }
    }
}
