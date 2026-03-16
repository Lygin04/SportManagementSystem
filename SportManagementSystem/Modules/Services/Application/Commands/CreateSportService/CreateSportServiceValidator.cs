using FluentValidation;

namespace SportManagementSystem.Modules.Services.Application.Commands.CreateSportService;

public class CreateSportServiceValidator : AbstractValidator<CreateSportServiceMessage>
{
    public CreateSportServiceValidator()
    {
        RuleFor(x => x.Request.BranchId)
            .GreaterThan(0);

        RuleFor(x => x.Request.Name)
            .NotEmpty();
        
        RuleFor(x => x.Request.Description)
            .NotEmpty();

        RuleFor(x => x.Request.Category)
            .NotEmpty()
            .IsInEnum();
    }
}
