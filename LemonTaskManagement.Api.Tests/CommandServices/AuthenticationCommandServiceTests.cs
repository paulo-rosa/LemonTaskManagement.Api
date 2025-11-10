using LemonTaskManagement.Domain.Commands.CommandServices;
using LemonTaskManagement.Domain.Commands.Commands;
using LemonTaskManagement.Domain.Commands.Interfaces.Services;
using LemonTaskManagement.Domain.Entities;
using LemonTaskManagement.Domain.Queries.Interfaces.Repositories;
using Moq;

namespace LemonTaskManagement.Api.Tests.CommandServices;

[TestFixture]
public class AuthenticationCommandServiceTests
{
    private Mock<IUsersQueryRepository> _mockUsersRepository;
    private Mock<IJwtTokenService> _mockJwtTokenService;
    private AuthenticationCommandService _service;

    [SetUp]
    public void Setup()
    {
        _mockUsersRepository = new Mock<IUsersQueryRepository>();
        _mockJwtTokenService = new Mock<IJwtTokenService>();
        _service = new AuthenticationCommandService(_mockUsersRepository.Object, _mockJwtTokenService.Object);
    }

    #region LoginAsync Tests

    [Test]
    public async Task LoginAsync_WithValidCredentials_ReturnsSuccessResponse()
    {
        // Arrange
        var username = "testuser";
        var password = "Password123!";
        var passwordHash = BCrypt.Net.BCrypt.HashPassword(password);
        var userId = Guid.NewGuid();
        var token = "jwt-token-string";
        var expiresAt = DateTimeOffset.UtcNow.AddHours(1);

        var user = new User
        {
            Id = userId,
            Username = username,
            Email = "test@example.com",
            PasswordHash = passwordHash
        };

        var command = new LoginCommand
        {
            Username = username,
            Password = password
        };

        _mockUsersRepository.Setup(r => r.GetUserByUsernameAsync(username))
            .ReturnsAsync(user);
        _mockJwtTokenService.Setup(s => s.GenerateToken(user))
            .Returns(token);
        _mockJwtTokenService.Setup(s => s.GetTokenExpiration())
            .Returns(expiresAt);

        // Act
        var result = await _service.LoginAsync(command);

        // Assert
        Assert.That(result.Success, Is.True);
        Assert.That(result.Data, Is.Not.Null);
        Assert.That(result.Data.UserId, Is.EqualTo(userId));
        Assert.That(result.Data.Username, Is.EqualTo(username));
        Assert.That(result.Data.Email, Is.EqualTo(user.Email));
        Assert.That(result.Data.Token, Is.EqualTo(token));
        Assert.That(result.Data.ExpiresAt, Is.EqualTo(expiresAt));
        Assert.That(result.Message, Is.EqualTo("Login successful"));
    }

    [Test]
    public async Task LoginAsync_WithEmptyUsername_ReturnsValidationError()
    {
        // Arrange
        var command = new LoginCommand
        {
            Username = "",
            Password = "Password123!"
        };

        // Act
        var result = await _service.LoginAsync(command);

        // Assert
        Assert.That(result.Success, Is.False);
        Assert.That(result.Errors, Is.Not.Empty);
        Assert.That(result.Errors.Any(e => e.Property == "Username" && e.Message == "Username is required"), Is.True);
        _mockUsersRepository.Verify(r => r.GetUserByUsernameAsync(It.IsAny<string>()), Times.Never);
    }

    [Test]
    public async Task LoginAsync_WithNullUsername_ReturnsValidationError()
    {
        // Arrange
        var command = new LoginCommand
        {
            Username = null,
            Password = "Password123!"
        };

        // Act
        var result = await _service.LoginAsync(command);

        // Assert
        Assert.That(result.Success, Is.False);
        Assert.That(result.Errors, Is.Not.Empty);
        Assert.That(result.Errors.Any(e => e.Property == "Username"), Is.True);
        _mockUsersRepository.Verify(r => r.GetUserByUsernameAsync(It.IsAny<string>()), Times.Never);
    }

    [Test]
    public async Task LoginAsync_WithEmptyPassword_ReturnsValidationError()
    {
        // Arrange
        var command = new LoginCommand
        {
            Username = "testuser",
            Password = ""
        };

        // Act
        var result = await _service.LoginAsync(command);

        // Assert
        Assert.That(result.Success, Is.False);
        Assert.That(result.Errors, Is.Not.Empty);
        Assert.That(result.Errors.Any(e => e.Property == "Password" && e.Message == "Password is required"), Is.True);
        _mockUsersRepository.Verify(r => r.GetUserByUsernameAsync(It.IsAny<string>()), Times.Never);
    }

    [Test]
    public async Task LoginAsync_WithBothFieldsEmpty_ReturnsMultipleValidationErrors()
    {
        // Arrange
        var command = new LoginCommand
        {
            Username = "",
            Password = ""
        };

        // Act
        var result = await _service.LoginAsync(command);

        // Assert
        Assert.That(result.Success, Is.False);
        Assert.That(result.Errors.Count(), Is.EqualTo(2));
        Assert.That(result.Errors.Any(e => e.Property == "Username"), Is.True);
        Assert.That(result.Errors.Any(e => e.Property == "Password"), Is.True);
        _mockUsersRepository.Verify(r => r.GetUserByUsernameAsync(It.IsAny<string>()), Times.Never);
    }

    [Test]
    public async Task LoginAsync_WithNonExistentUser_ReturnsAuthenticationError()
    {
        // Arrange
        var command = new LoginCommand
        {
            Username = "nonexistent",
            Password = "Password123!"
        };

        _mockUsersRepository.Setup(r => r.GetUserByUsernameAsync(command.Username))
            .ReturnsAsync((User?)null);

        // Act
        var result = await _service.LoginAsync(command);

        // Assert
        Assert.That(result.Success, Is.False);
        Assert.That(result.Message, Is.EqualTo("Invalid username or password"));
        Assert.That(result.Errors, Is.Not.Empty);
        Assert.That(result.Errors.Any(e => e.Property == "Authentication"), Is.True);
        _mockJwtTokenService.Verify(s => s.GenerateToken(It.IsAny<User>()), Times.Never);
    }

    [Test]
    public async Task LoginAsync_WithIncorrectPassword_ReturnsAuthenticationError()
    {
        // Arrange
        var username = "testuser";
        var correctPassword = "CorrectPassword123!";
        var incorrectPassword = "WrongPassword123!";
        var passwordHash = BCrypt.Net.BCrypt.HashPassword(correctPassword);

        var user = new User
        {
            Id = Guid.NewGuid(),
            Username = username,
            Email = "test@example.com",
            PasswordHash = passwordHash
        };

        var command = new LoginCommand
        {
            Username = username,
            Password = incorrectPassword
        };

        _mockUsersRepository.Setup(r => r.GetUserByUsernameAsync(username))
            .ReturnsAsync(user);

        // Act
        var result = await _service.LoginAsync(command);

        // Assert
        Assert.That(result.Success, Is.False);
        Assert.That(result.Message, Is.EqualTo("Invalid username or password"));
        Assert.That(result.Errors, Is.Not.Empty);
        Assert.That(result.Errors.Any(e => e.Property == "Authentication"), Is.True);
        _mockJwtTokenService.Verify(s => s.GenerateToken(It.IsAny<User>()), Times.Never);
    }

    [Test]
    public async Task LoginAsync_CallsJwtServiceOnlyOnSuccessfulAuthentication()
    {
        // Arrange
        var username = "testuser";
        var password = "Password123!";
        var passwordHash = BCrypt.Net.BCrypt.HashPassword(password);

        var user = new User
        {
            Id = Guid.NewGuid(),
            Username = username,
            Email = "test@example.com",
            PasswordHash = passwordHash
        };

        var command = new LoginCommand
        {
            Username = username,
            Password = password
        };

        _mockUsersRepository.Setup(r => r.GetUserByUsernameAsync(username))
            .ReturnsAsync(user);
        _mockJwtTokenService.Setup(s => s.GenerateToken(user))
            .Returns("token");
        _mockJwtTokenService.Setup(s => s.GetTokenExpiration())
            .Returns(DateTimeOffset.UtcNow.AddHours(1));

        // Act
        await _service.LoginAsync(command);

        // Assert
        _mockJwtTokenService.Verify(s => s.GenerateToken(user), Times.Once);
        _mockJwtTokenService.Verify(s => s.GetTokenExpiration(), Times.Once);
    }

    [Test]
    public async Task LoginAsync_WithWhitespaceUsername_ReturnsValidationError()
    {
        // Arrange
        var command = new LoginCommand
        {
            Username = "   ",
            Password = "Password123!"
        };

        // Act
        var result = await _service.LoginAsync(command);

        // Assert
        Assert.That(result.Success, Is.False);
        Assert.That(result.Errors.Any(e => e.Property == "Username"), Is.True);
        _mockUsersRepository.Verify(r => r.GetUserByUsernameAsync(It.IsAny<string>()), Times.Never);
    }

    [Test]
    public async Task LoginAsync_WithWhitespacePassword_ReturnsValidationError()
    {
        // Arrange
        var command = new LoginCommand
        {
            Username = "testuser",
            Password = "   "
        };

        // Act
        var result = await _service.LoginAsync(command);

        // Assert
        Assert.That(result.Success, Is.False);
        Assert.That(result.Errors.Any(e => e.Property == "Password"), Is.True);
        _mockUsersRepository.Verify(r => r.GetUserByUsernameAsync(It.IsAny<string>()), Times.Never);
    }

    #endregion
}
