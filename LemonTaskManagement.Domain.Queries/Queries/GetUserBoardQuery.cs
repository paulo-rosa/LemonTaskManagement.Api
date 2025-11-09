using LemonTaskManagement.Domain.Core.Models;
using LemonTaskManagement.Domain.Queries.DTOs;
using System;

namespace LemonTaskManagement.Domain.Queries.Queries;

public class GetUserBoardQuery
{
    public Guid UserId { get; set; }
    public Guid BoardId { get; set; }

    public GetUserBoardQuery()
    {
    }

    public GetUserBoardQuery(Guid userId, Guid boardId)
    {
        UserId = userId;
        BoardId = boardId;
    }
}

public class GetUserBoardResponse : Response<UserBoardDto>;
