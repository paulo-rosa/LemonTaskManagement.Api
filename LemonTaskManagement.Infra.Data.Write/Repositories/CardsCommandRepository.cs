using LemonTaskManagement.Domain.Commands.Commands;
using LemonTaskManagement.Domain.Commands.Interfaces.Repositories;
using LemonTaskManagement.Domain.Entities;
using LemonTaskManagement.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace LemonTaskManagement.Infra.Data.Write.Repositories;

public class CardsCommandRepository(LemonTaskManagementDbContext context) : ICardsCommandRepository
{
    public async Task<Card> CreateCardAsync(CreateCardCommand command)
    {
        var order = await GetNextCardOrderAsync(command.BoardColumnId);

        var card = new Card
        {
            Id = Guid.NewGuid(),
            BoardColumnId = command.BoardColumnId,
            Description = command.Description,
            Order = order,
            AssignedUserId = command.AssignedUserId
        };

        await context.Cards.AddAsync(card);
        await context.SaveChangesAsync();

        return card;
    }

    public async Task<bool> UserHasAccessToBoardAsync(Guid userId, Guid boardId)
    {
        return await context.BoardUsers
            .AnyAsync(bu => bu.UserId == userId && bu.BoardId == boardId);
    }

    public async Task<bool> BoardColumnExistsAsync(Guid boardColumnId, Guid boardId)
    {
        return await context.BoardColumns
            .AnyAsync(bc => bc.Id == boardColumnId && bc.BoardId == boardId);
    }

    public async Task<int> GetNextCardOrderAsync(Guid boardColumnId)
    {
        var maxOrder = await context.Cards
            .Where(c => c.BoardColumnId == boardColumnId)
            .MaxAsync(c => (int?)c.Order);

        return (maxOrder ?? 0) + 1;
    }

    public async Task<Card> GetCardByIdAsync(Guid cardId)
    {
        return await context.Cards.FirstOrDefaultAsync(c => c.Id == cardId);
    }

    public async Task<Card> MoveCardAsync(Guid cardId, Guid targetBoardColumnId, int targetOrder)
    {
        var card = await context.Cards.FirstOrDefaultAsync(c => c.Id == cardId);

        if (card == null)
            return null;

        card.BoardColumnId = targetBoardColumnId;
        card.Order = targetOrder;

        context.Cards.Update(card);
        await context.SaveChangesAsync();

        return card;
    }

    public async Task ReorderCardsAsync(Guid boardColumnId, int fromOrder)
    {
        var cardsToReorder = await context.Cards
            .Where(c => c.BoardColumnId == boardColumnId && c.Order >= fromOrder)
            .ToListAsync();

        foreach (var card in cardsToReorder)
        {
            card.Order += 1;
        }

        if (cardsToReorder.Any())
        {
            context.Cards.UpdateRange(cardsToReorder);
            await context.SaveChangesAsync();
        }
    }
}
