using FluentValidation;

namespace SportManagementSystem.Modules.Clients.Application.Commands.CreateBooking;

public class CreateBookingValidator : AbstractValidator<CreateBookingMessage>
{
    public CreateBookingValidator()
    {
        RuleFor(x => x.ClientId)
            .NotEmpty()
            .GreaterThan(0);
        
        RuleFor(x => x.Request.SessionId)
            .NotEmpty()
            .GreaterThan(0);

        RuleFor(x => x.Request.Booked)
            .NotEmpty()
            .Must(x => x > DateTime.Now);
    }
}