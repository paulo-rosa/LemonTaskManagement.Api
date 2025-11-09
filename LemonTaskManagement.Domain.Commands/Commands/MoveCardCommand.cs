using LemonTaskManagement.Domain.Core.Models;
using System;

namespace LemonTaskManagement.Domain.Commands.Commands;

public class MoveCardCommand
{
    public Guid UserId { get; set; }
    public Guid CardId { get; set; }
    public Guid BoardId { get; set; }
    public Guid TargetBoardColumnId { get; set; }
    public int TargetOrder { get; set; }

    public MoveCardCommand()
    {
    }

    public MoveCardCommand(Guid userId, Guid cardId, Guid boardId, Guid targetBoardColumnId, int targetOrder)
    {
        UserId = userId;
        CardId = cardId;
        BoardId = boardId;
        TargetBoardColumnId = targetBoardColumnId;
        TargetOrder = targetOrder;
    }
}

public class MoveCardResponse : Response<MoveCardDto>;

public class MoveCardDto
{
    public Guid Id { get; set; }
    public Guid BoardColumnId { get; set; }
    public string Description { get; set; }
    public int Order { get; set; }
    public Guid? AssignedUserId { get; set; }
}
