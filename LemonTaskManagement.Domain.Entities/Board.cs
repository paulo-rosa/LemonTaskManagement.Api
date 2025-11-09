using System;

namespace LemonTaskManagement.Domain.Entities;

public class Board : EntityBase
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
}
