using Moq;
using SportManagementSystem.Modules.Services.Application.Commands.CreateSportService;
using SportManagementSystem.Modules.Services.Contracts.Requests;
using SportManagementSystem.Modules.Services.Domain.Entities;
using SportManagementSystem.Modules.Services.Domain.Enums;
using SportManagementSystem.Modules.Services.Domain.Repositories;

namespace SportManagementSystem.Tests.Modules.Services.Unit;

public class CreateSportServiceHandlerTests
{
    private readonly Mock<ISportServiceRepository> _sportServiceRepository;
    private readonly CreateSportServiceHandler _handler;

    public CreateSportServiceHandlerTests()
    {
        _sportServiceRepository = new Mock<ISportServiceRepository>();
        _handler = new CreateSportServiceHandler(_sportServiceRepository.Object);
    }

    [Fact]
    public async Task Handle_WhenRequestValid_CreatesService()
    {
        DbSportService? created = null;
        _sportServiceRepository
            .Setup(x => x.CreateAsync(It.IsAny<DbSportService>(), It.IsAny<CancellationToken>()))
            .Callback<DbSportService, CancellationToken>((entity, _) => created = entity)
            .ReturnsAsync((DbSportService entity, CancellationToken _) =>
            {
                entity.Id = 44;
                return entity;
            });

        var result = await _handler.Handle(new CreateSportServiceMessage(new CreateSportServiceRequest
        {
            Name = "Yoga",
            Description = "Morning yoga",
            Category = EServiceCategory.Fitness
        }), CancellationToken.None);

        Assert.True(result.IsSuccess);
        Assert.Equal(44, result.Data);
        Assert.NotNull(created);
        Assert.Equal("Yoga", created!.Name);
        Assert.True(created.IsActive);
    }
}
