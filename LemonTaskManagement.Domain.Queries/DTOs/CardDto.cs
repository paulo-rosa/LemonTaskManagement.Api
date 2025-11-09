using System;

namespace LemonTaskManagement.Domain.Queries.DTOs
{
    public class CardDto
    {
        public Guid Id { get; set; }
        public Guid BoardColumnId { get; set; }
        public string Description { get; set; }
        public int Order { get; set; }
        public Guid? AssignedUserId { get; set; }
        public UserDto AssignedUser { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }
    }
}
