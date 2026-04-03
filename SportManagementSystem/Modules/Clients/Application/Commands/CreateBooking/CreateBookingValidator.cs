using FluentValidation;
using SportManagementSystem.BuildingBlocks.Time;

namespace SportManagementSystem.Modules.Clients.Application.Commands.CreateBooking;

public class CreateBookingValidator : AbstractValidator<CreateBookingMessage>
{
    public CreateBookingValidator(IAppClock clock)
    {
        RuleFor(x => x.ClientId)
            .NotEmpty()
            .GreaterThan(0);
        
        RuleFor(x => x.Request.SessionId)
            .NotEmpty()
            .GreaterThan(0);

        RuleFor(x => x.Request.TimeZoneId)
            .Must(BeValidTimeZone)
            .When(x => !string.IsNullOrWhiteSpace(x.Request.TimeZoneId))
            .WithMessage("Укажите корректный идентификатор часового пояса.");

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
