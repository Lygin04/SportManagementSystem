using FluentValidation;

namespace SportManagementSystem.Modules.Clients.Application.Commands.CreateMembershipTemplate;

public class CreateMembershipTemplateValidator : AbstractValidator<CreateMembershipTemplateMessage>
{
    public CreateMembershipTemplateValidator()
    {
        RuleFor(x => x.Request.BranchId).GreaterThan(0);
        RuleFor(x => x.Request.SportServiceId).GreaterThan(0);
        RuleFor(x => x.Request.ServicePriceId).GreaterThan(0);
        RuleFor(x => x.Request.Name).NotEmpty();
        RuleFor(x => x.Request.DurationDays).GreaterThan(0);
        RuleFor(x => x.Request.VisitLimit).GreaterThan(0);
    }
}
