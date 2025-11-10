using System;

namespace LemonTaskManagement.Domain.Entities;

public class BoardUser : EntityBase
{
    public Guid UserId { get; set; }

    public virtual User User { get; set; }

    public Guid BoardId { get; set; }

    public virtual Board Board { get; set; }
}
