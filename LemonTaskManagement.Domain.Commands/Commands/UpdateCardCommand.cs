using LemonTaskManagement.Domain.Core.Models;
using System;

namespace LemonTaskManagement.Domain.Commands.Commands;

public class UpdateCardCommand
{
    public Guid UserId { get; set; }
    public Guid CardId { get; set; }
    public Guid BoardId { get; set; }
    public string Description { get; set; }
    public Guid? AssignedUserId { get; set; }
}

public class UpdateCardResponse : Response
{
    public UpdateCardDto Data { get; set; }
}

public class UpdateCardDto
{
    public Guid Id { get; set; }
    public Guid BoardColumnId { get; set; }
    public string Description { get; set; }
    public int Order { get; set; }
    public Guid? AssignedUserId { get; set; }
}
