using LemonTaskManagement.Domain.Commands.CommandServices;
using LemonTaskManagement.Domain.Commands.Commands;
using LemonTaskManagement.Domain.Commands.Interfaces.Repositories;
using LemonTaskManagement.Domain.Entities;
using Moq;

namespace LemonTaskManagement.Api.Tests.CommandServices;

[TestFixture]
public class CardsCommandServiceTests
{
    private Mock<ICardsCommandRepository> _mockRepository;
    private CardsCommandService _service;

    [SetUp]
    public void Setup()
    {
        _mockRepository = new Mock<ICardsCommandRepository>();
        _service = new CardsCommandService(_mockRepository.Object);
    }

    #region CreateCardAsync Tests

    [Test]
    public async Task CreateCardAsync_WithValidCommand_ReturnsSuccessResponse()
    {
        // Arrange
        var command = new CreateCardCommand
        {
            UserId = Guid.NewGuid(),
            BoardId = Guid.NewGuid(),
            BoardColumnId = Guid.NewGuid(),
            Description = "Test Card",
            AssignedUserId = Guid.NewGuid()
        };

        var expectedCard = new Card
        {
            Id = Guid.NewGuid(),
            BoardColumnId = command.BoardColumnId,
            Description = command.Description,
            Order = 1,
            AssignedUserId = command.AssignedUserId
        };

        _mockRepository.Setup(r => r.UserHasAccessToBoardAsync(command.UserId, command.BoardId))
            .ReturnsAsync(true);
        _mockRepository.Setup(r => r.BoardColumnExistsAsync(command.BoardColumnId, command.BoardId))
            .ReturnsAsync(true);
        _mockRepository.Setup(r => r.CreateCardAsync(command))
            .ReturnsAsync(expectedCard);

        // Act
        var result = await _service.CreateCardAsync(command);

        // Assert
        Assert.That(result.Success, Is.True);
        Assert.That(result.Data, Is.Not.Null);
        Assert.That(result.Data.Id, Is.EqualTo(expectedCard.Id));
        Assert.That(result.Data.Description, Is.EqualTo(expectedCard.Description));
        Assert.That(result.Data.BoardColumnId, Is.EqualTo(expectedCard.BoardColumnId));
        Assert.That(result.Data.Order, Is.EqualTo(expectedCard.Order));
        Assert.That(result.Data.AssignedUserId, Is.EqualTo(expectedCard.AssignedUserId));
    }

    [Test]
    public async Task CreateCardAsync_WithoutUserAccess_ReturnsErrorResponse()
    {
        // Arrange
        var command = new CreateCardCommand
        {
            UserId = Guid.NewGuid(),
            BoardId = Guid.NewGuid(),
            BoardColumnId = Guid.NewGuid(),
            Description = "Test Card"
        };

        _mockRepository.Setup(r => r.UserHasAccessToBoardAsync(command.UserId, command.BoardId))
            .ReturnsAsync(false);
        _mockRepository.Setup(r => r.BoardColumnExistsAsync(command.BoardColumnId, command.BoardId))
            .ReturnsAsync(true);

        // Act
        var result = await _service.CreateCardAsync(command);

        // Assert
        Assert.That(result.Success, Is.False);
        Assert.That(result.Errors, Is.Not.Empty);
        Assert.That(result.Errors.Any(e => e.Property == "UserId"), Is.True);
        _mockRepository.Verify(r => r.CreateCardAsync(It.IsAny<CreateCardCommand>()), Times.Never);
    }

    [Test]
    public async Task CreateCardAsync_WithInvalidBoardColumn_ReturnsErrorResponse()
    {
        // Arrange
        var command = new CreateCardCommand
        {
            UserId = Guid.NewGuid(),
            BoardId = Guid.NewGuid(),
            BoardColumnId = Guid.NewGuid(),
            Description = "Test Card"
        };

        _mockRepository.Setup(r => r.UserHasAccessToBoardAsync(command.UserId, command.BoardId))
            .ReturnsAsync(true);
        _mockRepository.Setup(r => r.BoardColumnExistsAsync(command.BoardColumnId, command.BoardId))
            .ReturnsAsync(false);

        // Act
        var result = await _service.CreateCardAsync(command);

        // Assert
        Assert.That(result.Success, Is.False);
        Assert.That(result.Errors, Is.Not.Empty);
        Assert.That(result.Errors.Any(e => e.Property == "BoardColumnId"), Is.True);
        _mockRepository.Verify(r => r.CreateCardAsync(It.IsAny<CreateCardCommand>()), Times.Never);
    }

    [Test]
    public async Task CreateCardAsync_WithEmptyDescription_ReturnsErrorResponse()
    {
        // Arrange
        var command = new CreateCardCommand
        {
            UserId = Guid.NewGuid(),
            BoardId = Guid.NewGuid(),
            BoardColumnId = Guid.NewGuid(),
            Description = ""
        };

        _mockRepository.Setup(r => r.UserHasAccessToBoardAsync(command.UserId, command.BoardId))
            .ReturnsAsync(true);
        _mockRepository.Setup(r => r.BoardColumnExistsAsync(command.BoardColumnId, command.BoardId))
            .ReturnsAsync(true);

        // Act
        var result = await _service.CreateCardAsync(command);

        // Assert
        Assert.That(result.Success, Is.False);
        Assert.That(result.Errors, Is.Not.Empty);
        Assert.That(result.Errors.Any(e => e.Property == "Description"), Is.True);
        _mockRepository.Verify(r => r.CreateCardAsync(It.IsAny<CreateCardCommand>()), Times.Never);
    }

    [Test]
    public async Task CreateCardAsync_WithMultipleErrors_ReturnsAllErrors()
    {
        // Arrange
        var command = new CreateCardCommand
        {
            UserId = Guid.NewGuid(),
            BoardId = Guid.NewGuid(),
            BoardColumnId = Guid.NewGuid(),
            Description = ""
        };

        _mockRepository.Setup(r => r.UserHasAccessToBoardAsync(command.UserId, command.BoardId))
            .ReturnsAsync(false);
        _mockRepository.Setup(r => r.BoardColumnExistsAsync(command.BoardColumnId, command.BoardId))
            .ReturnsAsync(false);

        // Act
        var result = await _service.CreateCardAsync(command);

        // Assert
        Assert.That(result.Success, Is.False);
        Assert.That(result.Errors.Count(), Is.EqualTo(3));
        _mockRepository.Verify(r => r.CreateCardAsync(It.IsAny<CreateCardCommand>()), Times.Never);
    }

    #endregion

    #region MoveCardAsync Tests

    [Test]
    public async Task MoveCardAsync_WithValidCommand_ReturnsSuccessResponse()
    {
        // Arrange
        var cardId = Guid.NewGuid();
        var command = new MoveCardCommand
        {
            UserId = Guid.NewGuid(),
            CardId = cardId,
            BoardId = Guid.NewGuid(),
            TargetBoardColumnId = Guid.NewGuid(),
            TargetOrder = 2
        };

        var existingCard = new Card
        {
            Id = cardId,
            BoardColumnId = Guid.NewGuid(),
            Description = "Test Card",
            Order = 1
        };

        var movedCard = new Card
        {
            Id = cardId,
            BoardColumnId = command.TargetBoardColumnId,
            Description = "Test Card",
            Order = command.TargetOrder
        };

        _mockRepository.Setup(r => r.UserHasAccessToBoardAsync(command.UserId, command.BoardId))
            .ReturnsAsync(true);
        _mockRepository.Setup(r => r.BoardColumnExistsAsync(command.TargetBoardColumnId, command.BoardId))
            .ReturnsAsync(true);
        _mockRepository.Setup(r => r.GetCardByIdAsync(command.CardId))
            .ReturnsAsync(existingCard);
        _mockRepository.Setup(r => r.ReorderCardsAsync(command.TargetBoardColumnId, command.TargetOrder))
            .Returns(Task.CompletedTask);
        _mockRepository.Setup(r => r.MoveCardAsync(command.CardId, command.TargetBoardColumnId, command.TargetOrder))
            .ReturnsAsync(movedCard);

        // Act
        var result = await _service.MoveCardAsync(command);

        // Assert
        Assert.That(result.Success, Is.True);
        Assert.That(result.Data, Is.Not.Null);
        Assert.That(result.Data.Id, Is.EqualTo(cardId));
        Assert.That(result.Data.BoardColumnId, Is.EqualTo(command.TargetBoardColumnId));
        Assert.That(result.Data.Order, Is.EqualTo(command.TargetOrder));
        _mockRepository.Verify(r => r.ReorderCardsAsync(command.TargetBoardColumnId, command.TargetOrder), Times.Once);
    }

    [Test]
    public async Task MoveCardAsync_WithNonExistentCard_ReturnsErrorResponse()
    {
        // Arrange
        var command = new MoveCardCommand
        {
            UserId = Guid.NewGuid(),
            CardId = Guid.NewGuid(),
            BoardId = Guid.NewGuid(),
            TargetBoardColumnId = Guid.NewGuid(),
            TargetOrder = 1
        };

        _mockRepository.Setup(r => r.UserHasAccessToBoardAsync(command.UserId, command.BoardId))
            .ReturnsAsync(true);
        _mockRepository.Setup(r => r.BoardColumnExistsAsync(command.TargetBoardColumnId, command.BoardId))
            .ReturnsAsync(true);
        _mockRepository.Setup(r => r.GetCardByIdAsync(command.CardId))
            .ReturnsAsync((Card?)null);

        // Act
        var result = await _service.MoveCardAsync(command);

        // Assert
        Assert.That(result.Success, Is.False);
        Assert.That(result.Errors.Any(e => e.Property == "CardId"), Is.True);
        _mockRepository.Verify(r => r.MoveCardAsync(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<int>()), Times.Never);
    }

    [Test]
    public async Task MoveCardAsync_WithInvalidTargetOrder_ReturnsErrorResponse()
    {
        // Arrange
        var command = new MoveCardCommand
        {
            UserId = Guid.NewGuid(),
            CardId = Guid.NewGuid(),
            BoardId = Guid.NewGuid(),
            TargetBoardColumnId = Guid.NewGuid(),
            TargetOrder = 0
        };

        var existingCard = new Card { Id = command.CardId };

        _mockRepository.Setup(r => r.UserHasAccessToBoardAsync(command.UserId, command.BoardId))
            .ReturnsAsync(true);
        _mockRepository.Setup(r => r.BoardColumnExistsAsync(command.TargetBoardColumnId, command.BoardId))
            .ReturnsAsync(true);
        _mockRepository.Setup(r => r.GetCardByIdAsync(command.CardId))
            .ReturnsAsync(existingCard);

        // Act
        var result = await _service.MoveCardAsync(command);

        // Assert
        Assert.That(result.Success, Is.False);
        Assert.That(result.Errors.Any(e => e.Property == "TargetOrder"), Is.True);
    }

    #endregion

    #region UpdateCardAsync Tests

    [Test]
    public async Task UpdateCardAsync_WithValidCommand_ReturnsSuccessResponse()
    {
        // Arrange
        var cardId = Guid.NewGuid();
        var boardId = Guid.NewGuid();
        var command = new UpdateCardCommand
        {
            UserId = Guid.NewGuid(),
            CardId = cardId,
            BoardId = boardId,
            Description = "Updated Description",
            AssignedUserId = Guid.NewGuid()
        };

        var updatedCard = new Card
        {
            Id = cardId,
            BoardColumnId = Guid.NewGuid(),
            Description = command.Description,
            Order = 1,
            AssignedUserId = command.AssignedUserId
        };

        _mockRepository.Setup(r => r.GetCardBoardIdAsync(command.CardId))
            .ReturnsAsync(boardId);
        _mockRepository.Setup(r => r.UserHasAccessToBoardAsync(command.UserId, boardId))
            .ReturnsAsync(true);
        _mockRepository.Setup(r => r.UpdateCardAsync(command))
            .ReturnsAsync(updatedCard);

        // Act
        var result = await _service.UpdateCardAsync(command);

        // Assert
        Assert.That(result.Success, Is.True);
        Assert.That(result.Data, Is.Not.Null);
        Assert.That(result.Data.Id, Is.EqualTo(cardId));
        Assert.That(result.Data.Description, Is.EqualTo(command.Description));
        Assert.That(result.Data.AssignedUserId, Is.EqualTo(command.AssignedUserId));
    }

    [Test]
    public async Task UpdateCardAsync_WithEmptyDescription_ReturnsErrorResponse()
    {
        // Arrange
        var command = new UpdateCardCommand
        {
            UserId = Guid.NewGuid(),
            CardId = Guid.NewGuid(),
            BoardId = Guid.NewGuid(),
            Description = ""
        };

        // Act
        var result = await _service.UpdateCardAsync(command);

        // Assert
        Assert.That(result.Success, Is.False);
        Assert.That(result.Errors.Any(e => e.Property == "Description"), Is.True);
        _mockRepository.Verify(r => r.UpdateCardAsync(It.IsAny<UpdateCardCommand>()), Times.Never);
    }

    [Test]
    public async Task UpdateCardAsync_WithNonExistentCard_ReturnsErrorResponse()
    {
        // Arrange
        var command = new UpdateCardCommand
        {
            UserId = Guid.NewGuid(),
            CardId = Guid.NewGuid(),
            BoardId = Guid.NewGuid(),
            Description = "Valid Description"
        };

        _mockRepository.Setup(r => r.GetCardBoardIdAsync(command.CardId))
            .ReturnsAsync((Guid?)null);

        // Act
        var result = await _service.UpdateCardAsync(command);

        // Assert
        Assert.That(result.Success, Is.False);
        Assert.That(result.Errors.Any(e => e.Property == "CardId"), Is.True);
        _mockRepository.Verify(r => r.UpdateCardAsync(It.IsAny<UpdateCardCommand>()), Times.Never);
    }

    [Test]
    public async Task UpdateCardAsync_WithWrongBoardId_ReturnsErrorResponse()
    {
        // Arrange
        var command = new UpdateCardCommand
        {
            UserId = Guid.NewGuid(),
            CardId = Guid.NewGuid(),
            BoardId = Guid.NewGuid(),
            Description = "Valid Description"
        };

        _mockRepository.Setup(r => r.GetCardBoardIdAsync(command.CardId))
            .ReturnsAsync(Guid.NewGuid()); // Different board ID

        // Act
        var result = await _service.UpdateCardAsync(command);

        // Assert
        Assert.That(result.Success, Is.False);
        Assert.That(result.Errors.Any(e => e.Property == "BoardId"), Is.True);
        _mockRepository.Verify(r => r.UpdateCardAsync(It.IsAny<UpdateCardCommand>()), Times.Never);
    }

    [Test]
    public async Task UpdateCardAsync_WithoutUserAccess_ReturnsErrorResponse()
    {
        // Arrange
        var boardId = Guid.NewGuid();
        var command = new UpdateCardCommand
        {
            UserId = Guid.NewGuid(),
            CardId = Guid.NewGuid(),
            BoardId = boardId,
            Description = "Valid Description"
        };

        _mockRepository.Setup(r => r.GetCardBoardIdAsync(command.CardId))
            .ReturnsAsync(boardId);
        _mockRepository.Setup(r => r.UserHasAccessToBoardAsync(command.UserId, boardId))
            .ReturnsAsync(false);

        // Act
        var result = await _service.UpdateCardAsync(command);

        // Assert
        Assert.That(result.Success, Is.False);
        Assert.That(result.Errors.Any(e => e.Property == "UserId"), Is.True);
        _mockRepository.Verify(r => r.UpdateCardAsync(It.IsAny<UpdateCardCommand>()), Times.Never);
    }

    #endregion
}
