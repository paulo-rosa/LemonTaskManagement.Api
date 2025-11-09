using System;
using System.Collections.Generic;

namespace LemonTaskManagement.Domain.Entities;

public class User : EntityBase
{
    public Guid Id { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public string PasswordHash { get; set; }
    
    public virtual ICollection<BoardUser> BoardUsers { get; set; }
}
