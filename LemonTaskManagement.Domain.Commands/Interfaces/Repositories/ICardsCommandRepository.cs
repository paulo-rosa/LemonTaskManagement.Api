using System;
using System.Threading.Tasks;
using LemonTaskManagement.Domain.Commands.Commands;
using LemonTaskManagement.Domain.Entities;

namespace LemonTaskManagement.Domain.Commands.Interfaces.Repositories;

public interface ICardsCommandRepository
{
    Task<Card> CreateCardAsync(CreateCardCommand command);
    Task<bool> UserHasAccessToBoardAsync(Guid userId, Guid boardId);
    Task<bool> BoardColumnExistsAsync(Guid boardColumnId, Guid boardId);
    Task<int> GetNextCardOrderAsync(Guid boardColumnId);
    Task<Card> GetCardByIdAsync(Guid cardId);
    Task<Card> MoveCardAsync(Guid cardId, Guid targetBoardColumnId, int targetOrder);
    Task ReorderCardsAsync(Guid boardColumnId, int fromOrder);
    Task<Card> UpdateCardAsync(UpdateCardCommand command);
    Task<Guid?> GetCardBoardIdAsync(Guid cardId);
}
