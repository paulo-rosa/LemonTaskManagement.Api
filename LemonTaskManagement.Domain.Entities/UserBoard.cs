using System;

namespace LemonTaskManagement.Domain.Entities
{
    public class UserBoard : DomainBase
    {
        public Guid UserId { get; set; }
        public Guid BoardId { get; set; }
    }
}
