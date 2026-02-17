using Microsoft.AspNetCore.Http;
using Moq;
using SportManagementSystem.BuildingBlocks.Authentication.Hash.Interfaces;
using SportManagementSystem.Modules.Users.Application.Commands.RegisterClient;
using SportManagementSystem.Modules.Users.Contracts.Requests;
using SportManagementSystem.Modules.Users.Domain.Entities;
using SportManagementSystem.Modules.Users.Domain.Enums;
using SportManagementSystem.Modules.Users.Domain.Repositories;

namespace SportManagementSystem.Tests.Modules.Users.Unit;

public class RegisterClientHandlerTests
{
    private readonly Mock<IUserAccountRepository> _userAccountRepository;
    private readonly Mock<IClientRepository> _clientRepository;
    private readonly Mock<IPasswordHasher> _passwordHasher;
    private readonly RegisterClientHandler _handler;

    public RegisterClientHandlerTests()
    {
        _userAccountRepository = new Mock<IUserAccountRepository>();
        _clientRepository = new Mock<IClientRepository>();
        _passwordHasher = new Mock<IPasswordHasher>();
        _handler = new RegisterClientHandler(
            _userAccountRepository.Object,
            _clientRepository.Object,
            _passwordHasher.Object);
    }

    [Fact]
    public async Task Handle_WhenEmailAlreadyExists_ReturnsConflict()
    {
        _userAccountRepository
            .Setup(x => x.ExistsEmailAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var result = await _handler.Handle(new RegisterClientMessage(CreateRequest()), CancellationToken.None);

        Assert.False(result.IsSuccess);
        Assert.NotNull(result.Error);
        Assert.Equal(StatusCodes.Status409Conflict, result.Error!.Status);
        _clientRepository.Verify(x => x.CreateAsync(It.IsAny<DbClient>(), It.IsAny<CancellationToken>()), Times.Never);
        _userAccountRepository.Verify(x => x.CreateAsync(It.IsAny<DbUserAccount>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_WhenRequestIsValid_CreatesClientAndUserAccount()
    {
        var request = CreateRequest();
        DbClient? createdClient = null;
        DbUserAccount? createdUser = null;

        _userAccountRepository
            .Setup(x => x.ExistsEmailAsync(request.Email, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);
        _clientRepository
            .Setup(x => x.CreateAsync(It.IsAny<DbClient>(), It.IsAny<CancellationToken>()))
            .Callback<DbClient, CancellationToken>((entity, _) => createdClient = entity)
            .ReturnsAsync((DbClient entity, CancellationToken _) =>
            {
                entity.Id = 11;
                return entity;
            });
        _passwordHasher
            .Setup(x => x.Hash(request.Password))
            .Returns("hashed-pass");
        _userAccountRepository
            .Setup(x => x.CreateAsync(It.IsAny<DbUserAccount>(), It.IsAny<CancellationToken>()))
            .Callback<DbUserAccount, CancellationToken>((entity, _) => createdUser = entity)
            .ReturnsAsync((DbUserAccount entity, CancellationToken _) => entity);

        var result = await _handler.Handle(new RegisterClientMessage(request), CancellationToken.None);

        Assert.True(result.IsSuccess);
        _clientRepository.Verify(x => x.CreateAsync(It.IsAny<DbClient>(), It.IsAny<CancellationToken>()), Times.Once);
        _userAccountRepository.Verify(x => x.CreateAsync(It.IsAny<DbUserAccount>(), It.IsAny<CancellationToken>()), Times.Once);
        Assert.NotNull(createdClient);
        Assert.Equal(request.FirstName, createdClient!.FirstName);
        Assert.Equal(request.LastName, createdClient.LastName);
        Assert.NotNull(createdUser);
        Assert.Equal(request.Email, createdUser!.Email);
        Assert.Equal(EUserRole.Client, createdUser.Role);
        Assert.Equal(EAccountStatus.Active, createdUser.Status);
        Assert.Equal(11, createdUser.ClientId);
        Assert.Equal("hashed-pass", createdUser.PasswordHash);
    }

    private static RegisterClientRequest CreateRequest() =>
        new()
        {
            Email = "client@example.com",
            Password = "Password123",
            FirstName = "Jane",
            LastName = "Doe",
            Patronymic = "A.",
            BirthDate = new DateOnly(1993, 3, 1),
            Phone = "+12345678901"
        };
}
