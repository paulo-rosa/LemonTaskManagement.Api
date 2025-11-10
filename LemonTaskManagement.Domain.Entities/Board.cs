using System;
using System.Collections.Generic;

namespace LemonTaskManagement.Domain.Entities;

public class Board : EntityBase
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    
    public virtual ICollection<BoardColumn> Columns { get; set; }
    public virtual ICollection<BoardUser> BoardUsers { get; set; }
}
