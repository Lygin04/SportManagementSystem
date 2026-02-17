using SportManagementSystem.Modules.Assets.Application.Commands.CreateBranch;
using SportManagementSystem.Modules.Assets.Application.Commands.CreateEquipment;
using SportManagementSystem.Modules.Assets.Application.Commands.CreateRoom;
using SportManagementSystem.Modules.Assets.Contracts.Request;
using SportManagementSystem.Modules.Assets.Domain.Enums;

namespace SportManagementSystem.Tests.Modules.Assets.Unit;

public class AssetsValidatorsTests
{
    [Fact]
    public void CreateBranchValidator_WhenRequestInvalid_ReturnsErrors()
    {
        var validator = new CreateBranchValidator();
        var message = new CreateBranchMessage(new CreateBranchRequest
        {
            Name = "",
            Address = ""
        });

        var result = validator.Validate(message);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "Request.Name");
        Assert.Contains(result.Errors, e => e.PropertyName == "Request.Address");
    }

    [Fact]
    public void CreateRoomValidator_WhenRequestInvalid_ReturnsErrors()
    {
        var validator = new CreateRoomValidator();
        var message = new CreateRoomMessage(new CreateRoomRequest
        {
            BranchId = 0,
            Name = "A",
            Capacity = 0
        });

        var result = validator.Validate(message);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "Request.BranchId");
        Assert.Contains(result.Errors, e => e.PropertyName == "Request.Name");
        Assert.Contains(result.Errors, e => e.PropertyName == "Request.Capacity");
    }

    [Fact]
    public void CreateEquipmentValidator_WhenRequestInvalid_ReturnsErrors()
    {
        var validator = new CreateEquipmentValidator();
        var message = new CreateEquipmentMessage(new CreateEquipmentRequest
        {
            RoomId = 0,
            Name = "",
            Quantity = 0,
            Condition = (EEquipmentCondition)99
        });

        var result = validator.Validate(message);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "Request.RoomId");
        Assert.Contains(result.Errors, e => e.PropertyName == "Request.Name");
        Assert.Contains(result.Errors, e => e.PropertyName == "Request.Quantity");
        Assert.Contains(result.Errors, e => e.PropertyName == "Request.Condition");
    }

    [Fact]
    public void CreateEquipmentValidator_WhenRequestValid_HasNoErrors()
    {
        var validator = new CreateEquipmentValidator();
        var message = new CreateEquipmentMessage(new CreateEquipmentRequest
        {
            RoomId = 1,
            Name = "Bike",
            Quantity = 2,
            Condition = EEquipmentCondition.New
        });

        var result = validator.Validate(message);

        Assert.True(result.IsValid);
    }
}
