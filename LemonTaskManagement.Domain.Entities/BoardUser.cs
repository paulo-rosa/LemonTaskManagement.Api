using System;

namespace LemonTaskManagement.Domain.Entities;

public class BoardUser : EntityBase
{
    public Guid UserId { get; set; }
    public Guid BoardId { get; set; }
}
