using LemonTaskManagement.Domain.Commands.Commands;
using LemonTaskManagement.Domain.Commands.Interfaces.CommandServices;
using LemonTaskManagement.Domain.Commands.Interfaces.Repositories;
using LemonTaskManagement.Domain.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LemonTaskManagement.Domain.Commands.CommandServices;

public class CardsCommandService(ICardsCommandRepository cardsCommandRepository) : ICardsCommandService
{
    public async Task<CreateCardResponse> CreateCardAsync(CreateCardCommand command)
    {
        var errors = new List<Error>();

        if (!await cardsCommandRepository.UserHasAccessToBoardAsync(command.UserId, command.BoardId))
        {
            errors.Add(new Error("UserId", "User does not have access to this board"));
        }

        if (!await cardsCommandRepository.BoardColumnExistsAsync(command.BoardColumnId, command.BoardId))
        {
            errors.Add(new Error("BoardColumnId", "Board column does not exist or does not belong to the specified board"));
        }

        if (string.IsNullOrWhiteSpace(command.Description))
        {
            errors.Add(new Error("Description", "Description is required"));
        }

        if (errors.Count > 0)
        {
            return new CreateCardResponse
            {
                Success = false,
                Errors = errors
            };
        }

        var card = await cardsCommandRepository.CreateCardAsync(command);

        return new CreateCardResponse
        {
            Success = true,
            Data = new CreateCardDto
            {
                Id = card.Id,
                BoardColumnId = card.BoardColumnId,
                Description = card.Description,
                Order = card.Order,
                AssignedUserId = card.AssignedUserId
            }
        };
    }

    public async Task<MoveCardResponse> MoveCardAsync(MoveCardCommand command)
    {
        var errors = new List<Error>();

        if (!await cardsCommandRepository.UserHasAccessToBoardAsync(command.UserId, command.BoardId))
        {
            errors.Add(new Error("UserId", "User does not have access to this board"));
        }

        if (!await cardsCommandRepository.BoardColumnExistsAsync(command.TargetBoardColumnId, command.BoardId))
        {
            errors.Add(new Error("TargetBoardColumnId", "Target board column does not exist or does not belong to the specified board"));
        }

        var card = await cardsCommandRepository.GetCardByIdAsync(command.CardId);
        if (card == null)
        {
            errors.Add(new Error("CardId", "Card does not exist"));
        }

        if (command.TargetOrder < 1)
        {
            errors.Add(new Error("TargetOrder", "Target order must be greater than 0"));
        }

        if (errors.Count > 0)
        {
            return new MoveCardResponse
            {
                Success = false,
                Errors = errors
            };
        }

        await cardsCommandRepository.ReorderCardsAsync(command.TargetBoardColumnId, command.TargetOrder);

        var movedCard = await cardsCommandRepository.MoveCardAsync(command.CardId, command.TargetBoardColumnId, command.TargetOrder);

        return new MoveCardResponse
        {
            Success = true,
            Data = new MoveCardDto
            {
                Id = movedCard.Id,
                BoardColumnId = movedCard.BoardColumnId,
                Description = movedCard.Description,
                Order = movedCard.Order,
                AssignedUserId = movedCard.AssignedUserId
            }
        };
    }
}
