using System;

namespace LemonTaskManagement.Domain.Queries.DTOs
{
    public class UserBoardDto
    {
        public Guid UserId { get; set; }
        public UserDto User { get; set; }
        public Guid BoardId { get; set; }
        public BoardDto Board { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
    }
}
