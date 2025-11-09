using System;

namespace LemonTaskManagement.Domain.Entities;

public class Card : EntityBase
{
    public Guid Id { get; set; }

    public Guid BoardColumnId { get; set; }

    public virtual BoardColumn BoardColumn { get; set; }

    public string Description { get; set; }

    public int Order { get; set; }

    public Guid? AssignedUserId { get; set; }

    public virtual User AssignedUser { get; set; }
}
