namespace LemonTaskManagement.Domain.Queries.DTOs
{
    public class DatabaseSummaryDto
    {
        public int UsersCount { get; set; }
        public int BoardsCount { get; set; }
        public int UserBoardsCount { get; set; }
        public string Message { get; set; }
    }
}
