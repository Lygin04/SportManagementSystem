using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Moq;
using SportManagementSystem.BuildingBlocks.Authentication.Hash.Interfaces;
using SportManagementSystem.Modules.Users.Application.Commands.LoginUser;
using SportManagementSystem.Modules.Users.Application.Commands.RegisterStaff;
using SportManagementSystem.Modules.Users.Contracts.Requests;
using SportManagementSystem.Modules.Users.Contracts.Response;
using SportManagementSystem.Modules.Users.Domain.Entities;
using SportManagementSystem.Modules.Users.Domain.Enums;
using SportManagementSystem.Modules.Users.Domain.Repositories;
using SportManagementSystem.Modules.Users.Domain.Services;

namespace SportManagementSystem.Tests.Modules.Users.Unit;

public class UsersHandlersTests
{
    private readonly Mock<IUserAccountRepository> _userRepoMock;
    private readonly Mock<IStaffRepository> _staffRepoMock;
    private readonly Mock<IPasswordHasher> _hasherMock;
    private readonly Mock<IJwtTokenService> _jwtMock;
    private readonly RegisterStaffHandler _registerStaffHandler;
    private readonly LoginUserHandler _loginUserHandler;

    public UsersHandlersTests()
    {
        _userRepoMock = new Mock<IUserAccountRepository>();
        _staffRepoMock = new Mock<IStaffRepository>();
        _hasherMock = new Mock<IPasswordHasher>();
        _jwtMock = new Mock<IJwtTokenService>();

        _registerStaffHandler = new RegisterStaffHandler(_userRepoMock.Object, _staffRepoMock.Object, _hasherMock.Object);
        _loginUserHandler = new LoginUserHandler(_jwtMock.Object, _hasherMock.Object, _userRepoMock.Object);
    }

    [Fact]
    public async Task RegisterStaff_WhenEmailAlreadyExists_ReturnsConflict()
    {
        _userRepoMock
            .Setup(x => x.ExistsEmailAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var result = await _registerStaffHandler.Handle(new RegisterStaffMessage(CreateRegisterStaffRequest()), CancellationToken.None);

        Assert.False(result.IsSuccess);
        Assert.NotNull(result.Error);
        Assert.Equal(StatusCodes.Status409Conflict, result.Error!.Status);
        _staffRepoMock.Verify(x => x.CreateAsync(It.IsAny<DbStaff>(), It.IsAny<CancellationToken>()), Times.Never);
        _userRepoMock.Verify(x => x.CreateAsync(It.IsAny<DbUserAccount>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task RegisterStaff_WhenEmailIsUnique_CreatesStaffAndUser()
    {
        var request = CreateRegisterStaffRequest();
        DbUserAccount? createdUser = null;
        DbStaff? createdStaff = null;

        _userRepoMock
            .Setup(x => x.ExistsEmailAsync(request.Email, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);
        _staffRepoMock
            .Setup(x => x.CreateAsync(It.IsAny<DbStaff>(), It.IsAny<CancellationToken>()))
            .Callback<DbStaff, CancellationToken>((entity, _) => createdStaff = entity)
            .ReturnsAsync((DbStaff entity, CancellationToken _) =>
            {
                entity.Id = 42;
                return entity;
            });
        _hasherMock
            .Setup(x => x.Hash(request.Password))
            .Returns("hashed-password");
        _userRepoMock
            .Setup(x => x.CreateAsync(It.IsAny<DbUserAccount>(), It.IsAny<CancellationToken>()))
            .Callback<DbUserAccount, CancellationToken>((entity, _) => createdUser = entity)
            .ReturnsAsync((DbUserAccount entity, CancellationToken _) => entity);

        var result = await _registerStaffHandler.Handle(new RegisterStaffMessage(request), CancellationToken.None);

        Assert.True(result.IsSuccess);
        _staffRepoMock.Verify(x => x.CreateAsync(It.IsAny<DbStaff>(), It.IsAny<CancellationToken>()), Times.Once);
        _userRepoMock.Verify(x => x.CreateAsync(It.IsAny<DbUserAccount>(), It.IsAny<CancellationToken>()), Times.Once);
        Assert.NotNull(createdStaff);
        Assert.Equal(request.FirstName, createdStaff!.FirstName);
        Assert.Equal(request.LastName, createdStaff.LastName);
        Assert.NotNull(createdUser);
        Assert.Equal(request.Email, createdUser!.Email);
        Assert.Equal("hashed-password", createdUser.PasswordHash);
        Assert.Equal(request.Role, createdUser.Role);
        Assert.Equal(EAccountStatus.Active, createdUser.Status);
        Assert.Equal(42, createdUser.ClientId);
    }

    [Fact]
    public async Task LoginUser_WhenCandidateNotFound_ReturnsUnauthorized()
    {
        _userRepoMock
            .Setup(x => x.GetByEmailAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((DbUserAccount?)null);

        var result = await _loginUserHandler.Handle(new LoginUserMessage(CreateLoginUserRequest()), CancellationToken.None);

        Assert.False(result.IsSuccess);
        Assert.NotNull(result.Error);
        Assert.Equal(StatusCodes.Status401Unauthorized, result.Error!.Status);
        _userRepoMock.Verify(x => x.UpdateLastLoginDateAsync(It.IsAny<long>(), It.IsAny<CancellationToken>()), Times.Never);
        _jwtMock.Verify(x => x.CreateAccessTokenAsync(It.IsAny<ICollection<Claim>>()), Times.Never);
    }

    [Fact]
    public async Task LoginUser_WhenPasswordInvalid_ReturnsUnauthorized()
    {
        var candidate = new DbUserAccount
        {
            Id = 100,
            Email = "test@test.com",
            PasswordHash = "stored-hash",
            Role = EUserRole.Manager,
            Status = EAccountStatus.Active
        };
        _userRepoMock
            .Setup(x => x.GetByEmailAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(candidate);
        _hasherMock
            .Setup(x => x.Verify(It.IsAny<string>(), It.IsAny<string>()))
            .Returns(false);

        var result = await _loginUserHandler.Handle(new LoginUserMessage(CreateLoginUserRequest()), CancellationToken.None);

        Assert.False(result.IsSuccess);
        Assert.NotNull(result.Error);
        Assert.Equal(StatusCodes.Status401Unauthorized, result.Error!.Status);
        _userRepoMock.Verify(x => x.UpdateLastLoginDateAsync(It.IsAny<long>(), It.IsAny<CancellationToken>()), Times.Never);
        _jwtMock.Verify(x => x.CreateAccessTokenAsync(It.IsAny<ICollection<Claim>>()), Times.Never);
    }

    [Fact]
    public async Task LoginUser_WhenAccountInactive_ReturnsConflict()
    {
        var candidate = new DbUserAccount
        {
            Id = 100,
            Email = "test@test.com",
            PasswordHash = "stored-hash",
            Role = EUserRole.Manager,
            Status = EAccountStatus.Suspended
        };
        _userRepoMock
            .Setup(x => x.GetByEmailAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(candidate);
        _hasherMock
            .Setup(x => x.Verify(It.IsAny<string>(), It.IsAny<string>()))
            .Returns(true);

        var result = await _loginUserHandler.Handle(new LoginUserMessage(CreateLoginUserRequest()), CancellationToken.None);

        Assert.False(result.IsSuccess);
        Assert.NotNull(result.Error);
        Assert.Equal(StatusCodes.Status409Conflict, result.Error!.Status);
        _userRepoMock.Verify(x => x.UpdateLastLoginDateAsync(It.IsAny<long>(), It.IsAny<CancellationToken>()), Times.Never);
        _jwtMock.Verify(x => x.CreateAccessTokenAsync(It.IsAny<ICollection<Claim>>()), Times.Never);
    }

    [Fact]
    public async Task LoginUser_WhenCredentialsAreValid_ReturnsTokenAndUpdatesLoginDate()
    {
        var candidate = new DbUserAccount
        {
            Id = 555,
            Email = "test@test.com",
            PasswordHash = "stored-hash",
            Role = EUserRole.Trainer,
            Status = EAccountStatus.Active
        };
        var tokenResponse = new LoginUserResponse
        {
            Token = "jwt-token",
            Expires = DateTime.UtcNow.AddHours(1)
        };
        ICollection<Claim>? sentClaims = null;
        _userRepoMock
            .Setup(x => x.GetByEmailAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(candidate);
        _hasherMock
            .Setup(x => x.Verify(It.IsAny<string>(), It.IsAny<string>()))
            .Returns(true);
        _jwtMock
            .Setup(x => x.CreateAccessTokenAsync(It.IsAny<ICollection<Claim>>()))
            .Callback<ICollection<Claim>>(claims => sentClaims = claims)
            .ReturnsAsync(tokenResponse);

        var result = await _loginUserHandler.Handle(new LoginUserMessage(CreateLoginUserRequest()), CancellationToken.None);

        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Data);
        Assert.Equal("jwt-token", result.Data!.Token);
        _userRepoMock.Verify(x => x.UpdateLastLoginDateAsync(555, It.IsAny<CancellationToken>()), Times.Once);
        _jwtMock.Verify(x => x.CreateAccessTokenAsync(It.IsAny<ICollection<Claim>>()), Times.Once);
        Assert.NotNull(sentClaims);
        Assert.Equal("test@test.com", sentClaims!.Single(x => x.Type == "email").Value);
        Assert.Equal(EUserRole.Trainer.ToString(), sentClaims.Single(x => x.Type == "role").Value);
        Assert.Equal("555", sentClaims.Single(x => x.Type == "id").Value);
    }

    private static RegisterStaffRequest CreateRegisterStaffRequest() =>
        new()
        {
            Email = "test@test.com",
            Password = "Password123",
            FirstName = "John",
            LastName = "Doe",
            Patronymic = "Smith",
            BirthDate = new DateOnly(1990, 1, 1),
            Phone = "+12345678901",
            Role = EUserRole.Manager
        };

    private static LoginUserRequest CreateLoginUserRequest() =>
        new()
        {
            Email = "test@test.com",
            Password = "Password123"
        };
}
