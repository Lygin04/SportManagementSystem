using FluentValidation;

namespace SportManagementSystem.Modules.Assets.Application.Commands.CreateBranch;

public class CreateBranchValidator : AbstractValidator<CreateBranchMessage>
{
    public CreateBranchValidator()
    {
        RuleFor(x => x.Request.Name)
            .Length(2, 100)
            .NotEmpty();
        
        RuleFor(x => x.Request.Address)
            .Length(2, 150)
            .NotEmpty();
    }
}