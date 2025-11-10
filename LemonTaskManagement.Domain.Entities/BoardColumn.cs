using System;
using System.Collections.Generic;

namespace LemonTaskManagement.Domain.Entities;

public class BoardColumn : EntityBase
{
    public Guid Id { get; set; }
    
    public Guid BoardId { get; set; }
    
    public virtual Board Board { get; set; }
    
    public string Name { get; set; }
    
    public int Order { get; set; }
    
    public virtual ICollection<Card> Cards { get; set; }
}
