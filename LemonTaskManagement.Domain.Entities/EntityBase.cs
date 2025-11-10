using System;

namespace LemonTaskManagement.Domain.Entities;

public class EntityBase
{
    public DateTimeOffset CreatedAt { get; set; }
    public string CreatedBy { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
    public string? UpdatedBy { get; set; }
}
