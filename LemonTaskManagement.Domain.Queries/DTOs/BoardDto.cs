using System;
using System.Collections.Generic;

namespace LemonTaskManagement.Domain.Queries.DTOs
{
    public class BoardDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public List<BoardColumnDto> Columns { get; set; }
    }
}
