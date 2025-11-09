using System;

namespace LemonTaskManagement.Domain.Queries.DTOs
{
    public class BoardUserDto
    {
        public Guid UserId { get; set; }
        public Guid BoardId { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
    }
}
