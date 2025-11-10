using System;
using System.Collections.Generic;

namespace LemonTaskManagement.Domain.Queries.DTOs
{
    public class BoardColumnDto
    {
        public Guid Id { get; set; }
        public Guid BoardId { get; set; }
        public string Name { get; set; }
        public int Order { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset? UpdatedAt { get; set; }
        public List<CardDto> Cards { get; set; }
    }
}
