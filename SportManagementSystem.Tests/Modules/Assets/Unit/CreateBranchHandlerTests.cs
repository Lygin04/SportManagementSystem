using Moq;
using SportManagementSystem.Modules.Assets.Application.Commands.CreateBranch;
using SportManagementSystem.Modules.Assets.Contracts.Request;
using SportManagementSystem.Modules.Assets.Domain.Entities;
using SportManagementSystem.Modules.Assets.Domain.Repositories;

namespace SportManagementSystem.Tests.Modules.Assets.Unit;

public class CreateBranchHandlerTests
{
    private readonly Mock<IBranchRepository> _branchRepository;
    private readonly CreateBranchHandler _handler;

    public CreateBranchHandlerTests()
    {
        _branchRepository = new Mock<IBranchRepository>();
        _handler = new CreateBranchHandler(_branchRepository.Object);
    }

    [Fact]
    public async Task Handle_WhenRequestValid_CreatesBranch()
    {
        DbBranch? created = null;
        _branchRepository
            .Setup(x => x.CreateAsync(It.IsAny<DbBranch>(), It.IsAny<CancellationToken>()))
            .Callback<DbBranch, CancellationToken>((entity, _) => created = entity)
            .ReturnsAsync((DbBranch entity, CancellationToken _) =>
            {
                entity.Id = 12;
                return entity;
            });

        var result = await _handler.Handle(new CreateBranchMessage(new CreateBranchRequest
        {
            Name = "Central",
            Address = "Main street"
        }), CancellationToken.None);

        Assert.True(result.IsSuccess);
        Assert.Equal(12, result.Data);
        Assert.NotNull(created);
        Assert.Equal("Central", created!.Name);
    }
}
