using Microsoft.AspNetCore.Http;
using Moq;
using SportManagementSystem.Modules.Services.Application.Commands.CreateServicePrice;
using SportManagementSystem.Modules.Services.Contracts.Requests;
using SportManagementSystem.Modules.Services.Domain.Entities;
using SportManagementSystem.Modules.Services.Domain.Enums;
using SportManagementSystem.Modules.Services.Domain.Repositories;

namespace SportManagementSystem.Tests.Modules.Services.Unit;

public class CreateServicePriceHandlerTests
{
    private readonly Mock<IServicePriceRepository> _servicePriceRepository;
    private readonly Mock<ISportServiceRepository> _sportServiceRepository;
    private readonly CreateServicePriceHandler _handler;

    public CreateServicePriceHandlerTests()
    {
        _servicePriceRepository = new Mock<IServicePriceRepository>();
        _sportServiceRepository = new Mock<ISportServiceRepository>();
        _handler = new CreateServicePriceHandler(_servicePriceRepository.Object, _sportServiceRepository.Object);
    }

    [Fact]
    public async Task Handle_WhenServiceMissing_ReturnsConflict()
    {
        _sportServiceRepository.Setup(x => x.ExistsAsync(It.IsAny<long>(), It.IsAny<CancellationToken>())).ReturnsAsync(false);

        var result = await _handler.Handle(new CreateServicePriceMessage(new CreateServicePriceRequest
        {
            SportServiceId = 9,
            Amount = 100,
            Currency = EIsoCurrency.USD,
            ValidFrom = new DateOnly(2026, 1, 1)
        }), CancellationToken.None);

        Assert.False(result.IsSuccess);
        Assert.NotNull(result.Error);
        Assert.Equal(StatusCodes.Status409Conflict, result.Error!.Status);
        _servicePriceRepository.Verify(x => x.CreateAsync(It.IsAny<DbServicePrice>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_WhenRequestValid_CreatesServicePrice()
    {
        DbServicePrice? created = null;
        _sportServiceRepository.Setup(x => x.ExistsAsync(It.IsAny<long>(), It.IsAny<CancellationToken>())).ReturnsAsync(true);
        _servicePriceRepository
            .Setup(x => x.CreateAsync(It.IsAny<DbServicePrice>(), It.IsAny<CancellationToken>()))
            .Callback<DbServicePrice, CancellationToken>((entity, _) => created = entity)
            .ReturnsAsync((DbServicePrice entity, CancellationToken _) =>
            {
                entity.Id = 101;
                return entity;
            });

        var request = new CreateServicePriceRequest
        {
            SportServiceId = 3,
            Amount = 150,
            Currency = EIsoCurrency.EUR,
            ValidFrom = new DateOnly(2026, 2, 1),
            ValidTo = new DateOnly(2026, 3, 1)
        };

        var result = await _handler.Handle(new CreateServicePriceMessage(request), CancellationToken.None);

        Assert.True(result.IsSuccess);
        Assert.Equal(101, result.Data);
        Assert.NotNull(created);
        Assert.Equal(150, created!.Amount);
        Assert.Equal(EIsoCurrency.EUR, created.Currency);
        Assert.Equal(request.ValidTo, created.ValidTo);
    }
}
