using LemonTaskManagement.Domain.Core.Models;
using System;

namespace LemonTaskManagement.Domain.Commands.Commands;

public class CreateCardCommand
{
    public Guid UserId { get; set; }
    public Guid BoardId { get; set; }
    public Guid BoardColumnId { get; set; }
    public string Description { get; set; }
    public Guid? AssignedUserId { get; set; }

    public CreateCardCommand()
    {
    }

    public CreateCardCommand(Guid userId, Guid boardId, Guid boardColumnId, string description, Guid? assignedUserId = null)
    {
        UserId = userId;
        BoardId = boardId;
        BoardColumnId = boardColumnId;
        Description = description;
        AssignedUserId = assignedUserId;
    }
}

public class CreateCardResponse : Response<CreateCardDto>;

public class CreateCardDto
{
    public Guid Id { get; set; }
    public Guid BoardColumnId { get; set; }
    public string Description { get; set; }
    public int Order { get; set; }
    public Guid? AssignedUserId { get; set; }
}
