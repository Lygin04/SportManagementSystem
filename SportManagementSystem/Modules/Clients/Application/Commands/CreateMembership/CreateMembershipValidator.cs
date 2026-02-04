using FluentValidation;

namespace SportManagementSystem.Modules.Clients.Application.Commands.CreateMembership;

public class CreateMembershipValidator : AbstractValidator<CreateMembershipMessage>
{
    public CreateMembershipValidator()
    {
        RuleFor(x => x.Request.SportServiceId)
            .NotEmpty()
            .GreaterThan(0);

        RuleFor(x => x.Request.StartDate)
            .NotEmpty()
            .Must(x => x > DateOnly.Parse(DateTime.UtcNow.ToString("yyyy-MM-dd")));
        
        RuleFor(x => x.Request.EndDate)
            .NotEmpty()
            .Must(x => x > DateOnly.Parse(DateTime.UtcNow.AddDays(1).ToString("yyyy-MM-dd")));
        
        RuleFor(x => x.Request.RemainingVisits)
            .GreaterThan(0);
    }
}