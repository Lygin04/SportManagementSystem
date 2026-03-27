using FluentValidation;

namespace SportManagementSystem.Modules.Clients.Application.Commands.CreateMembership;

public class CreateMembershipValidator : AbstractValidator<CreateMembershipMessage>
{
    public CreateMembershipValidator()
    {
        RuleFor(x => x.Request.ClientId)
            .Must((message, clientId) => !IsManager(message) || clientId is > 0)
            .WithMessage("Укажите корректный идентификатор клиента");

        RuleFor(x => x.Request.MembershipTemplateId)
            .GreaterThan(0)
            .When(x => x.Request.MembershipTemplateId.HasValue);

        RuleFor(x => x.Request.SportServiceId)
            .NotEmpty()
            .GreaterThan(0)
            .When(x => !x.Request.MembershipTemplateId.HasValue);

        RuleFor(x => x.Request.StartDate)
            .NotEmpty()
            .Must(x => x > DateOnly.Parse(DateTime.UtcNow.ToString("yyyy-MM-dd")));
        
        RuleFor(x => x.Request.EndDate)
            .NotEmpty()
            .Must(x => x > DateOnly.Parse(DateTime.UtcNow.AddDays(1).ToString("yyyy-MM-dd")))
            .When(x => !x.Request.MembershipTemplateId.HasValue);
        
        RuleFor(x => x.Request.RemainingVisits)
            .GreaterThan(0)
            .When(x => x.Request.RemainingVisits.HasValue);
    }

    private static bool IsManager(CreateMembershipMessage message)
    {
        return string.Equals(message.Role, "Manager", StringComparison.OrdinalIgnoreCase);
    }
}
