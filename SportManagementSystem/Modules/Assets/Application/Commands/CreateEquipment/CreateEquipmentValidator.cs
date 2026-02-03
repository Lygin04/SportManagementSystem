using FluentValidation;

namespace SportManagementSystem.Modules.Assets.Application.Commands.CreateEquipment;

public class CreateEquipmentValidator : AbstractValidator<CreateEquipmentMessage>
{
    public CreateEquipmentValidator()
    {
        RuleFor(x => x.Request.RoomId)
            .NotEmpty();
        
        RuleFor(x => x.Request.Name)
            .NotEmpty();
        
        RuleFor(x => x.Request.Quantity)
            .NotEmpty()
            .GreaterThan(0);

        RuleFor(x => x.Request.Condition)
            .IsInEnum();
    }
}